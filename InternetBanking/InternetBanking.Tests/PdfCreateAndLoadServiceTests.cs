using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using InternetBanking.Services;
using InternetBanking.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Moq;
using Xunit;

namespace InternetBanking.Tests
{
    public class PdfCreateAndLoadServiceTests
    {
        private readonly ICreatePDFandLoad pdfCreateAndLoadServiceTests;
        ConvertConversionToDocViewModel model = new ConvertConversionToDocViewModel()
        {
            AccountToNumber = "123456789123",
            AccountFromNumber = "123498789123",
            CurrencyFromName = "KGS",
            CurrencyToName = "USD",
            AmountSend = "3584",
            AmountReceive = "54",
            CurrencyFromRate = "68",
            CurrencyToRate = null,
            Date = DateTime.Now.ToString(),
        };
        //private IHostingEnvironment hostingEnvironment;
        public PdfCreateAndLoadServiceTests()
        {
            var hostingEnvironment = new Mock<IHostingEnvironment>();
            IHostingEnvironment hs = new HostingEnvironment();
            //var str = hs.ContentRootPath;
          
            //hostingEnvironment.Setup(h => h.ContentRootPath).Returns();
            pdfCreateAndLoadServiceTests = new PdfCreateAndLoadService(hostingEnvironment.Object);

        }
        [Fact]
        public void GetFont()
        {
            var x = Path.Combine(Directory.GetCurrentDirectory());
            string filePath = x.Replace("\\bin\\Debug\\netcoreapp2.0", "");
            //string filePath = x.Replace("\\InternetBanking.Tests\\bin\\Debug\\netcoreapp2.0", "");
            //BaseFont arial = BaseFont.CreateFont(filePath + @"\wwwroot\fonts\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            BaseFont arial = BaseFont.CreateFont(filePath + @"\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font normal = new Font(arial, 10f, Font.NORMAL, BaseColor.Black);
            Assert.IsType(typeof(Font), normal);
        }

        [Fact]
        public void GetTable()

        {
            var table = pdfCreateAndLoadServiceTests.TableForConvertion(model);
            Assert.IsType(typeof(PdfPTable), table);
        }

        [Fact]
        public void CreatePDF()
        {
            var pdf = pdfCreateAndLoadServiceTests.CreatePDF(pdfCreateAndLoadServiceTests.TableForConvertion(model));
            Assert.IsType(typeof(Document), pdf);
        }

        
    }
}
