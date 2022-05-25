using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectML_Models.Models
{
    public partial class FilmData : BaseClass
    {
        public override string this[string columnName]
        {
            get
            {
                if (columnName == "original_title" && string.IsNullOrWhiteSpace(original_title))
                {
                    return "The 'title' is mandatory!";
                }
                if (columnName == "director" && string.IsNullOrWhiteSpace(director))
                {
                    return "The 'director' is mandatory!";
                }
                if (columnName == "description" && string.IsNullOrWhiteSpace(description))
                {
                    return "The 'description' is mandatory!";
                }
                return "";
            }
        }
    }
}
