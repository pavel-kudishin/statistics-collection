using StatisticsCollection.Models;
using StatisticsCollection.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
				foreach (Item item in items)
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