using Binance.Trading.Bot.Test.Entities;
using Microsoft.EntityFrameworkCore;

namespace Binance.Trading.Bot.Test.DataAccess
{
    public class AppDbContext : DbContext
    {
        //public DbSet<SymbolEntity> Symbols { get; set; }
        public DbSet<CandleEntity> Candles { get; set; }
        public DbSet<TradeSignal> TradeSignals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<SymbolEntity>(x =>
            //x.ToTable("Symbols")
            // .HasKey(m => m.SymbolId)
            //);
            modelBuilder.Entity<CandleEntity>(
                x => x.ToTable("Candles")
                .HasKey(m => m.CandleId)
            );
            modelBuilder.Entity<TradeSignal>(
                x => x.ToTable("TradeSignals")
                .HasKey(m => m.Id)
            );
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=AHMET;Database=BinanceTradingBot; User Id=sa;Password=1;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
