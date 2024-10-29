namespace WebApp.Models
{
    public class AuditTrail
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string MenuAccessed { get; set; }
        public string Activity { get; set; }
        public DateTime ActivityTimestamp { get; set; }
        public string DataChanges { get; set; }
    }

}
