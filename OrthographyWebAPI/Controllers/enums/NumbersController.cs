using System.Collections.Generic;
using System.Linq;
using DataLayer;
using DataLayer.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OAPI.Controllers.dict
{
	[EnableCors("AllowAnyOriginPolicy")]
	[ApiController]
	[Route("enums/[controller]")]
	public class NumbersController : ControllerBase
	{
		private DataContext Context { get; set; }
		private readonly ILogger<OrthographyTestController> _logger;

		public NumbersController(ILogger<OrthographyTestController> logger)
		{
			_logger = logger;
			Context = new DataContext(Program.ConnectionString);
		}

		// GET: api/<NumbersController>
		[HttpGet]
		public IEnumerable<Number> Get()
		{
			return Context.Numbers.OrderBy(p => p.ID);
		}

		// GET api/<NumbersController>/5
		[HttpGet("{id}")]
		public Number Get(int id)
		{
			return Context.Numbers.FirstOrDefault(p => p.ID == id);
		}

		// POST api/<NumbersController>
		[HttpPost]
		public void Post([FromBody] Number value)
		{
		}

		// PUT api/<NumbersController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] Number value)
		{
		}

		// DELETE api/<NumbersController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
