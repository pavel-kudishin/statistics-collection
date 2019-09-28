using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using StatisticsCollection.Services;
using StatisticsCollection.Views;

namespace StatisticsCollection
{
	public partial class App : Application
	{

		public App()
		{
			InitializeComponent();

			DependencyService.Register<DataStore>();
			MainPage = new AppShell();
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
