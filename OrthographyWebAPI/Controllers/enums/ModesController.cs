using System.Collections.Generic;
using System.Linq;
using DataLayer;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OAPI.Controllers.dict
{
	[ApiController]
	[Route("enums/[controller]")]
	public class ModesController : ControllerBase
	{
		private DataContext Context { get; set; }
		private readonly ILogger<OrthographyTestController> _logger;

		public ModesController(ILogger<OrthographyTestController> logger)
		{
			_logger = logger;
			Context = new DataContext(Program.ConnectionString);
		}

		// GET: api/<ModesController>
		[HttpGet]
		public IEnumerable<Mode> Get()
		{
			return Context.Modes.OrderBy(p => p.ID);
		}

		// GET api/<ModesController>/5
		[HttpGet("{id}")]
		public Mode Get(int id)
		{
			return Context.Modes.FirstOrDefault(p => p.ID == id);
		}

		// POST api/<ModesController>
		[HttpPost]
		public void Post([FromBody] Mode value)
		{
		}

		// PUT api/<ModesController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] Mode value)
		{
		}

		// DELETE api/<ModesController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
