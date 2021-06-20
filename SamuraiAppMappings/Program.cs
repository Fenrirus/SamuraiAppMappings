using Microsoft.EntityFrameworkCore;
using SamuraiAppMappings.Data;
using SamuraiAppMappings.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SamuraiAppMappings
{
    internal class Program
    {
        private static SamuraiContext _context = new SamuraiContext();

        private static void AddSamuraiWithBetterName()
        {
            var samurai = new Samurai() { BetterName = new PersonFullName("Rob", "K"), Name = "Rob K" };
            _context.Add(samurai);
            _context.SaveChanges();
        }

        private static void AddSamuraiWithSectertIdentity()
        {
            var samurai = new Samurai { Name = "RobertSan" };
            samurai.SecretIdentity = new SecretIdentity { RealName = "Robert" };
            _context.Add(samurai);
            _context.SaveChanges();
        }

        private static void AddSecretIdentitBySamuraiId()
        {
            var secretIdentity = new SecretIdentity { SamuraiId = 1 };
            _context.Add(secretIdentity);
            _context.SaveChanges();
        }

        private static void AddSecretIdentitBySamuraiIdUntracked()
        {
            Samurai samurai;
            using (var diffcontext = new SamuraiContext())
            {
                samurai = diffcontext.Samurais.Find(2);
            }
            samurai.SecretIdentity = new SecretIdentity { RealName = "Robert" };
            _context.Attach(samurai);
            _context.SaveChanges();
        }

        private static void CreateThenEditSamuraiWithQuote()
        {
            var samurai = new Samurai { Name = "Ronin" };
            var quote = new Quote { Text = "Aren't I MARVELous?" };
            samurai.Quotes.Add(quote);
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
            quote.Text += " See what I did there?";
            _context.SaveChanges();
        }

        private static void CreteaSamurai()
        {
            var samurai = new Samurai() { Name = "Marta" };
            _context.Samurais.Add(samurai);
            var time = DateTime.Now;
            _context.Entry(samurai).Property("Created").CurrentValue = time;
            _context.Entry(samurai).Property("LastModified").CurrentValue = time;
            _context.SaveChanges();
        }

        private static int DateDiffDaysPlusOne(DateTime start, DateTime end)
        {
            return (int)end.Subtract(start).TotalDays + 1;
        }

        private static void EditASecretIdentity()
        {
            var samurai = _context.Samurais.Include(s => s.SecretIdentity)
                                  .FirstOrDefault(s => s.Id == 1);
            samurai.SecretIdentity.RealName = "T'Challa";
            _context.SaveChanges();
        }

        private static void EnlistSamuraiBattle()
        {
            var battle = _context.Battles.Find(1);
            battle.SamuraiBattles.Add(new SamuraiBattle { SamuraiId = 3 });
            _context.SaveChanges();
        }

        private static void EnlistSamuraiBattleNoTracked()
        {
            Battle battle;
            using (var separetedOperation = new SamuraiContext())
            {
                battle = separetedOperation.Battles.Find(1);
            }
            var newSamurai = new Samurai { Name = "RobertSan" };
            battle.SamuraiBattles.Add(new SamuraiBattle { Samurai = newSamurai });
            _context.Battles.Attach(battle);
            _context.SaveChanges();
        }

        private static void GetAllSamurai()
        {
            var samurais = _context.Samurais.ToList();
        }

        private static void GetSamuraiWithBattles()
        {
            var samuraiWithBattles = _context.Samurais
               .Include(s => s.SamuraiBattles)
               .ThenInclude(sb => sb.Battle).FirstOrDefault(s => s.Id == 1);

            var battle = samuraiWithBattles.SamuraiBattles.First().Battle;
            var allTheBattles = new List<Battle>();
            foreach (var samuraiBattle in samuraiWithBattles.SamuraiBattles)
            {
                allTheBattles.Add(samuraiBattle.Battle);
            }
        }

        private static void JoinBattlesAndSamurai()
        {
            var sbJoin = new SamuraiBattle { SamuraiId = 1, BattleId = 3 };
            _context.Add(sbJoin);
            _context.SaveChanges();
        }

        private static void Main(string[] args)
        {
            //PrePopulateSamuraisAndBattles();
            //JoinBattlesAndSamurai();
            //EnlistSamuraiBattle();
            //EnlistSamuraiBattleNoTracked();
            //GetSamuraiWithBattles();
            //RemoveBattleSamuraiSimple();
            //RemoveBattleSamuraiNoTracked();
            //RemoveBattleSamurai();
            //AddSamuraiWithSectertIdentity();
            //AddSecretIdentitBySamuraiId();
            //AddSecretIdentitBySamuraiIdUntracked();
            //ReplaceASecretIdentity();
            //ReplaceSecretIdentityNotInMemory();
            //CreteaSamurai();
            //CreateThenEditSamuraiWithQuote();
            //SamuraiCretedLastWeek();
            //AddSamuraiWithBetterName();
            //UpdateBetterName();
            //ReplaceBetterName();
            //GetAllSamurai();
            //RetrieveScalarResult();
            //SortWithoutReturningScalar();
            RetrieveBattleDays();
            RetrieveBattleDaysWithoutDbFunction();
            RetrieveBattleYears();
        }

        private static void PrePopulateSamuraisAndBattles()
        {
            _context.AddRange(
             new Samurai { Name = "Kikuchiyo" },
             new Samurai { Name = "Kambei Shimada" },
             new Samurai { Name = "Shichirōji " },
             new Samurai { Name = "Katsushirō Okamoto" },
             new Samurai { Name = "Heihachi Hayashida" },
             new Samurai { Name = "Kyūzō" },
             new Samurai { Name = "Gorōbei Katayama" }
           );

            _context.Battles.AddRange(
             new Battle { Name = "Battle of Okehazama", StartDate = new DateTime(1560, 05, 01), EndDate = new DateTime(1560, 06, 15) },
             new Battle { Name = "Battle of Shiroyama", StartDate = new DateTime(1877, 9, 24), EndDate = new DateTime(1877, 9, 24) },
             new Battle { Name = "Siege of Osaka", StartDate = new DateTime(1614, 1, 1), EndDate = new DateTime(1615, 12, 31) },
             new Battle { Name = "Boshin War", StartDate = new DateTime(1868, 1, 1), EndDate = new DateTime(1869, 1, 1) }
           );
            _context.SaveChanges();
        }

        private static void RemoveBattleSamurai()
        {
            var samurai = _context.Samurais.Include(s => s.SamuraiBattles).ThenInclude(sb => sb.Battle).SingleOrDefault(s => s.Id == 3);
            var sbToRemove = samurai.SamuraiBattles.SingleOrDefault(sb => sb.BattleId == 1);
            samurai.SamuraiBattles.Remove(sbToRemove);
            _context.SaveChanges();
        }

        // this doesnt work atm
        private static void RemoveBattleSamuraiNoTracked()
        {
            Samurai samurai;
            using (var diffContext = new SamuraiContext())
            {
                samurai = diffContext.Samurais.Include(s => s.SamuraiBattles).ThenInclude(sb => sb.Battle).SingleOrDefault(s => s.Id == 3);
            }
            var sbToRemove = samurai.SamuraiBattles.SingleOrDefault(sb => sb.BattleId == 1);
            samurai.SamuraiBattles.Remove(sbToRemove);
            _context.SaveChanges();
        }

        private static void RemoveBattleSamuraiSimple()
        {
            var join = new SamuraiBattle { BattleId = 1, SamuraiId = 8 };
            _context.Remove(join);
            _context.SaveChanges();
        }

        private static void ReplaceASecretIdentity()
        {
            var samurai = _context.Samurais.Include(s => s.SecretIdentity)
                                  .FirstOrDefault(s => s.Id == 1);
            samurai.SecretIdentity = new SecretIdentity { RealName = "Sampson" };
            _context.SaveChanges();
        }

        private static void ReplaceASecretIdentityNotTracked()
        {
            Samurai samurai;
            using (var separateOperation = new SamuraiContext())
            {
                samurai = separateOperation.Samurais.Include(s => s.SecretIdentity)
                                           .FirstOrDefault(s => s.Id == 1);
            }
            samurai.SecretIdentity = new SecretIdentity { RealName = "Sampson" };
            _context.Samurais.Attach(samurai);
            //this will fail...EF Core tries to insert a duplicate samuraiID FK
            _context.SaveChanges();
        }

        private static void ReplaceBetterName()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.BetterName = new PersonFullName("Grom", "Hellscream");
            _context.SaveChanges();
        }

        private static void ReplaceSecretIdentityNotInMemory()
        {
            var samurai = _context.Samurais.FirstOrDefault(s => s.SecretIdentity != null);
            samurai.SecretIdentity = new SecretIdentity { RealName = "Bobbie Draper" };
            //error
            _context.SaveChanges();
        }

        private static void RetrieveBattleDays()
        {
            var battles = _context.Battles.Select(b => new { b.Name, Days = SamuraiContext.DaysInBattle(b.StartDate, b.EndDate) }).ToList();
        }

        private static void RetrieveBattleDaysWithoutDbFunction()
        {
            var battles = _context.Battles.Select(
                b => new
                {
                    b.Name,
                    Days = DateDiffDaysPlusOne(b.StartDate, b.EndDate)
                }
                ).ToList();
        }

        private static void RetrieveBattleYears()
        {
            var battles = _context.Battles.Select(b => new { b.Name, startYear = b.StartDate.Year }).ToList();
        }

        private static void RetrieveScalarResult()
        {
            var samurai = _context.Samurais.Where(s => EF.Functions.Like(SamuraiContext.EarliestBattleFoughtBySamurai(s.Id), "%Battle%")).Select(s => new { s.Name, ErliestBattle = SamuraiContext.EarliestBattleFoughtBySamurai(s.Id) }).ToList();
        }

        private static void SamuraiCretedLastWeek()
        {
            var oneWeekAgo = DateTime.Now.AddDays(-7);

            var samurai = _context.Samurais.Where(s => EF.Property<DateTime>(s, "Created") >= oneWeekAgo).Select(s => new { s.Id, s.Name, Created = EF.Property<DateTime>(s, "Created") }).ToList();
        }

        private static void SortWithoutReturningScalar()
        {
            var samurais = _context.Samurais
                 .OrderBy(s => SamuraiContext.EarliestBattleFoughtBySamurai(s.Id))
                 .ToList();
        }

        private static void UpdateBetterName()
        {
            // to nie zadziała w przypadku zmiany na value object
            //var samurai = _context.Samurais.FirstOrDefault(f => f.BetterName.SurName == "K");
            //samurai.BetterName.SurName = "Kru";
            //_context.SaveChanges();
        }
    }
}