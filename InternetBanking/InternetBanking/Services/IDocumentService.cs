using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using InternetBanking.ViewModels;

namespace InternetBanking.Services
{
    public interface IDocumentService
    {
        DocumentType CreateFile(AddDocumentFileViewModel model);
    }
}
