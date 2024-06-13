namespace ViewQuickApp.Server.Core.Entities
{
    public class Message : BaseEntity<long>
    {
        public string SenderUserName { get; set; }

        public string ReciverUserName { get; set; }    

        public string message {  get; set; }
    }
}
