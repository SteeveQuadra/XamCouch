using Couchbase.Lite;
using Couchbase.Lite.Query;
using Couchbase.Lite.Sync;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace XamNetCore
{
	public partial class MainPage : ContentPage
	{
		Database _database;

		Replicator _replicator;
		ObservableCollection<Airline> _listLog = new ObservableCollection<Airline>();
		ObservableCollection<Airline> _listAirlines = new ObservableCollection<Airline>();

		public string Status { get; set; }
		public ObservableCollection<Airline> ListLog { get => _listLog; set => _listLog = value; }
		public ObservableCollection<Airline> ListAirlines { get => _listAirlines; set => _listAirlines = value; }
		public Airline NewAirline { get => _newAirline; set => _newAirline = value; }

		private Airline _newAirline = new Airline();

		public MainPage()
		{
			InitializeComponent();
			BindingContext = this;
		}

		public void StartBase()
		{
			Airline a = new Airline
			{
				Country = "Start database"
			};
			ListLog.Add(a);
			try
			{
				_database = new CouchbaseHelper().GiveBase();
			}
			catch(Exception e)
			{
				a = new Airline
				{
					Country = "Exception GiveBase"
				};
				ListLog.Add(a);
				a = new Airline
				{
					Country = e.Message
				};
				ListLog.Add(a);
				throw new Exception($"Connexion error in StartBase, {e.Message}", e);
			}
			if (_database != null)
			{
				_replicator = new CouchbaseHelper().StartReplicator(_database);
				if (_replicator != null)
					_replicator.StatusChanged += _replicator_StatusChanged;
			}
		}

		private void _replicator_StatusChanged(object sender, ReplicationStatusChangedEventArgs e)
		{
			//Airline a = new Airline
			//{
			//	Country = e.Status.Activity.ToString()
			//};
			//ListAirline.Add(a);
		}

		public void CloseDataBase()
		{
			_replicator.Stop();
			_database.Dispose();
		}

		private void btnUpdate_Clicked(object sender, EventArgs e)
		{
			Airline a = new Airline
			{
				Country = _replicator?.Status.Activity.ToString()
			};
			ListLog.Add(a);
		}

		private void BtnGet_Clicked(object sender, EventArgs e)
		{
			LoadAirlines();
		}

		private void LoadAirlines()
		{
			ListAirlines.Clear();
			if (_database == null)
				return;
			var rows = new CouchbaseHelper().Travel(_database);
			foreach (IResult doc in rows)
			{

				var o = doc.GetDictionary(0);

				Airline a = new Airline
				{
					Country = o.GetString("country"),
					Name = o.GetString("name"),
					Id = o.GetInt("id")
				};
				ListAirlines.Add(a);
			}
		}

		private void BtnAdd_Clicked(object sender, EventArgs e)
		{
			new CouchbaseHelper().AddAirlineDocument(NewAirline, _database);

			LoadAirlines();
		}
	}
}
