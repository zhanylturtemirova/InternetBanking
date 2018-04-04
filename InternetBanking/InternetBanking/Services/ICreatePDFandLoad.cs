using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using InternetBanking.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InternetBanking.Services
{
    public interface ICreatePDFandLoad
    {

        // FileResult DownloadFile();
        string CreatePDF(PdfPTable table);
        Font GetFont();
        PdfPTable TableForConvertion(ConvertConversionToDocViewModel docmodel);
        PdfPTable TableForTransfer(PdfForExactTransferViewModel model);
        string CreateName();
        string ReturnName();
        // Task<string> RenderViewToString(string viewName, object model);
    }
}
