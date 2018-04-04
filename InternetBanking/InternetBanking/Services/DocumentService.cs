using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.ViewModels;
using Microsoft.AspNetCore.Hosting;

namespace InternetBanking.Services
{
    public class DocumentService : IDocumentService
    {
        private ApplicationContext context;
        private IHostingEnvironment environment;
        private FileUploadService fileUploadService;

        public DocumentService(ApplicationContext context, IHostingEnvironment environment,
            FileUploadService fileUploadService)
        {
            this.environment = environment;
            this.fileUploadService = fileUploadService;
            this.context = context;
        }

        public DocumentType CreateFile(AddDocumentFileViewModel model)
        {
            string filePath = String.Empty;
            if (model.FileDocument != null)
            {
                var path = Path.Combine(
                    environment.WebRootPath,
                    $"file\\{model.FileDocument.FileName}\\FileDocument");
                fileUploadService.Upload(path, model.FileDocument.FileName, model.FileDocument);
                filePath = $"file/{model.FileDocument.FileName}/FileDocument/{model.FileDocument.FileName}";
            }

            DocumentType file = new DocumentType
            {
                TypeOfTransferId = model.TypeOfTransferId,
                FileDocument = filePath
            };
            context.Documents.Add(file);
            context.SaveChanges();
            return file;
        }
    }
}
