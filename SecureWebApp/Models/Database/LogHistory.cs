using System;

namespace SecureWebApp.Models.Database
{
    public partial class LogHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime LoggedAt { get; set; }
        public Guid SessionId { get; set; }

        public virtual Users User { get; set; }
    }
}
