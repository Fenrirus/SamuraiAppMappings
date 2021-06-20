using Microsoft.EntityFrameworkCore;

namespace SamuraiAppMappings.Domain
{
    public class Quote
    {
        public int Id { get; set; }
        public Samurai Samurai { get; set; }
        public int SamuraiId { get; set; }
        public string Text { get; set; }
    }
}