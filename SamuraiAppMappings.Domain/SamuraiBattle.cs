using Microsoft.EntityFrameworkCore;

namespace SamuraiAppMappings.Domain
{
    public class SamuraiBattle
    {
        public Battle Battle { get; set; }
        public int BattleId { get; set; }
        public Samurai Samurai { get; set; }
        public int SamuraiId { get; set; }
    }
}