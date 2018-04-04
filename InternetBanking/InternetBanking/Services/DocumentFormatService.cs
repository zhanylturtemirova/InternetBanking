using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace InternetBanking.Services
{
    public class DocumentFormatService : IDocumentFormatService
    {
        private readonly IHostingEnvironment _appEnvironment;

        public DocumentFormatService(IHostingEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        public string[] CreateExcelStatement(AccountTransfersViewModel model)
        {
            var fileName = "Выписка-" + model.FullName + DateTime.Now.ToString("-yyyy-MM-dd--hh-mm-ss") + ".xlsx";
            var path = Path.Combine(
                _appEnvironment.WebRootPath,
                $"statements\\");
            // Create the file using the FileInfo object
            var file = new FileInfo(path + fileName);
            using (var package = new ExcelPackage(file))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sales list - " + DateTime.Now.ToShortDateString());

                // --------- Data and styling goes here -------------- //
                worksheet.Cells[1, 1].Value = "Клиент";
                worksheet.Cells[2, 1].Value = "Номер счета";
                worksheet.Cells[3, 1].Value = "Период";

                // Add the second row of header data
                worksheet.Cells[1, 2].Value = model.FullName;
                worksheet.Cells[2, 2].Value = string.Format("{0},{1}", model.Account.Number, model.Account.Currency.Name);
                worksheet.Cells[3, 2].Value = string.Format("{0} - {1}", model.FromDate, model.ToDate);

                worksheet.Cells[4, 1].Value = "№";
                worksheet.Cells[4, 2].Value = "Дата";
                worksheet.Cells[4, 3].Value = "Счет-кореспондент";
                worksheet.Cells[4, 4].Value = "Расход";
                worksheet.Cells[4, 5].Value = "Приход";
                worksheet.Cells[4, 6].Value = "Назначение платежа";
                worksheet.Cells[4, 7].Value = "Курс";
                int row = 4;
                int number = 0;
                foreach (StatementObjectViewModel transfer in model.Transfers)
                {
                    row = row + 1;
                    number = number + 1;
                    worksheet.Cells[row, 1].Value = number;
                    worksheet.Cells[row, 2].Value = transfer.TransferDate.ToString("d");
                    worksheet.Cells[row, 3].Value = transfer.AccountNumber;
                    worksheet.Cells[row, 4].Value = transfer.CreditAmount;
                    worksheet.Cells[row, 5].Value = transfer.DebitAmount;
                    worksheet.Cells[row, 6].Value = transfer.Comment;
                    worksheet.Cells[row, 7].Value = transfer.Rate;

                }

                worksheet.Column(1).Width = 13;
                worksheet.Column(2).Width = 20;
                worksheet.Column(3).Width = 21;
                worksheet.Column(4).Width = 14;
                worksheet.Column(5).Width = 14;
                worksheet.Column(6).Width = 26;
                worksheet.Column(7).Width = 17;
                package.Save();


            }
            string file_path = Path.Combine(_appEnvironment.ContentRootPath, path + fileName);
            string file_type = "application/xlsx";
            string[] dFile = {file_path, file_type, fileName};
            return dFile;
        }

       


    }
}
