using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using System.Threading.Tasks;

namespace StatisticsCollection.Droid
{
	[Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
	public class SplashActivity : AppCompatActivity
	{
		private static readonly string _tag = "X:" + typeof(SplashActivity).Name;

		public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
		{
			base.OnCreate(savedInstanceState, persistentState);
			Log.Debug(_tag, "SplashActivity.OnCreate");
		}

		// Launches the startup task
		protected override void OnResume()
		{
			base.OnResume();
			Task startupWork = new Task(SimulateStartup);
			startupWork.Start();
		}

		// Simulates background work that happens behind the splash screen
		async void SimulateStartup()
		{
			Log.Debug(_tag, "Performing some startup work that takes a bit of time.");
			//await Task.Delay(8000); // Simulate a bit of startup work.
			Log.Debug(_tag, "Startup work is finished - starting MainActivity.");
			StartActivity(new Intent(Application.Context, typeof(MainActivity)));
		}

		public override void OnBackPressed()
		{

		}
	}
}