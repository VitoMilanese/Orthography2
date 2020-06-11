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
	public class ModesDataStore : IDataStore<Mode>
	{
		HttpClient client;
		IEnumerable<Mode> items;
		string route => Mode.Route;

		public ModesDataStore()
		{
			client = new HttpClient();
			client.BaseAddress = new Uri($"{App.AzureBackendUrl}/");

			items = new List<Mode>();
		}

		bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;
		public async Task<IEnumerable<Mode>> GetItemsAsync(bool forceRefresh = false)
		{
			if (forceRefresh && IsConnected)
			{
				var json = await client.GetStringAsync(route).ConfigureAwait(false);
				items = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Mode>>(json));
			}

			return items;
		}

		public async Task<Mode> GetItemAsync(string id)
		{
			if (id != null && IsConnected)
			{
				var json = await client.GetStringAsync($"{route}/{id}").ConfigureAwait(false);
				return await Task.Run(() => JsonConvert.DeserializeObject<Mode>(json));
			}

			return null;
		}

		public async Task<bool> AddItemAsync(Mode item)
		{
			if (item == null || !IsConnected)
				return false;

			var serializedMode = JsonConvert.SerializeObject(item);

			var response = await client.PostAsync(route, new StringContent(serializedMode, Encoding.UTF8, "application/json"));

			return response.IsSuccessStatusCode;
		}

		public async Task<bool> UpdateItemAsync(Mode item)
		{
			if (item == null || item.ID <= 0 || !IsConnected)
				return false;

			var serializedMode = JsonConvert.SerializeObject(item);
			var buffer = Encoding.UTF8.GetBytes(serializedMode);
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
