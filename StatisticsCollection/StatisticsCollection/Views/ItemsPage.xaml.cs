using Newtonsoft.Json;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using StatisticsCollection.Models;
using StatisticsCollection.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace StatisticsCollection.Views
{
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible(false)]
	public partial class ItemsPage : ContentPage
	{
		private readonly ItemsViewModel _viewModel;

		public ItemsPage()
		{
			InitializeComponent();

			BindingContext = _viewModel = new ItemsViewModel();
		}

		async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
		{
			Item item = args.SelectedItem as Item;
			if (item == null)
				return;

			await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

			// Manually deselect item.
			ItemsListView.SelectedItem = null;
		}

		async void AddItem_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			if (_viewModel.Items.Count == 0)
				_viewModel.LoadItemsCommand.Execute(null);
		}

		public void OnEdit(object sender, EventArgs e)
		{
			MenuItem mi = ((MenuItem)sender);
			Item item = ((Item)mi.CommandParameter);
		}

		public async void OnDelete(object sender, EventArgs e)
		{
			MenuItem mi = ((MenuItem)sender);
			Item item = ((Item)mi.CommandParameter);
			bool isConfirmed = await DisplayAlert(
				"Удаление",
				$"Удалить {item.Date.ToString("g")} - {item.Value}?",
				"Да",
				"Нет");

			if (isConfirmed)
			{
				_viewModel.DeleteItemCommand.Execute(item);
			}
		}

		private async void ShareItems_Clicked(object sender, EventArgs e)
		{
			string serializedObject = JsonConvert.SerializeObject(_viewModel.Items);

			await Share.RequestAsync(new ShareTextRequest
			{
				Text = serializedObject,
				Title = "Поделиться"
			});
		}

		private async void ImportItems_Clicked(object sender, EventArgs e)
		{
			try
			{
				FileData fileData = await CrossFilePicker.Current.PickFile();
				if (fileData == null)
				{
					return; // user canceled file picking
				}

				string fileName = fileData.FileName;
				string contents = System.Text.Encoding.UTF8.GetString(fileData.DataArray);

				bool isConfirmed = await DisplayAlert(
					"Импорт",
					$"Импортировать {fileName}?",
					"Да",
					"Нет");

				if (isConfirmed)
				{
					Item[] items = JsonConvert.DeserializeObject<Item[]>(contents);
					_viewModel.ImportItemsCommand.Execute(items);
				}
			}
			catch (Exception ex)
			{
			}
		}
	}
}