using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using OrthographyMobile.ViewModels.Helpers;
using OrthographyMobile.Models;
using System.Collections.Generic;

namespace OrthographyMobile.ViewModels
{
	public class ConjunctionsViewModel : BaseViewModel
	{
		public const int ShowAnswerTime = 1500;
		public const int ShowResultTime = 750;
		public const int DispatcherAwakeTime = 200;
		public const int CacheSize = 30;

		private bool isCacheThreadRunning;
		private Task cacheThread;
		private object m_cacheLock = new object();
		private Stack<GeneratedPackage> Cache { get; set; }

		private IDispatcher Dispatcher { get; }

		public ConjunctionsPageBindings UI { get; private set; }

		public bool IsGenerating { get; private set; }

		private GeneratedPackageToken selected;
		public GeneratedPackageToken Selected
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

		private bool m_randomMode;
		public bool RandomMode
		{
			get => m_randomMode;
			set => SetProperty(ref m_randomMode, value);
		}

		public EventHandler OnAnswerSubmit { get; set; }

		public Command AnswerSubmitCommand { get; set; }

		public ConjunctionsViewModel(IDispatcher dispatcher)
		{
			Dispatcher = dispatcher;
			Cache = new Stack<GeneratedPackage>();
			Selected = new GeneratedPackageToken();
			if (Device.RuntimePlatform == Device.iOS)
				UI = new ConjunctionsPageBindings_iOS();
			else
				UI = new ConjunctionsPageBindings_Droid();
			UI.UpdateProperties();
			Title = "Browse";
			AnswerSubmitCommand = new Command(() => OnAnswerSubmit?.Invoke(this, EventArgs.Empty));
			GenerateWord(true);
		}

		public void GenerateWord() => GenerateWord(false);

		private void GenerateWord(bool ignoreBusy)
		{
			if (IsGenerating) return;
			IsGenerating = true;
			if (!ignoreBusy)
			{
				if (IsBusy) return;
				else IsBusy = true;
			}
			Task.Run(() =>
			{
				try
				{
					BusyIndicator = true;
					Task.Delay(DispatcherAwakeTime).Wait();

					GeneratedPackage package = null;

					if (Cache.Count > 0)
						lock (m_cacheLock)
							package = Cache.Pop();
					else
						try
						{
							var exclId = Selected.Relation != null ? Selected.Relation.ID : int.MinValue;
							var exclMode = !RandomMode && Selected.Mode != null ? Selected.Mode.ID : int.MinValue;
							package = Logic.GetRandomRelationDetailed(exclId, exclMode).Result;
						}
						catch (Exception ex)
						{
							Debugger.Log(0, $"Debug_{GetType()}", ex.Message);
						}

					if (package != null)
					{
						Dispatcher.BeginInvokeOnMainThread(() => Selected.Package = package);
						Task.Delay(DispatcherAwakeTime).Wait();
						while (Selected.IsBusy) ;
					}
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
					if (!ignoreBusy) IsBusy = false;
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
				if (Selected.Relation == null || Selected.Mode == null)
				{
					Task.Delay(500).Wait();
					continue;
				}

				if (Cache.Count >= CacheSize)
				{
					Task.Delay(1000).Wait();
					continue;
				}

				var prevR = Selected.Relation.ID;
				var prevM = !RandomMode ? Selected.Mode.ID : int.MinValue;
				try
				{
					var random = Logic.GetRandomRelationDetailed(prevR, prevM).Result;
					if (random != null)
						lock (m_cacheLock)
							Cache.Push(random);
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