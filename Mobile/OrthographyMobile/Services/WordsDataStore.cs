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
	public class WordsDataStore : IDataStore<Word>
	{
		HttpClient client;
		IEnumerable<Word> items;
		string route => Word.Route;

		public WordsDataStore()
		{
			client = new HttpClient();
			client.BaseAddress = new Uri($"{App.AzureBackendUrl}/");

			items = new List<Word>();
		}

		bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;
		public async Task<IEnumerable<Word>> GetItemsAsync(bool forceRefresh = false)
		{
			if (forceRefresh && IsConnected)
			{
				var json = await client.GetStringAsync(route).ConfigureAwait(false);
				items = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Word>>(json));
			}

			return items;
		}

		public async Task<Word> GetItemAsync(string id)
		{
			if (id != null && IsConnected)
			{
				var json = await client.GetStringAsync($"{route}/{id}").ConfigureAwait(false);
				return await Task.Run(() => JsonConvert.DeserializeObject<Word>(json));
			}

			return null;
		}

		public async Task<bool> AddItemAsync(Word item)
		{
			if (item == null || !IsConnected)
				return false;

			var serializedWord = JsonConvert.SerializeObject(item);

			var response = await client.PostAsync(route, new StringContent(serializedWord, Encoding.UTF8, "application/json"));

			return response.IsSuccessStatusCode;
		}

		public async Task<bool> UpdateItemAsync(Word item)
		{
			if (item == null || item.ID <= 0 || !IsConnected)
				return false;

			var serializedWord = JsonConvert.SerializeObject(item);
			var buffer = Encoding.UTF8.GetBytes(serializedWord);
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
