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
	[Route("lang/[controller]")]
	public class TermsController : ControllerBase
	{
		private DataContext Context { get; set; }
		private readonly ILogger<OrthographyTestController> _logger;

		public TermsController(ILogger<OrthographyTestController> logger)
		{
			_logger = logger;
			Context = new DataContext(Program.ConnectionString);
		}

		// GET: api/<TermController>
		[HttpGet]
		public IEnumerable<Term> Get()
		{
			return Context.Terms.OrderBy(p => p.ID);
		}

		// GET api/<TermController>/5
		[HttpGet("{id}")]
		public Term Get(int id)
		{
			return Context.Terms.FirstOrDefault(p => p.ID == id);
		}

		// POST api/<TermController>
		[HttpPost]
		public void Post([FromBody] Term value)
		{
		}

		// PUT api/<TermController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] Term value)
		{
		}

		// DELETE api/<TermController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
