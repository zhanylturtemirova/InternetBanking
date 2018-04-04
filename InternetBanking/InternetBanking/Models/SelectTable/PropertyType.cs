using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.Models.SelectTable
{
    public class PropertyType
    {
        [Key]
        public int Id { get; set; }
        public string PropertyTypeName { get; set; }
    }
}
