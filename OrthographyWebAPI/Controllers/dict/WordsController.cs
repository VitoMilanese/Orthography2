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
	public class WordsController : ControllerBase
	{
		private DataContext Context { get; set; }
		private readonly ILogger<OrthographyTestController> _logger;

		public WordsController(ILogger<OrthographyTestController> logger)
		{
			_logger = logger;
			Context = new DataContext(Program.ConnectionString);
		}

		// GET: api/<WordsController>
		[HttpGet]
		public IEnumerable<Word> Get()
		{
			return Context.Words.OrderBy(p => p.ID);
		}

		// GET api/<WordsController>/5
		[HttpGet("{id}")]
		public Word Get(int id)
		{
			return Context.Words.FirstOrDefault(p => p.ID == id);
		}

		// POST api/<WordsController>
		[HttpPost]
		public void Post([FromBody] Word value)
		{
		}

		// PUT api/<WordsController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] Word value)
		{
		}

		// DELETE api/<WordsController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
