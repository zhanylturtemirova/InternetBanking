namespace AcceptionsTest.Model
{
    public class InterBankTransfer
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public string ReciverName { get; set; }
        public int? BankId { get; set; }
        public Bank Bank { get; set; }
        public int? PaymentCodeId { get; set; }
        public PaymentСode PaymentСode { get; set; }
        public int InnerTransferId { get; set; }
        public InnerTransfer Transfer { get; set; }
    }
}