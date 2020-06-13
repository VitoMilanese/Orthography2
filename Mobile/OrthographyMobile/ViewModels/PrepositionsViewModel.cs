using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using OrthographyMobile.Models.dict;

namespace OrthographyMobile.ViewModels
{
	public class PrepositionsViewModel : BaseViewModel
	{
		public const int DispatcherAwakeTime = 200;
		public const int CacheSize = 30;

		private bool isCacheThreadRunning;
		private Task cacheThread;
		private object m_cacheLock = new object();
		private Stack<Word> Cache { get; set; }

		public bool IsGenerating { get; private set; }

		private Word selected;
		public Word Selected
		{
			get => selected;
			private set => SetProperty(ref selected, value);
		}

		private bool busyIndicator = true;
		public bool BusyIndicator
		{
			get => busyIndicator;
			set => SetProperty(ref busyIndicator, value);
		}

		public PrepositionsViewModel()
		{
			Cache = new Stack<Word>();
			Selected = new Word();
			GenerateWord();
		}

		/// <summary>
		/// Must be called before accessing the data and recalled when is needed to update data
		/// </summary>
		/// <returns></returns>
		public void LoadData()
		{
			if (IsBusy) return;
			IsBusy = true;
			try
			{
				GenerateWord();
			}
			catch (Exception ex)
			{
				Debugger.Log(0, $"Debug_{GetType()}", ex.Message);
			}
			finally
			{
				IsBusy = false;
			}
		}

		public void GenerateWord()
		{
			if (IsGenerating) return;
			if (IsBusy) return; else IsBusy = true;
			IsGenerating = true;
			Task.Run(() =>
			{
				try
				{
					BusyIndicator = true;
					Task.Delay(DispatcherAwakeTime).Wait();

					Word word = null;

					if (Cache.Count > 0)
						lock (m_cacheLock)
							word = Cache.Pop();
					else
						try
						{
							var exclId = Selected?.ID ?? 0;
							word = Logic.GetRandomWordWithPreposition(exclId).Result;
						}
						catch (Exception ex)
						{
							Debugger.Log(0, $"Debug_{GetType()}", ex.Message);
						}

					if (word != null)
						Selected = word;
					else
					{
						// TODO: Manage missing connection
					}
				}
				catch (Exception ex)
				{
					Debugger.Log(0, $"Debug_{GetType()}", ex.Message);
				}
				finally
				{
					BusyIndicator = false;
					IsBusy = false;
					IsGenerating = false;
				}
			});
		}

		public void RunRefillCacheThread()
		{
			if (cacheThread != null)
			{
				isCacheThreadRunning = false;
				cacheThread?.Wait();
			}
			isCacheThreadRunning = true;
			cacheThread = new Task(() => RefillCacheThread());
			cacheThread.ConfigureAwait(false);
			cacheThread.Start();
		}

		public void StopRefillCacheThread()
		{
			isCacheThreadRunning = false;
		}

		private Task RefillCacheThread()
		{
			while (isCacheThreadRunning)
			{
				if (Selected == null)
				{
					Task.Delay(500).Wait();
					continue;
				}

				if (Cache.Count >= CacheSize)
				{
					Task.Delay(1000).Wait();
					continue;
				}

				try
				{
					var word = Logic.GetRandomWordWithPreposition(Selected.ID).Result;
					if (word != null)
						lock (m_cacheLock)
							Cache.Push(word);
				}
				catch (Exception ex)
				{
					Debugger.Log(0, $"Debug_{GetType()}", ex.Message);
				}

				if (isCacheThreadRunning)
					Task.Delay(1000).Wait();
			}
			return Task.CompletedTask;
		}
	}
}
