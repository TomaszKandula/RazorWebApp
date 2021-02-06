using System.ComponentModel.DataAnnotations;

namespace RazorWebApp.Infrastructure.Domain
{
    public class Entity<TKey>
    {
        [Key]
        public int Id { get; set; }
    }
}
