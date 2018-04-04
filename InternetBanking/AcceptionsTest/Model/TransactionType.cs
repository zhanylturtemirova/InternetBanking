using System.ComponentModel.DataAnnotations;

namespace AcceptionsTest.Model
{
    public class TransactionType
    {

        [Key]
        public int Id { get; set; }

        public string TypeName { get; set; }

        public static TransactionType Create(TransactionTypesEnum enumItem)
        {
            return new TransactionType { TypeName = enumItem.ToString() };
        }


        public bool IsEqual(TransactionTypesEnum enumItem)
        {
            return this.TypeName == enumItem.ToString();
        }
    }
    public enum TransactionTypesEnum
    {
        Debit = 0,
        Credit = 1
    }
}