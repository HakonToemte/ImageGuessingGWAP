using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Xunit.Abstractions;
using ImageGuessingGame;
using ImageGuessingGame.GameContext;
using System.Threading.Tasks;

namespace ImageGuessingGame.Test.Helpers
{
	public class DbTest
	{
		//private readonly ShopContext _context;
		private readonly DbContextOptions<GuessingGameContext> _contextOptions;
		private readonly DbConnection _connection;
		private readonly ITestOutputHelper _output;

		public DbTest(ITestOutputHelper output)

		{
			_output = output;

			_contextOptions = new DbContextOptionsBuilder<GuessingGameContext>()
					.UseSqlite(CreateInMemoryDatabase())
					.LogTo(_output.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, Microsoft.Extensions.Logging.LogLevel.Information)
					.EnableSensitiveDataLogging()
					.Options;
			_connection = RelationalOptionsExtension.Extract(_contextOptions).Connection;
			//_context = new ShopContext(_contextOptions);
		}

		public DbContextOptions<GuessingGameContext> ContextOptions => _contextOptions;

		//public ShopContext Context => _context;

		private static DbConnection CreateInMemoryDatabase()
		{
			var connection = new SqliteConnection("Filename=:memory:");

			connection.Open();

			return connection;
		}


		public void Dispose() => _connection.Dispose();
	}
}