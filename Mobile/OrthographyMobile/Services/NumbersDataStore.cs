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
	public class NumbersDataStore : IDataStore<Number>
	{
		HttpClient client;
		IEnumerable<Number> items;
		string route => Number.Route;

		public NumbersDataStore()
		{
			client = new HttpClient();
			client.BaseAddress = new Uri($"{App.AzureBackendUrl}/");

			items = new List<Number>();
		}

		bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;
		public async Task<IEnumerable<Number>> GetItemsAsync(bool forceRefresh = false)
		{
			if (forceRefresh && IsConnected)
			{
				var json = await client.GetStringAsync(route).ConfigureAwait(false);
				items = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Number>>(json));
			}

			return items;
		}

		public async Task<Number> GetItemAsync(string id)
		{
			if (id != null && IsConnected)
			{
				var json = await client.GetStringAsync($"{route}/{id}").ConfigureAwait(false);
				return await Task.Run(() => JsonConvert.DeserializeObject<Number>(json));
			}

			return null;
		}

		public async Task<bool> AddItemAsync(Number item)
		{
			if (item == null || !IsConnected)
				return false;

			var serializedNumber = JsonConvert.SerializeObject(item);

			var response = await client.PostAsync(route, new StringContent(serializedNumber, Encoding.UTF8, "application/json"));

			return response.IsSuccessStatusCode;
		}

		public async Task<bool> UpdateItemAsync(Number item)
		{
			if (item == null || item.ID <= 0 || !IsConnected)
				return false;

			var serializedNumber = JsonConvert.SerializeObject(item);
			var buffer = Encoding.UTF8.GetBytes(serializedNumber);
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
