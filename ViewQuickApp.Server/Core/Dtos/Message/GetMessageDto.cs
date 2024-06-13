namespace ViewQuickApp.Server.Core.Dtos.Message
{
    public class GetMessageDto
    {

        public long Id { get; set; }

        public string SenderUserName { get; set; }

        public string ReceiverUserNaem {  get; set; }

        public string Text { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.Now;
    }
}
