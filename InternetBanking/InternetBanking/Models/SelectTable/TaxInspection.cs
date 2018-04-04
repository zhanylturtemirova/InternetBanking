using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.Models.SelectTable
{
    public class TaxInspection
    {
        [Key]
        public int Id { get; set; }
        public string TaxInspectionName { get; set; }
    }
}
