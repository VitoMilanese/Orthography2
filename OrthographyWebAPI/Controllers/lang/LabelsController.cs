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
	public class LabelsController : ControllerBase
	{
		private DataContext Context { get; set; }
		private readonly ILogger<OrthographyTestController> _logger;

		public LabelsController(ILogger<OrthographyTestController> logger)
		{
			_logger = logger;
			Context = new DataContext(Program.ConnectionString);
		}

		// GET: api/<LabelController>
		[HttpGet]
		public IEnumerable<Label> Get()
		{
			return Context.Labels.OrderBy(p => p.ID);
		}

		// GET api/<LabelController>/5
		[HttpGet("{id}")]
		public Label Get(int id)
		{
			return Context.Labels.FirstOrDefault(p => p.ID == id);
		}

		// POST api/<LabelController>
		[HttpPost]
		public void Post([FromBody] Label value)
		{
		}

		// PUT api/<LabelController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] Label value)
		{
		}

		// DELETE api/<LabelController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
