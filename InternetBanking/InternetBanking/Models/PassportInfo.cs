using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.Models
{
    public class PassportInfo
    {
        [Key]
        public int Id { get; set; }
        public int TypeOfDocumentId { get; set; }
        [ForeignKey("TypeOfDocumentId")]
        public TypeOfDocument TypeOfDocument { get; set; }
        public DateTime DateofExtradition { get; set; }
        public string Number { get; set; }
        public string IssuedBy { get; set; }
        public DateTime Validaty { get; set; }
        public string Series { get; set; }
       

    }
}
