using System;

namespace RazorWebApp.Infrastructure.Domain.Entities
{
    public class SigninHistory : Entity<int>
    {
        public int UserId { get; set; }

        public DateTime LoggedAt { get; set; }

        public virtual Users User { get; set; }
    }
}
