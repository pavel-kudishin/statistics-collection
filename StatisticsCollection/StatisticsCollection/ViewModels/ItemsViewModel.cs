using StatisticsCollection.Models;
using StatisticsCollection.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StatisticsCollection.ViewModels
{
	public class ItemsViewModel : BaseViewModel
	{
		public ObservableCollection<Item> Items { get; set; }
		public Command LoadItemsCommand { get; set; }
		public Command DeleteItemCommand { get; set; }

		public ItemsViewModel()
		{
			Title = "Browse";
			Items = new ObservableCollection<Item>();

			LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
			DeleteItemCommand = new Command(async item => await ExecuteDeleteItemCommand((Item)item));

			MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
			{
				await DataStore.AddItemAsync(item);
				LoadItemsCommand.Execute(null);
			});
		}

		private async Task ExecuteLoadItemsCommand()
		{
			if (IsBusy)
				return;

			IsBusy = true;

			try
			{
				Items.Clear();
				IEnumerable<Item> items = await DataStore.GetItemsAsync(true);
				List<Item> list = items.ToList();
				for (int i = list.Count - 2; i >= 0; i--)
				{
					Item item = list[i];
					item.Change = item.Value - list[i + 1].Value;
					item.DailyAverageChange = GetDailyAverageChange(i, list);
				}

				foreach (Item item in list)
				{
					Items.Add(item);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
			finally
			{
				IsBusy = false;
			}
		}

		private static decimal? GetDailyAverageChange(int i, IReadOnlyList<Item> list)
		{
			Item item = list[i];
			for (int j = i + 1; j < list.Count; j++)
			{
				double totalDays = (item.Date - list[j].Date).TotalDays;
				if (totalDays < 1)
				{
					continue;
				}

				decimal? change = item.Value - list[j].Value;
				if (change.HasValue)
				{
					return Math.Round(change.Value / (decimal) totalDays, 1);
				}

				return null;
			}

			return null;
		}

		private async Task ExecuteDeleteItemCommand(Item item)
		{
			if (IsBusy)
				return;

			IsBusy = true;

			try
			{
				bool result = await DataStore.DeleteItemAsync(item.Id);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
			finally
			{
				IsBusy = false;
			}

			await ExecuteLoadItemsCommand();
		}
	}
}