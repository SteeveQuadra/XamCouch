using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace XamNetCore
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

			MainPage = new XamNetCore.MainPage();
		}

		protected override void OnStart ()
		{
			(MainPage as XamNetCore.MainPage).StartBase();
		}

		protected override void OnSleep ()
		{
			(MainPage as XamNetCore.MainPage).CloseDataBase();
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
