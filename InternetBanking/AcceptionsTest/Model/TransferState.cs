using System.ComponentModel.DataAnnotations;

namespace AcceptionsTest.Model
{
    public class TransferState
    {
        [Key]
        public int Id { get; set; }

        public string StateName { get; set; }

        public static TransferState Create(TransferStatesEnum enumItem)
        {
            return new TransferState { StateName = enumItem.ToString() };
        }

        public bool IsEqual(TransferStatesEnum enumItem)
        {
            return this.StateName == enumItem.ToString();
        }
    }
    public enum TransferStatesEnum
    {
        Confirmed = 1,
        NotConfirmed = 2,
        Canceled = 3,
        BalanceNotEnough = 4,
        AccountIsLocked = 5
    }
}