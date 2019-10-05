using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Util;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Environment = System.Environment;
using Platform = Xamarin.Essentials.Platform;

namespace StatisticsCollection.Droid
{
	[Activity(Label = "StatisticsCollection", Icon = "@mipmap/icon",
		Theme = "@style/MainTheme",
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(savedInstanceState);

			AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
			TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;


			Forms.SetFlags("Shell_Experimental", "Visual_Experimental",
				"CollectionView_Experimental", "FastRenderers_Experimental");
			Platform.Init(this, savedInstanceState);
			Forms.Init(this, savedInstanceState);

			DisplayCrashReport();

			LoadApplication(new App());
		}

		protected override void OnDestroy()
		{
			try
			{
				base.OnDestroy();
			}
			catch (Exception e)
			{
			}
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
			[GeneratedEnum] Permission[] grantResults)
		{
			Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}

		private static void TaskSchedulerOnUnobservedTaskException(object sender,
			UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
		{
			Exception newExc = new Exception("TaskSchedulerOnUnobservedTaskException",
				unobservedTaskExceptionEventArgs.Exception);
			LogUnhandledException(newExc);
		}

		private static void CurrentDomainOnUnhandledException(object sender,
			UnhandledExceptionEventArgs unhandledExceptionEventArgs)
		{
			Exception newExc = new Exception("CurrentDomainOnUnhandledException",
				unhandledExceptionEventArgs.ExceptionObject as Exception);
			LogUnhandledException(newExc);
		}

		internal static void LogUnhandledException(Exception exception)
		{
			try
			{
				const string ERROR_FILE_NAME = "Fatal.log";
				string libraryPath =
					Environment.GetFolderPath(Environment.SpecialFolder
						.Personal); // iOS: Environment.SpecialFolder.Resources
				string errorFilePath = Path.Combine(libraryPath, ERROR_FILE_NAME);
				string errorMessage = string.Format("Time: {0}\r\nError: Unhandled Exception\r\n{1}",
					DateTime.Now, exception);
				File.WriteAllText(errorFilePath, errorMessage);

				// Log to Android Device Logging.
				Log.Error("Crash Report", errorMessage);
			}
			catch
			{
				// just suppress any error logging exceptions
			}
		}

		/// <summary>
		// If there is an unhandled exception, the exception information is diplayed
		// on screen the next time the app is started (only in debug configuration)
		/// </summary>
		[Conditional("DEBUG")]
		private void DisplayCrashReport()
		{
			const string ERROR_FILENAME = "Fatal.log";
			string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string errorFilePath = Path.Combine(libraryPath, ERROR_FILENAME);

			if (!File.Exists(errorFilePath))
			{
				return;
			}

			string errorText = File.ReadAllText(errorFilePath);
			new AlertDialog.Builder(this)
				.SetPositiveButton("Clear", (sender, args) => { File.Delete(errorFilePath); })
				.SetNegativeButton("Close", (sender, args) =>
				{
					// User pressed Close.
				})
				.SetMessage(errorText)
				.SetTitle("Crash Report")
				.Show();
		}
	}
}