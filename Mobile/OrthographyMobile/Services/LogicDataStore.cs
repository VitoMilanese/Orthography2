using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OrthographyMobile.Models;
using OrthographyMobile.Models.dict;
using OrthographyMobile.Models.enums;
using Xamarin.Essentials;

namespace OrthographyMobile.Services
{
	public class LogicDataStore
	{
		private HttpClient client;
		private bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;
		public string Route => "logic/";

		public LogicDataStore()
		{
			client = new HttpClient();
			client.BaseAddress = new Uri($"{App.AzureBackendUrl}/");
		}

		public async Task<List<Mode>> GetWorkingModes()
		{
			if (IsConnected)
			{
				var json = await client.GetStringAsync($"{Route}GetWorkingModes").ConfigureAwait(false);
				return await Task.Run(() => JsonConvert.DeserializeObject<List<Mode>>(json));
			}

			return null;
		}

		public async Task<Rule> GetRuleByDetails(int modeId, int numberId, int personId, int genderId)
		{
			if (IsConnected)
			{
				var json = await client.GetStringAsync($"{Route}GetRuleByDetails?modeId={modeId}&numberId={numberId}&personId={personId}&genderId={genderId}").ConfigureAwait(false);
				return await Task.Run(() => JsonConvert.DeserializeObject<Rule>(json));
			}

			return null;
		}

		public async Task<GeneratedPackage> GetRandomRelationDetailed(int exclId, int fixMode)
		{
			if (IsConnected)
			{
				var json = await client.GetStringAsync($"{Route}GetRandomRelationDetailed?exclId={exclId}&fixMode={fixMode}").ConfigureAwait(false);
				return await Task.Run(() => JsonConvert.DeserializeObject<GeneratedPackage>(json));
			}

			return null;
		}

		public async Task<GeneratedPackage> GetRelationByRuleAndWord(int ruleId, int wordId)
		{
			if (IsConnected)
			{
				var json = await client.GetStringAsync($"{Route}GetRelationByRuleAndWord?ruleId={ruleId}&wordId={wordId}").ConfigureAwait(false);
				return await Task.Run(() => JsonConvert.DeserializeObject<GeneratedPackage>(json));
			}

			return null;
		}

		public async Task<List<int>> GetAvailableGenders(int modeId, int numberId, int personId)
		{
			if (IsConnected)
			{
				var json = await client.GetStringAsync($"{Route}GetAvailableGenders?modeId={modeId}&numberId={numberId}&personId={personId}").ConfigureAwait(false);
				return await Task.Run(() => JsonConvert.DeserializeObject<List<int>>(json));
			}

			return null;
		}

		public async Task<List<int>> GetAvailablePersons(int modeId, int numberId)
		{
			if (IsConnected)
			{
				var json = await client.GetStringAsync($"{Route}GetAvailablePersons?modeId={modeId}&numberId={numberId}").ConfigureAwait(false);
				return await Task.Run(() => JsonConvert.DeserializeObject<List<int>>(json));
			}

			return null;
		}

		public async Task<Word> GetRandomWordWithPreposition(int exclId)
		{
			if (IsConnected)
			{
				var json = await client.GetStringAsync($"{Route}GetRandomWordWithPreposition?exclId={exclId}").ConfigureAwait(false);
				return await Task.Run(() => JsonConvert.DeserializeObject<Word>(json));
			}

			return null;
		}
	}
}
