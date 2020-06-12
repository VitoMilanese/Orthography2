using DataLayer;

namespace Orthography.Shared
{
	public class Db : DataContext
	{
		private const string ConnectionString = @"Data Source=192.168.1.111\SQLSERVER;Initial Catalog=Orthography;Persist Security Info=True;User ID=sa;Password=developer";
		public void Save() => SaveChanges();

		public Db() : base(ConnectionString)
		{
		}
	}
}
