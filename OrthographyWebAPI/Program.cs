using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace OAPI
{
	public class Program
	{
		public const string ConnectionString = @"Data Source=192.168.1.111\SQLSERVER;Initial Catalog=Orthography;Persist Security Info=True;User ID=sa;Password=developer";
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}