using System.ComponentModel.DataAnnotations;

namespace AcceptionsTest.Model
{
    public class TypeOfTransfer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public static TypeOfTransfer Create(TypeOfTransfersEnum enumItem)
        {
            return new TypeOfTransfer { Name = enumItem.ToString() };
        }

        public bool IsEqual(TypeOfTransfersEnum enumItem)
        {
            return this.Name == enumItem.ToString();
        }
    }
    public enum TypeOfTransfersEnum
    {
        InnerTransfer = 1,
        InterBankTransfer = 2,
        Conversion = 3
    }
}