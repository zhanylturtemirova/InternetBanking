using System.ComponentModel.DataAnnotations;

namespace AcceptionsTest.Model
{
    public class IntervalType
    {
        [Key]
        public int Id { get; set; }

        public int IntervalCode { get; set; }
        public string IntervalName { get; set; }

        public static IntervalType Create(IntervalTypesEnum enumItem)
        {
            IntervalType type = new IntervalType
            {
                IntervalCode = (int)enumItem,
                IntervalName = enumItem.ToString(),
            };
            return type;
        }

        public bool IsEqual(IntervalTypesEnum enumItem)
        {
            return this.IntervalName == enumItem.ToString();
        }
    }
    public enum IntervalTypesEnum
    {
        OnceADay = 1,
        OnceAWeek = 2,
        OnceInTwoWeeks = 3,
        OnceAMonth = 4,
        OnceAQuarter = 5,
        OnceAHalfYear = 6,
        OnceAYear = 7
    }
}