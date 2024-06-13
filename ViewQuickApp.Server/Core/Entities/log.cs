namespace ViewQuickApp.Server.Core.Entities
{
    public class log : BaseEntity<long>
    {
        public string? UserName { get; set; }

        public string Description { get; set; }


    }
}
