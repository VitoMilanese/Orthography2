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
	public class LanguagesController : ControllerBase
	{
		private DataContext Context { get; set; }
		private readonly ILogger<OrthographyTestController> _logger;

		public LanguagesController(ILogger<OrthographyTestController> logger)
		{
			_logger = logger;
			Context = new DataContext(Program.ConnectionString);
		}

		// GET: api/<LanguagesController>
		[HttpGet]
		public IEnumerable<Language> Get()
		{
			return Context.Languages.OrderBy(p => p.ID);
		}

		// GET api/<LanguagesController>/5
		[HttpGet("{id}")]
		public Language Get(int id)
		{
			return Context.Languages.FirstOrDefault(p => p.ID == id);
		}

		// POST api/<LanguagesController>
		[HttpPost]
		public void Post([FromBody] Language value)
		{
		}

		// PUT api/<LanguagesController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] Language value)
		{
		}

		// DELETE api/<LanguagesController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
