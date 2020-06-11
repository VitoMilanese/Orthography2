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
	public class GendersController : ControllerBase
	{
		private DataContext Context { get; set; }
		private readonly ILogger<OrthographyTestController> _logger;

		public GendersController(ILogger<OrthographyTestController> logger)
		{
			_logger = logger;
			Context = new DataContext(Program.ConnectionString);
		}

		// GET: api/<GenderController>
		[HttpGet]
		public IEnumerable<Gender> Get()
		{
			return Context.Genders.OrderBy(p => p.ID);
		}

		// GET api/<GenderController>/5
		[HttpGet("{id}")]
		public Gender Get(int id)
		{
			return Context.Genders.FirstOrDefault(p => p.ID == id);
		}

		// POST api/<GenderController>
		[HttpPost]
		public void Post([FromBody] Gender value)
		{
		}

		// PUT api/<GenderController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] Gender value)
		{
		}

		// DELETE api/<GenderController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
