using System.ComponentModel.DataAnnotations;

namespace SecureWebApp.Infrastructure.Domain
{
    public class Entity<TKey>
    {
        [Key]
        public int Id { get; set; }
    }
}
