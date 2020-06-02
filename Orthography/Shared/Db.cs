using DataLayer;

namespace Orthography.Shared
{
	public class Db : DataContext
	{
		private const string ConnectionString = @"Data Source=192.168.1.3\SQLSERVER;Initial Catalog=Orthography;Persist Security Info=True;User ID=sa;Password=developer";
		public static Db Context;

		static Db()
		{
			Context = new Db();
		}

		private Db() : base(ConnectionString)
		{
		}
	}
}
