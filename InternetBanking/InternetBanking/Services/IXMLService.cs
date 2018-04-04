using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InternetBanking.Services
{
    public interface IXmlService
    {
        void LoadFile();
        List<NbkrRatesXMLViewModel> ConvertXmlToList(string xml);
        string XmlToString();
    }
}
