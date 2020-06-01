using DataLayer;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace DataLayerTest
{
	public class UnitTest1
	{
		private const string ConnectionString = @"Data Source=192.168.1.3\SQLSERVER;Initial Catalog=Orthography;Persist Security Info=True;User ID=sa;Password=developer";

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
			optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
			var context = new DataContext(optionsBuilder.Options);
			var persons = context.Persons.ToList();
			Assert.True(persons.Count > -1, persons.Count.ToString());
		}
	}
}
