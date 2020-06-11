using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;
using OrthographyMobile.Models.dict;

namespace OrthographyMobile.Services
{
	public class RulesDataStore : IDataStore<Rule>
	{
		HttpClient client;
		IEnumerable<Rule> items;
		string route => Rule.Route;

		public RulesDataStore()
		{
			client = new HttpClient();
			client.BaseAddress = new Uri($"{App.AzureBackendUrl}/");

			items = new List<Rule>();
		}

		bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;
		public async Task<IEnumerable<Rule>> GetItemsAsync(bool forceRefresh = false)
		{
			if (forceRefresh && IsConnected)
			{
				var json = await client.GetStringAsync(route).ConfigureAwait(false);
				items = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Rule>>(json));
			}

			return items;
		}

		public async Task<Rule> GetItemAsync(string id)
		{
			if (id != null && IsConnected)
			{
				var json = await client.GetStringAsync($"{route}/{id}").ConfigureAwait(false);
				return await Task.Run(() => JsonConvert.DeserializeObject<Rule>(json));
			}

			return null;
		}

		public async Task<bool> AddItemAsync(Rule item)
		{
			if (item == null || !IsConnected)
				return false;

			var serializedRule = JsonConvert.SerializeObject(item);

			var response = await client.PostAsync(route, new StringContent(serializedRule, Encoding.UTF8, "application/json"));

			return response.IsSuccessStatusCode;
		}

		public async Task<bool> UpdateItemAsync(Rule item)
		{
			if (item == null || item.ID <= 0 || !IsConnected)
				return false;

			var serializedRule = JsonConvert.SerializeObject(item);
			var buffer = Encoding.UTF8.GetBytes(serializedRule);
			var byteContent = new ByteArrayContent(buffer);

			var response = await client.PutAsync(new Uri($"{route}/{item.ID}"), byteContent);

			return response.IsSuccessStatusCode;
		}

		public async Task<bool> DeleteItemAsync(string id)
		{
			if (string.IsNullOrEmpty(id) && !IsConnected)
				return false;

			var response = await client.DeleteAsync($"{route}/{id}");

			return response.IsSuccessStatusCode;
		}
	}
}
