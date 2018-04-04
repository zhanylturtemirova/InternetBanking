using System.ComponentModel.DataAnnotations;

namespace AcceptionsTest.Model
{
    public class AddressType
    {
        [Key]
        public int Id { get; set; }

        public string TypeName { get; set; }

        public static AddressType Create(AddressTypesEnum enumItem)
        {
            return new AddressType { TypeName = enumItem.ToString() };
        }

        public bool IsEqual(AddressTypesEnum enumItem)
        {
            return this.TypeName == enumItem.ToString();
        }
        public enum AddressTypesEnum
        {
            FactAddress = 1,
            LegalAddress = 2,
            BirthAddress = 3
        }
    }
}