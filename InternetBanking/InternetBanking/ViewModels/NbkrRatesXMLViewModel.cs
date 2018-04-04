using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.ViewModels
{
    public class NbkrRatesXMLViewModel
    {
        public string Name;

        public string Value;
        //public string StylesheetFilename { get; set; }

        //public Dictionary<string, string> Parameters { get; protected set; }

        public NbkrRatesXMLViewModel(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public NbkrRatesXMLViewModel()
        {
        }
    }
}
