using DataLayer;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace DataLayerTest
{
	public class UnitTest1
	{
		private const string ConnectionString = @"Data Source=localhost\SQLSERVER;Initial Catalog=Orthography;Persist Security Info=True;User ID=sa;Password=developer";

		[Fact]
		public void TestDataContext()
		{
			var context = new DataContext(ConnectionString);
			var persons = context.Persons.ToList();
			Assert.True(persons.Count > -1, persons.Count.ToString());
		}

		[Fact]
		public void TestDataContextWithOptions()
		{
			var optionsBuilder = new DbContextOptionsBuilder();
			optionsBuilder.UseSqlServer(ConnectionString);
			optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
			var context = new DataContext(optionsBuilder.Options);
			var persons = context.Persons.ToList();
			Assert.True(persons.Count > -1, persons.Count.ToString());
		}

		[Fact]
		public void TestWords()
		{
			var context = new DataContext(ConnectionString);
			var relations = context.Relations.ToList();
			var wordsIds = context.Relations.Select(p => p.WordID).Distinct().ToList();
			var words = context.Words.Where(p => wordsIds.Contains(p.ID)).ToList();
			Assert.NotNull(words);
		}
	}
}
