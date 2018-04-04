using System.ComponentModel.DataAnnotations;

namespace AcceptionsTest.Model
{
    public class Country
    {
        [Key]
        public int Id { get; set; }
        public string CountryName { get; set; }
    }
}