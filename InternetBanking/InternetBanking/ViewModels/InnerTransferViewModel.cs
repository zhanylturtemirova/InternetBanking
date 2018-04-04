using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InternetBanking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Azure.KeyVault.Models;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace InternetBanking.ViewModels
{
    public class InnerTransferViewModel
    {

        public SelectList UserAccounts { get; set; }

        [Display(Name = "AccountSenderName")]
        [Required(ErrorMessage = "RequiredErrorMessage")]
        public int? AccountSenderId { get; set; }


        [Required(ErrorMessage = "RequiredErrorMessage")]
        [MinLength(16, ErrorMessage = "AccountLength")]
        [MaxLength(16, ErrorMessage = "AccountLength")]
        [Range(0, Int64.MaxValue)]
        [Display(Name = "ReceiverAccountName")]
        public string ReceiverAccountNumber { get; set; }


        [Required(ErrorMessage = "RequiredErrorMessage")]
        [Display(Name = "AmountName")]
        public string Amount { get; set; }
       
        [Required(ErrorMessage = "RequiredErrorMessage")]
        [Display(Name = "CommentName")]
        public string Comment { get; set; }

        [Display(Name = "SaveInTempalte")]
        public bool SaveInTempalte { get; set; }

        public int? TemplateId { get; set; }
        public Template Template { get; set; }


    }
}
