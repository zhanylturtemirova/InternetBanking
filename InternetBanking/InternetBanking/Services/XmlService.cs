using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using InternetBanking.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InternetBanking.Services
{
    public class XmlService:IXmlService
    {
        public void LoadFile()
        {
            XmlDocument document = new XmlDocument();
            document.Load("http://www.nbkr.kg/XML/daily.xml");
            document.Save(Path.Combine(
                ("wwwroot/temporary"), "exchangeratesNBKR.xml"));
        }

        public List<NbkrRatesXMLViewModel> ConvertXmlToList(string xml)
        {
            List<NbkrRatesXMLViewModel> mylist = new List<NbkrRatesXMLViewModel>();

            try
            {
                XDocument xDocument = XDocument.Parse(xml);
                XElement root = xDocument.Root;
                foreach (var ele in root.Elements())
                {
                    if (ele.HasElements)
                    {
                        NbkrRatesXMLViewModel currRate = new NbkrRatesXMLViewModel();
                        currRate.Name = ele.LastAttribute.Value;
                        foreach (var x in ele.Elements())
                        {
                            if (x.Name == "Value")
                            {
                                currRate.Value = x.Value;
                            }
                        }
                        mylist.Add(currRate);
                    }
                }
            }
            catch (Exception e)
            {
                return mylist;
            }
            return mylist;
        }

        public string XmlToString()
        {
            LoadFile();
           string ratesXML = System.IO.File.ReadAllText(@"wwwroot\temporary\exchangeratesNBKR.xml");
            return ratesXML;
        }

       
    }
}
