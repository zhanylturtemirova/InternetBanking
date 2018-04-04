using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcceptionsTest.Model
{
    public class PaymentSchedule
    {
        [Key]
        public int Id { get; set; }

        public int TemplateId { get; set; }
        public Template Template { get; set; }

        public int IntervalTypeId { get; set; }
        public IntervalType IntervalType { get; set; }

        public DateTime NextPaymentDate { get; set; }

        public DateTime Start { get; set; }
        public DateTime? Finish { get; set; }

        public int DeferredPaymentCounter { get; set; }
    }
}
