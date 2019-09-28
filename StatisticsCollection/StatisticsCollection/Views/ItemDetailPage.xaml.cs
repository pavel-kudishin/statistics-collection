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
		private readonly ItemDetailViewModel _viewModel;

		public ItemDetailPage(ItemDetailViewModel viewModel)
		{
			InitializeComponent();

			BindingContext = _viewModel = viewModel;
		}

		public ItemDetailPage()
		{
			InitializeComponent();

			Item item = new Item
			{
				Text = "Item 1",
				Date = DateTime.Now
			};

			_viewModel = new ItemDetailViewModel(item);
			BindingContext = _viewModel;
		}
	}
}