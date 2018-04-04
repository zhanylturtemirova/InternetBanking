using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models.SelectTable;

namespace InternetBanking.Models
{
    public class DocumentType
    {
        public int Id { get; set; }
        public string FileDocument { get; set; }
        public int? TypeOfTransferId { get; set; }
        public TypeOfTransfer TypeOfTransfer { get; set; }
    }
}
