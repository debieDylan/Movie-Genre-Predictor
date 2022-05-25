using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectML_Models.Models
{
    public abstract class BaseClass : IDataErrorInfo, INotifyPropertyChanged
    {
        public abstract string this[string columnName] { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsValid()
        {
            return string.IsNullOrWhiteSpace(Error);
        }
        [LoadColumn(18)]
        public string Error
        {
            get
            {
                string errorMessages = "";

                foreach (var item in this.GetType().GetProperties()) //reflection 
                {

                    string error = this[item.Name];
                    if (!string.IsNullOrWhiteSpace(error))
                    {
                        errorMessages += error + Environment.NewLine;
                    }
                }
                return errorMessages;
            }
        }
    }
}
