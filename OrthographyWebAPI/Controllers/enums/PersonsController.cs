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
	public class PersonsController : ControllerBase
	{
		private DataContext Context { get; set; }
		private readonly ILogger<OrthographyTestController> _logger;

		public PersonsController(ILogger<OrthographyTestController> logger)
		{
			_logger = logger;
			Context = new DataContext(Program.ConnectionString);
		}

		// GET: api/<PersonController>
		[HttpGet]
		public IEnumerable<Person> Get()
		{
			return Context.Persons.OrderBy(p => p.ID);
		}

		// GET api/<PersonController>/5
		[HttpGet("{id}")]
		public Person Get(int id)
		{
			return Context.Persons.FirstOrDefault(p => p.ID == id);
		}

		// POST api/<PersonController>
		[HttpPost]
		public void Post([FromBody] Person value)
		{
		}

		// PUT api/<PersonController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] Person value)
		{
		}

		// DELETE api/<PersonController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
