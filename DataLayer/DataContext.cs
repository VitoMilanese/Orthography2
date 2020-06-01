using DataLayer.Models;
using DataLayer.Models.enums;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
	public class DataContext : DbContext
	{
		private string m_connectionString { get; }
		public DataContext(string connectionString)
		{
			m_connectionString = connectionString;
		}

		public DataContext(DbContextOptions options) : base(options)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(m_connectionString);
			optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
		}

		public DbSet<Mode> Modes { get; set; }
		public DbSet<Person> Persons { get; set; }
		public DbSet<Number> Numbers { get; set; }
		public DbSet<Gender> Genders { get; set; }
		public DbSet<Word> Words { get; set; }
		public DbSet<Rule> Rules { get; set; }
		public DbSet<Relation> Relations { get; set; }
	}
}
