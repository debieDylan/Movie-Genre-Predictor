using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;
using ProjectML_Models.Models;
using System.IO;
using static Microsoft.ML.DataOperationsCatalog;
using Microsoft.ML.Transforms.Text;
using Microsoft.ML.Data;

namespace ProjectML_Models.Builder
{
    public static class Builder
    {
        #region Globale variabels
        static readonly string _dataPath = Path.Combine(Environment.CurrentDirectory, "Data", "Dylan_Dataset.csv");
        private static string _appPath => Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
        private static string _modelPath => Path.Combine(Environment.CurrentDirectory, "BuildModel", "model_Genre.zip");


        private static MLContext _mlContext = new MLContext(seed: 111);
        private static PredictionEngine<FilmData, FilmPrediction> _predEngine;
        private static ITransformer _trainedModel;
        static IDataView _trainingDataView;
        static IDataView _testDataView;
        #endregion

        public static string BuildModel()
        {
            //load data
            TrainTestData splitDataView = LoadData(_mlContext);
            _trainingDataView = splitDataView.TrainSet;
            _testDataView = splitDataView.TestSet;

            //process data
            var pipeline = ProcessData();

            //train data
            BuildAndTrainModel(_trainingDataView, pipeline);

            //evaluate data
            string evaluate = Evaluate(_trainingDataView.Schema);

            return evaluate;
        }

        #region Data opladen
        private static TrainTestData LoadData(MLContext mlContext)
        {
            IDataView dataView = mlContext.Data.LoadFromTextFile<FilmData>(_dataPath, hasHeader: true, separatorChar: ';');
            TrainTestData splitDataView = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);

            return splitDataView;
        }
        #endregion

        #region Data transformeren en  trainen
        private static IEstimator<ITransformer> ProcessData()
        {
            var pipeline = _mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "genre", outputColumnName: "Label")
                .Append(_mlContext.Transforms.Text.NormalizeText(inputColumnName: "description", outputColumnName: "description"))
                .Append(_mlContext.Transforms.Text.NormalizeText(inputColumnName: "original_title", outputColumnName: "original_title"))
                .Append(_mlContext.Transforms.Text.NormalizeText(inputColumnName: "director", outputColumnName: "director"))
                .Append(_mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "description", outputColumnName: "Value"))
                .Append(_mlContext.Transforms.Text.TokenizeIntoWords(inputColumnName: "description", outputColumnName: "description", separators: new[] { ' ', '.', ',' }))
                .Append(_mlContext.Transforms.Text.RemoveDefaultStopWords("description", "description", StopWordsRemovingEstimator.Language.English))
                .Append(_mlContext.Transforms.Text.FeaturizeText(inputColumnName: "original_title", outputColumnName: "TitleFeaturized"))
                .Append(_mlContext.Transforms.Text.FeaturizeText(inputColumnName: "description", outputColumnName: "DescriptionFeaturized"))
                .Append(_mlContext.Transforms.Text.FeaturizeText(inputColumnName: "director", outputColumnName: "DirectorFeaturized"))
                .Append(_mlContext.Transforms.Concatenate("Features", "TitleFeaturized", "DescriptionFeaturized", "DirectorFeaturized"))
                .AppendCacheCheckpoint(_mlContext);

            return pipeline;
        }

        private static void BuildAndTrainModel(IDataView trainingDataView, IEstimator<ITransformer> pipeline)
        {
            var trainingPipeline = pipeline
                .Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
                .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"))
                .Append(_mlContext.Transforms.Conversion.MapKeyToValue(inputColumnName: "Value", outputColumnName: "description")); //veranderen columntype <vector>string naar system.string

            _trainedModel = trainingPipeline.Fit(trainingDataView);
        }
        #endregion

        #region Getrained Model Evalueren en opslagen
        private static string Evaluate(DataViewSchema trainingDataViewSchema)
        {
            var testDataView = _testDataView;

            var testMetrics = _mlContext.MulticlassClassification.Evaluate(_trainedModel.Transform(testDataView));

            string result =
                $"*************************************************************************************************************\n" +
                $"*       Metrics for Multi-class Classification model - Test Data     \n" +
                $"*------------------------------------------------------------------------------------------------------------\n" +
                $"*       MicroAccuracy:    {testMetrics.MicroAccuracy:0.###}\n" +
                $"*       MacroAccuracy:    {testMetrics.MacroAccuracy:0.###}\n" +
                $"*       LogLoss:          {testMetrics.LogLoss:#.###}\n" +
                $"*       LogLossReduction: {testMetrics.LogLossReduction:#.###}\n" +
                $"*************************************************************************************************************";

            SaveModelAsFile(_mlContext, trainingDataViewSchema, _trainedModel);

            return result;
        }
        private static void SaveModelAsFile(MLContext mlContext, DataViewSchema trainingDataViewSchema, ITransformer model)
        {
            mlContext.Model.Save(model, trainingDataViewSchema, _modelPath);
        }
        #endregion

        #region Film Voorspellen
        public static string PredictFilm(FilmData Film)
        {
            ITransformer loadedModel = _mlContext.Model.Load(_modelPath, out var modelInputSchema);

            _predEngine = _mlContext.Model.CreatePredictionEngine<FilmData, FilmPrediction>(loadedModel);

            //haalt alle labels op die voorspeld kunnen worden
            var labels = AllScores(_predEngine);

            //voorspelt de film
            var prediction = _predEngine.Predict(Film);

            //haaalt hoogste score op
            var index = Array.IndexOf(labels, prediction.Prediction);
            //var score = prediction.Score[index];
            float score = 0;

            //haalt top 10 labels op
            var labelList = labels.ToDictionary(x => x, x => prediction.Score[Array.IndexOf(labels, x)]).OrderByDescending(x => x.Value).Take(10);

            var text = $"{prediction.Prediction}";
            //kunstmatige accuraatheid; als labelscore < 80%; 2de hoogste genre toevoegen, etc totdat score > 80
            foreach (var item in labelList)
            {
                if (score > 0.8)
                {
                    break;
                }
                if (item.Key != prediction.Prediction)
                {
                    text += $"/{item.Key}";
                }
                score += item.Value;
            }

            //string voorspelling = $"=============== Single Prediction - Result: {prediction.Prediction} ===============";
            string voorspelling = $"=============== Single Prediction - Result: {text} ===============";

            return voorspelling;
        }
        #endregion

        #region Methodes
        public static string[] AllScores(PredictionEngine<FilmData, FilmPrediction> predEngine)
        {
            var labelBuffer = new VBuffer<ReadOnlyMemory<char>>();
            predEngine.OutputSchema["Score"].Annotations.GetValue("SlotNames", ref labelBuffer);

            var labels = labelBuffer.DenseValues().Select(x => x.ToString()).ToArray();

            return labels;
        }
        #endregion
    }
}
