using System.Collections.Generic;
using System.Linq;
using DataLayer;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OAPI.Controllers.dict
{
	[ApiController]
	[Route("dict/[controller]")]
	public class RelationsController : ControllerBase
	{
		private DataContext Context { get; set; }
		private readonly ILogger<OrthographyTestController> _logger;

		public RelationsController(ILogger<OrthographyTestController> logger)
		{
			_logger = logger;
			Context = new DataContext(Program.ConnectionString);
		}

		// GET: api/<RelationsController>
		[HttpGet]
		public IEnumerable<Relation> Get()
		{
			return Context.Relations.OrderBy(p => p.ID);
		}

		// GET api/<RelationsController>/5
		[HttpGet("{id}")]
		public Relation Get(int id)
		{
			return Context.Relations.FirstOrDefault(p => p.ID == id);
		}

		// POST api/<RelationsController>
		[HttpPost]
		public void Post([FromBody] Relation value)
		{
		}

		// PUT api/<RelationsController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] Relation value)
		{
		}

		// DELETE api/<RelationsController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
