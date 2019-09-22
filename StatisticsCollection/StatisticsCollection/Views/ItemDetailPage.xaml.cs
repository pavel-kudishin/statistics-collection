using StatisticsCollection.Models;
using StatisticsCollection.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace StatisticsCollection.Views
{
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible(false)]
	public partial class ItemDetailPage : ContentPage
	{
		ItemDetailViewModel viewModel;

		public ItemDetailPage(ItemDetailViewModel viewModel)
		{
			InitializeComponent();

			BindingContext = this.viewModel = viewModel;
		}

		public ItemDetailPage()
		{
			InitializeComponent();

			Item item = new Item
			{
				Text = "Item 1",
				Date = DateTime.Now
			};

			viewModel = new ItemDetailViewModel(item);
			BindingContext = viewModel;
		}
	}
}