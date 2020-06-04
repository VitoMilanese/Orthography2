using DataLayer;

namespace Orthography.Shared
{
	public class Db : DataContext
	{
		private const string ConnectionString = @"Data Source=192.168.1.3\SQLSERVER;Initial Catalog=Orthography;Persist Security Info=True;User ID=sa;Password=developer";
		//private const string ConnectionString = @"Data Source=vhanychc.w12.hoster.by;Initial Catalog=vhanychc_Orthography;Persist Security Info=True;User ID=vhanychc_developer;Password=D3v310p3r";
		public static Db Context;

		static Db()
		{
			Context = new Db();
		}

		private Db() : base(ConnectionString)
		{
		}

		public static void Save()
		{
			Context.SaveChanges();
		}
	}
}
