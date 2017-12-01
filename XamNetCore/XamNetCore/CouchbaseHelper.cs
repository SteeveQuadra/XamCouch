using Couchbase.Lite;
using Couchbase.Lite.Query;
using Couchbase.Lite.Sync;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XamNetCore
{
    public class CouchbaseHelper
    {
        Database DatabaseSd { get; set; }

        public CouchbaseHelper()
        {
        }

        public IResultSet Travel(Database db)
        {
			if (db == null)
				return null;
            var query = Query.Select(SelectResult.All())
                .From(DataSource.Database(db))
                .Where(Expression.Property("type").EqualTo("airline"))
                .OrderBy(Ordering.Property("id"));

            var rows = query.Run();
            
            return rows;
        }

        public Database GiveBase()
        {
			try
			{
				string databaseName = "travel-sample";

				DatabaseConfiguration config = new DatabaseConfiguration();
				config.ConflictResolver = new ExampleConflictResolver();
				return new Database(databaseName, config);
			}
			catch (Exception e)
			{
				throw new Exception($"Connection Error in GiveBase, {e.Message}", e);
			}
        }

        public Replicator StartReplicator(Database db)
        {
			try
			{
				Uri target = new Uri("blip://192.168.1.10:4984/travel-sample");
				ReplicatorConfiguration replicationConfig = new ReplicatorConfiguration(db, target);
				replicationConfig.Continuous = true;
				replicationConfig.ReplicatorType = ReplicatorType.PushAndPull;
				replicationConfig.Authenticator = new BasicAuthenticator("Gateway", "production");
				var replication = new Replicator(replicationConfig);
				replication.Start();

				return replication;
			}
			catch(Exception e)
			{
				throw new Exception($"Connection starting replication in StartReplicator, {e.Message}", e);
			}
        }

        public void AddAirlineDocument(Airline airline, Database db)
        {
            string docId = $"airline_{airline.Id}";
			//MutableDocument document = new MutableDocument(docId); //db20
			Document document = new Document(docId); // db19
            airline.Type = "airline";

            string json = JsonConvert.SerializeObject(airline);
            document.Set("callsign", airline.Callsign);
            document.Set("country", airline.Country);
            document.Set("iata", airline.Iata);
            document.Set("icao", airline.Icao);
            document.Set("id", airline.Id);
			document.Set("name", airline.Name);
			document.Set("type", airline.Type);

			db.Save(document);

            Document doc = db.GetDocument(docId);
        }
    }
}
