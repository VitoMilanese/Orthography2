using DataLayer;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OAPI.Controllers.dict
{
	[ApiController]
	[Route("[controller]")]
	public class LogicController : ControllerBase
	{
		private DataContext Context { get; set; }
		private readonly ILogger<OrthographyTestController> _logger;

		public LogicController(ILogger<OrthographyTestController> logger)
		{
			_logger = logger;
			Context = new DataContext(Program.ConnectionString);
		}

		// GET: api/<LogicController>
		[HttpGet]
		public bool Get()
		{
			return Context?.Database?.CanConnect() ?? false;
		}

		// GET api/<LogicController>/5
		[HttpGet("{id}")]
		public bool Get(int id)
		{
			return Get();
		}

		// POST api/<LogicController>
		[HttpPost]
		public void Post([FromBody] Mode value)
		{
		}

		// PUT api/<LogicController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] Mode value)
		{
		}

		// DELETE api/<LogicController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}

		///// <summary>
		///// Returns a random record from [dict].[Relation]
		///// </summary>
		///// <param name="exclIds">IDs to exclude</param>
		///// <param name="exclModes">Modes to exclude</param>
		///// <returns>Random record from [dict].[Relation]</returns>
		//[Route("GetRandomRelation")]
		//public Relation GetRandomRelation(string exclIds, string exclModes)
		//{
		//	try
		//	{
		//		var exI = JsonConvert.DeserializeObject<int[]>(exclIds);
		//		if (exI == null || exI.Length == 0) exI = new[] { int.MinValue };

		//		var exM = JsonConvert.DeserializeObject<int[]>(exclModes);
		//		if (exM == null || exM.Length == 0) exM = new[] { int.MinValue };

		//		var exR = new List<int> { int.MinValue };
		//		exR.AddRange(Context?.Rules?.Where(p => exM.Contains(p.ModeID))?.Select(p => p.ID) ?? null);

		//		var relations = Context?.Relations?.Where(p => !exI.Contains(p.ID) && !exR.Contains(p.RuleID));

		//		var count = relations.Count();
		//		var rnd = new Random(DateTime.Now.Millisecond);
		//		var relation = relations.Skip(rnd.Next(count)).FirstOrDefault();

		//		return relation;
		//	}
		//	catch (Exception ex)
		//	{
		//		_logger.LogError(ex.Message, new[] { exclIds, exclModes });
		//	}
		//	return null;
		//}

		//[Route("GetRandomRelationDetailed")]
		//public GeneratedPackage GetRandomRelationDetailed(string exclIds, string exclModes)
		//{
		//	var res = new GeneratedPackage();
		//	try
		//	{
		//		res.Relation = GetRandomRelation(exclIds, exclModes);
		//		res.Word = Context?.Words?.FirstOrDefault(p => res.Relation != null && p.ID == res.Relation.WordID);
		//		res.Rule = Context?.Rules?.FirstOrDefault(p => res.Relation != null && p.ID == res.Relation.RuleID);
		//		var availableRules = Context?.Rules?.Where(p => res.Rule != null && p.ModeID == res.Rule.ModeID);
		//		res.AvailableNumbers = Context?.Numbers?.Where(p => availableRules.Any(q => q.NumberID == p.ID))?.ToArray();
		//		res.AvailablePersons = Context?.Persons?.Where(p => availableRules.Any(q => q.PersonID == p.ID))?.ToArray();
		//		res.AvailableGenders = Context?.Genders?.Where(p => availableRules.Any(q => q.GenderID == p.ID))?.ToArray();
		//		return res;
		//	}
		//	catch (Exception ex)
		//	{
		//		_logger.LogError(ex.Message, new[] { exclIds, null, exclModes });
		//	}
		//	return null;
		//}

		[Route("GetRandomRelation")]
		public Relation GetRandomRelation(int exclId, int fixMode)
		{
			try
			{
				var rules = (fixMode <= 0
					? Context.Rules
					: Context.Rules.Where(p => p.ModeID == fixMode))
					.Select(p => p.ID);

				var relations = Context?.Relations?.Where(p => p.ID != exclId && (fixMode <= 0 || rules.Contains(p.RuleID)));

				var count = relations.Count();
				var rnd = new Random(DateTime.Now.Millisecond);
				var relation = relations.Skip(rnd.Next(count)).FirstOrDefault();

				return relation;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, new[] { exclId, fixMode });
			}
			return null;
		}

		[Route("GetRandomRelationDetailed")]
		public GeneratedPackage GetRandomRelationDetailed(int exclId, int fixMode)
		{
			try
			{
				var res = new GeneratedPackage();
				res.Relation = GetRandomRelation(exclId, fixMode);
				res.Word = Context?.Words?.FirstOrDefault(p => res.Relation != null && p.ID == res.Relation.WordID);
				res.Rule = Context?.Rules?.FirstOrDefault(p => res.Relation != null && p.ID == res.Relation.RuleID);
				var availableRules = Context?.Rules?.Where(p => res.Rule != null && p.ModeID == res.Rule.ModeID);
				res.AvailableNumbers = Context?.Numbers?.Where(p => availableRules.Any(q => q.NumberID == p.ID))?.ToArray();
				res.AvailablePersons = Context?.Persons?.Where(p => availableRules.Any(q => q.PersonID == p.ID))?.ToArray();
				res.AvailableGenders = Context?.Genders?.Where(p => availableRules.Any(q => q.GenderID == p.ID))?.ToArray();
				return res;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, new[] { exclId, fixMode });
			}
			return null;
		}

		[Route("GetWorkingModes")]
		public List<Mode> GetWorkingModes()
		{
			try
			{
				var rules = Context.Rules.Select(p => p.ModeID).Distinct();
				var modes = Context.Modes.Where(p => rules.Contains(p.ID));
				return modes.ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}
			return null;
		}

		[Route("GetRuleByDetails")]
		public Rule GetRuleByDetails(int modeId, int numberId, int personId, int genderId)
		{
			try
			{
				var rule = Context.Rules.FirstOrDefault(p =>
					p.ModeID == modeId &&
					p.NumberID == numberId &&
					p.PersonID == personId &&
					(genderId <= 0 || p.GenderID == genderId));
				return rule;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, new[] { modeId, numberId, personId, genderId });
			}
			return null;
		}

		[Route("GetRelationByRuleAndWord")]
		public GeneratedPackage GetRelationByRuleAndWord(int ruleId, int wordId)
		{
			try
			{
				var res = new GeneratedPackage();
				res.Relation = Context.Relations.FirstOrDefault(p => p.RuleID == ruleId && p.WordID == wordId);
				res.Rule = Context.Rules.FirstOrDefault(p => p.ID == ruleId);
				res.Word = Context.Words.FirstOrDefault(p => p.ID == wordId);
				var availableRules = Context?.Rules?.Where(p => res.Rule != null && p.ModeID == res.Rule.ModeID);
				res.AvailableNumbers = Context?.Numbers?.Where(p => availableRules.Any(q => q.NumberID == p.ID))?.ToArray();
				res.AvailablePersons = Context?.Persons?.Where(p => availableRules.Any(q => q.PersonID == p.ID))?.ToArray();
				res.AvailableGenders = Context?.Genders?.Where(p => availableRules.Any(q => q.GenderID == p.ID))?.ToArray();
				return res;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, new[] { ruleId, wordId });
			}
			return null;
		}

		[Route("GetAvailableGenders")]
		public List<int> GetAvailableGenders(int modeId, int numberId, int personId)
		{
			try
			{
				var rule = Context.Rules.Where(p =>
					p.ModeID == modeId &&
					p.NumberID == numberId &&
					p.PersonID == personId)
					.Select(p => p.GenderID)
					.Distinct()
					.ToList();
				return rule;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, new[] { modeId, numberId, personId });
			}
			return null;
		}

		[Route("GetAvailablePersons")]
		public List<int> GetAvailablePersons(int modeId, int numberId)
		{
			try
			{
				var rule = Context.Rules.Where(p =>
					p.ModeID == modeId &&
					p.NumberID == numberId)
					.Select(p => p.PersonID)
					.Distinct()
					.ToList();
				return rule;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, new[] { modeId, numberId });
			}
			return null;
		}

		[Route("GetRandomWordWithPreposition")]
		public Word GetRandomWordWithPreposition(int exclId)
		{
			try
			{
				var words = Context.Words.Where(p => p.PrepositionsMask > 0 && p.ID != exclId);
				var count = words.Count();
				var rnd = new Random(DateTime.Now.Millisecond).Next(count);
				var word = words.Skip(rnd).FirstOrDefault();
				return word;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, new[] { exclId });
			}
			return null;
		}
	}
}
