using ProjectML_Models.Models;
using ProjectML_Models.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.ML.Data;

namespace ProjectML_WPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private FilmData _film;
        private FilmData _filmRecord;
        private string _result;
        private Visibility _loading;
        private bool _enabled;
        public MainViewModel()
        {
            _film = new FilmData()
            {
                original_title = "The Conjuring",
                description = "The Perron family moves into a farmhouse where they experience paranormal phenomena. They consult demonologists, Ed and Lorraine Warren, to help them get rid of the evil entity haunting them.",
                production_company = "New Line Cinema"
            };
            FilmRecord = new FilmData();
            Loading = Visibility.Collapsed;
            Enabled = true;
        }
        #region Properties
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                NotifyPropertyChanged();
            }
        }
        public Visibility Loading
        {
            get { return _loading; }
            set
            {
                _loading = value;
                NotifyPropertyChanged();
            }
        }
        public string Result
        {
            get { return _result; }
            set
            {
                _result = value;
                NotifyPropertyChanged();
            }
        }
        public FilmData Film
        {
            get { return _film; }
            set
            {
                _film = value;
                NotifyPropertyChanged();
            }
        }
        public FilmData FilmRecord
        {
            get { return _filmRecord; }
            set
            {
                _filmRecord = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region ICommand functies
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override async void Execute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "BuildModel": await BuildModelAsync(); break;
                case "PredictFilm": await PredictFilmAsync(); break;
            }
        }

        private async Task PredictFilmAsync()
        {
            if (FilmRecord.IsGeldig())
            {
                Loading = Visibility.Visible;
                Enabled = false;
                string result = await Task.Run(() => Builder.PredictFilm(FilmRecord));
                Result = result;
                Loading = Visibility.Collapsed;
                Enabled = true;
            }
            else
            {
                MessageBox.Show(FilmRecord.Error);
            }
        }

        private async Task BuildModelAsync()
        {
            Loading = Visibility.Visible;
            Enabled = false;
            string result = await Task.Run(() => Builder.BuildModel());
            Result = result;
            Loading = Visibility.Collapsed;
            Enabled = true;
        }
        #endregion
    }
}
