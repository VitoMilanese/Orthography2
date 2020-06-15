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
	[Route("dict/[controller]")]
	public class RulesController : ControllerBase
	{
		private DataContext Context { get; set; }
		private readonly ILogger<OrthographyTestController> _logger;

		public RulesController(ILogger<OrthographyTestController> logger)
		{
			_logger = logger;
			Context = new DataContext(Program.ConnectionString);
		}

		// GET: api/<RulesController>
		[HttpGet]
		public IEnumerable<Rule> Get()
		{
			return Context.Rules.OrderBy(p => p.ID);
		}

		// GET api/<RulesController>/5
		[HttpGet("{id}")]
		public Rule Get(int id)
		{
			return Context.Rules.FirstOrDefault(p => p.ID == id);
		}

		// POST api/<RulesController>
		[HttpPost]
		public void Post([FromBody] Rule value)
		{
		}

		// PUT api/<RulesController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] Rule value)
		{
		}

		// DELETE api/<RulesController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
