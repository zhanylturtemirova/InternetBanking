using System.ComponentModel.DataAnnotations;

namespace AcceptionsTest.Model
{
    public class BankInfo
    {
        [Key]
        public int Id { get; set; }
        public string BankName { get; set; }
        public string Email { get; set; }
    }
}