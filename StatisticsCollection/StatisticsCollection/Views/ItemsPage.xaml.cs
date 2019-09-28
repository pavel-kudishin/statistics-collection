﻿using StatisticsCollection.Models;
using StatisticsCollection.ViewModels;
using System;
using System.ComponentModel;
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
	}
}