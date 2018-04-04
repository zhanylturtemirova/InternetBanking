using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InternetBanking.ViewModels
{
    public class AddDocumentFileViewModel
    {
        public int? TypeOfTransferId { get; set; }
        public SelectList TypeOfTransfer { get; set; }
        public IFormFile FileDocument { get; set; }
    }
}
