using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;
using OrthographyMobile.Models.enums;

namespace OrthographyMobile.Services
{
	public class LanguagesDataStore : IDataStore<Language>
	{
		HttpClient client;
		IEnumerable<Language> items;
		string route => Language.Route;

		public LanguagesDataStore()
		{
			client = new HttpClient();
			client.BaseAddress = new Uri($"{App.AzureBackendUrl}/");

			items = new List<Language>();
		}

		bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;
		public async Task<IEnumerable<Language>> GetItemsAsync(bool forceRefresh = false)
		{
			if (forceRefresh && IsConnected)
			{
				var json = await client.GetStringAsync(Language.Route).ConfigureAwait(false);
				items = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Language>>(json));
			}

			return items;
		}

		public async Task<Language> GetItemAsync(string id)
		{
			if (id != null && IsConnected)
			{
				var json = await client.GetStringAsync($"{route}/{id}").ConfigureAwait(false);
				return await Task.Run(() => JsonConvert.DeserializeObject<Language>(json));
			}

			return null;
		}

		public async Task<bool> AddItemAsync(Language item)
		{
			if (item == null || !IsConnected)
				return false;

			var serializedLanguage = JsonConvert.SerializeObject(item);

			var response = await client.PostAsync(route, new StringContent(serializedLanguage, Encoding.UTF8, "application/json"));

			return response.IsSuccessStatusCode;
		}

		public async Task<bool> UpdateItemAsync(Language item)
		{
			if (item == null || item.ID <= 0 || !IsConnected)
				return false;

			var serializedLanguage = JsonConvert.SerializeObject(item);
			var buffer = Encoding.UTF8.GetBytes(serializedLanguage);
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
