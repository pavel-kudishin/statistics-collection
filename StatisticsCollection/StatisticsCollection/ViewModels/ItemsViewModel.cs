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
			Title = "Список";
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

			try
			{
				IsBusy = true;

				Items.Clear();
				IEnumerable<Item> items = await DataStore.GetItemsAsync(true);
				List<Item> list = items.ToList();

				for (int i = list.Count - 2; i >= 0; i--)
				{
					Item item = list[i];
					item.Change = item.Value - list[i + 1].Value;
					decimal? diff = item.Value - (decimal?)InterpolateLinear(list, item.Date.AddDays(-1));
					if (diff != null)
					{
						item.DailyAverageChange = Math.Round(diff.Value, 1);
					}
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

		public static decimal? InterpolateLinear(IList<Item> list, DateTime x)
		{
			int n = list.Count;

			int index = -1;
			for (int i = 1; i < n; i++)
			{
				if (list[i].Date <= x && x <= list[i - 1].Date)
				{
					index = i;
					break;
				}
			}

			if (index == -1)
			{
				return null;
			}

			decimal x1 = GetX(list[index].Date, list[index - 1].Date, x);

			decimal? valueChange = (list[index - 1].Value - list[index].Value);
			return valueChange * x1 - valueChange + list[index - 1].Value;
		}

		private static decimal GetX(DateTime startDate, DateTime endDate, DateTime x)
		{
			return (decimal) ((x - startDate).TotalDays / (endDate - startDate).TotalDays);
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

				if (j == i + 1)
				{
					decimal? change1 = item.Value - list[i + 1].Value;
					if (change1.HasValue)
					{
						return Math.Round(change1.Value / (decimal)totalDays, 1);
					}
					return null;
				}
				else
				{
					decimal? change1 = item.Value - list[i + 1].Value;
					decimal? change = list[i + 1].Value - list[j].Value;
					if (change1.HasValue && change.HasValue)
					{
						double value = (double)change.Value
										/ (list[i + 1].Date - list[j].Date).TotalDays
										* (1 - (item.Date - list[i + 1].Date).TotalDays);
						return Math.Round(change1.Value + (decimal)value, 1);
					}

				}

				return null;
			}

			return null;
		}

		private async Task ExecuteDeleteItemCommand(Item item)
		{
			if (IsBusy)
				return;

			try
			{
				IsBusy = true;

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