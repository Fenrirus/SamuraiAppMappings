using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace SamuraiAppMappings.Domain
{
    public class Battle
    {
        public Battle()
        {
            SamuraiBattles = new List<SamuraiBattle>();
        }

        public DateTime EndDate { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SamuraiBattle> SamuraiBattles { get; set; }
        public DateTime StartDate { get; set; }
    }
}