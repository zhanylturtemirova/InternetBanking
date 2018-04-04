using System.ComponentModel.DataAnnotations;

namespace AcceptionsTest.Model
{
    public class Bank
    {
        public Bank(int id, string bik, int bankInfoId)
        {
            Id = id;
            BIK = bik;
            BankInfoId = bankInfoId;
        }

        public Bank()
        {
        }

        [Key]
        public int Id { get; set; }

        public string BIK { get; set; }
        public int BankInfoId { get; set; }
        public BankInfo BankInfo { get; set; }
    }
}