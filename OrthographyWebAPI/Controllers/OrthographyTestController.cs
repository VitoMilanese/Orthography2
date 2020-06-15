using System.Collections.Generic;
using System.Linq;
using DataLayer;
using DataLayer.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OAPI.Controllers
{
	[EnableCors("AllowAnyOriginPolicy")]
	[ApiController]
	[Route("[controller]")]
	public class OrthographyTestController : ControllerBase
	{
		private DataContext Context { get; set; }
		private readonly ILogger<OrthographyTestController> _logger;

		public OrthographyTestController(ILogger<OrthographyTestController> logger)
		{
			_logger = logger;
			Context = new DataContext(Program.ConnectionString);
		}

		[HttpGet]
		public IEnumerable<Word> Get()
		{
			return Context.Words.Take(5);
		}
	}
}
