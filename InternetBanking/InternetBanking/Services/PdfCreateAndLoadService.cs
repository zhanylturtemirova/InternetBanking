using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.rtf;
using iTextSharp.text.rtf.parser;


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InternetBanking.Models;
using InternetBanking.ViewModels;
using Microsoft.EntityFrameworkCore;
using InternetBanking.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System.Text;

using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using HtmlAgilityPack;
using iTextSharp.text.html;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.FileProviders;
using Document = iTextSharp.text.Document;


namespace InternetBanking.Services
{
    public class PdfCreateAndLoadService:ICreatePDFandLoad
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public string name;
        public PdfCreateAndLoadService(IHostingEnvironment _hostingEnvironment)
        {
            this._hostingEnvironment = _hostingEnvironment;
        }


        //public FileResult DownloadFile()
        //{
        //    string filePath = _hostingEnvironment.ContentRootPath;
        //    string filename = "Conversion.pdf";
        //    IFileProvider provider = new PhysicalFileProvider(filePath);
        //    IFileInfo fileInfo = provider.GetFileInfo(filename);
        //    var readStream = fileInfo.CreateReadStream();
        //    var mimeType = "application/pdf";
        //    return File(readStream, mimeType, filename);
        //}

        public string CreatePDF(PdfPTable table)
        {
            string name =CreateName();
            var document = new iTextSharp.text.Document();
            iTextSharp.text.pdf.PdfWriter.GetInstance(document, new FileStream(name, FileMode.Create));
            document.Open();
            document.Add(table);
            document.Close();
            return name;
        }
        public Font GetFont()
        {
            string filePath = _hostingEnvironment.ContentRootPath;
            BaseFont arial = BaseFont.CreateFont(filePath + @"\wwwroot\fonts\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font normal = new Font(arial, 10f, Font.NORMAL, BaseColor.Black);
            return normal;
        }

        public string ReturnName()
        {
            return name;
        }

        //private async Task<string> RenderPartialViewToString(string viewName, object model)
        //{
        //    if (string.IsNullOrEmpty(viewName))
        //        viewName = ControllerContext.ActionDescriptor.ActionName;
        //    ViewData.Model = model;
        //    using (var writer = new StringWriter())
        //    {
        //        ViewEngineResult viewResult =
        //            _viewEngine.FindView(ControllerContext, viewName, false);

        //        ViewContext viewContext = new ViewContext(
        //            ControllerContext,
        //            viewResult.View,
        //            ViewData,
        //            TempData,
        //            writer,
        //            new HtmlHelperOptions()
        //        );
        //        await viewResult.View.RenderAsync(viewContext);
        //        return writer.GetStringBuilder().ToString();
        //    }
        //}
        public PdfPTable TableForConvertion(ConvertConversionToDocViewModel docmodel)
        {
            Font normal = GetFont();
            PdfPTable table = new PdfPTable(3);
            PdfPCell cell = new PdfPCell(new Phrase("Конвертация валют", normal));
            cell.Colspan = 3;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);
            table.AddCell(new Phrase("Счет, с которого конвертируется сумма", normal));
            table.AddCell(new Phrase(docmodel.AccountFromNumber, normal));
            table.AddCell(new Phrase(docmodel.CurrencyFromName, normal));
            table.AddCell(new Phrase("Сумма", normal));
            cell = new PdfPCell(new Phrase(docmodel.AmountSend, normal));
            cell.Colspan = 2;
            table.AddCell(cell);
            table.AddCell(new Phrase("Счет, на который конвертируется сумма", normal));
            table.AddCell(new Phrase(docmodel.AccountToNumber));
            table.AddCell(new Phrase(docmodel.CurrencyToName));
            table.AddCell(new Phrase("Сконвертированная Сумма", normal));
            cell = new PdfPCell(new Phrase(docmodel.AmountReceive));
            cell.Colspan = 2;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Курсы, по которым проходит конвертация, определяются в момент совершения операции", normal));
            cell.Colspan = 3;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);
            table.AddCell(new Phrase("Курс 1 валюты относительно нац. валюты ", normal));
            table.AddCell(new Phrase(docmodel.CurrencyFromName));
            table.AddCell(new Phrase(docmodel.CurrencyFromRate));
            table.AddCell(new Phrase("Курс 2 валюты относительно нац. валюты ", normal));
            table.AddCell(new Phrase(docmodel.CurrencyToName));
            table.AddCell(new Phrase(docmodel.CurrencyToRate));
            cell = new PdfPCell(new PdfPCell(new Phrase("Дата", normal)));
            cell.Colspan = 2;
            table.AddCell(cell);
            table.AddCell(new Phrase(docmodel.Date));
            return table;
        }

        public PdfPTable TableForTransfer(PdfForExactTransferViewModel model)
        {
           
            Font normal = GetFont();
            PdfPTable table = new PdfPTable(3);
            PdfPCell cell = new PdfPCell(new Phrase("Переводы", normal));
            cell.Colspan = 3;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(model.FullName, normal));
            cell.Colspan = 3;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);
            table.AddCell(new Phrase("Отправитель", normal));
            cell = new PdfPCell(new Phrase(model.AccountSender, normal));
            cell.Colspan = 2;
            table.AddCell(cell);
            table.AddCell(new Phrase("Отправленная cумма ", normal));
            cell = new PdfPCell(new Phrase(model.Amount.ToString()));
            cell.Colspan = 2;
            table.AddCell(cell);
            table.AddCell(new Phrase("Получатель", normal));
            cell = new PdfPCell(new Phrase(model.AccountReceiver, normal));
            cell.Colspan = 2;
            table.AddCell(cell);
            table.AddCell(new Phrase("Комментарий", normal));
            cell = new PdfPCell(new Phrase(model.Comment, normal));
            cell.Colspan = 2;
            table.AddCell(cell);
            table.AddCell(new Phrase("Состояние", normal));
            cell = new PdfPCell(new Phrase(model.State));
            cell.Colspan = 2;
            table.AddCell(cell);
            cell = new PdfPCell(new PdfPCell(new Phrase("Дата", normal)));
            table.AddCell(cell);
            cell = new PdfPCell(new PdfPCell(new Phrase(model.TransferDate.ToString())));
            cell.Colspan = 2;
            table.AddCell(cell);
            return table;
        }

        public string CreateName( )
        {
            string transferDate = DateTime.Now.ToString();
            transferDate = transferDate.Replace('.', '_').Replace(':', '_').Replace(' ', '_');
            string fileName =transferDate + ".pdf";
            return fileName;
        }
    }
}
