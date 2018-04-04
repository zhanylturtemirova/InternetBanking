namespace AcceptionsTest.Model
{
    public class DocumentType
    {
        public int Id { get; set; }
        public string FileDocument { get; set; }
        public int? TypeOfTransferId { get; set; }
        public TypeOfTransfer TypeOfTransfer { get; set; }
    }
}