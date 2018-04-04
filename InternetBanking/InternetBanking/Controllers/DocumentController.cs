using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.Services;
using InternetBanking.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InternetBanking.Controllers
{
    public class DocumentController : Controller
    {
        private readonly ISelectListService selectListService;
        private readonly IDocumentService documentService;

        public DocumentController(ISelectListService selectListService, IDocumentService documentService)
        {
            this.selectListService = selectListService;
            this.documentService = documentService;
        }
        [HttpGet]
        public IActionResult AddDocument()
        {
            AddDocumentFileViewModel model = new  AddDocumentFileViewModel{TypeOfTransfer = selectListService.GetTypeOfTransfers()};
            return View(model);
        }

        [HttpPost]
        public IActionResult AddDocument(AddDocumentFileViewModel model)
        {
            if (ModelState.IsValid)
            {
                documentService.CreateFile(model);
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                model.TypeOfTransfer = selectListService.GetTypeOfTransfers();
                return View(model);
            }
        }
    }

}