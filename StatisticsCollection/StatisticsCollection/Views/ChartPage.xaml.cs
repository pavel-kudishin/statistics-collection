using Microcharts;
using StatisticsCollection.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StatisticsCollection.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChartPage : ContentPage
	{
		private readonly ChartViewModel _viewModel;

		public ChartPage()
		{
			InitializeComponent();

			_viewModel = new ChartViewModel();
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			if (_viewModel.Entries == null)
				await _viewModel.LoadAsync();

			Chart chart = new LineChart() { Entries = _viewModel.Entries };
			this.ChartView.Chart = chart;
		}
	}
}