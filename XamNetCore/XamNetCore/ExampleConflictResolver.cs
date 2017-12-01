using Couchbase.Lite;
using System;
using System.Collections.Generic;
using System.Text;

namespace XamNetCore
{
    public class ExampleConflictResolver : IConflictResolver
    {
        public ExampleConflictResolver()
        {

        }
		
		//public Document Resolve(Conflict conflict) // db20
		public ReadOnlyDocument Resolve(Conflict conflict) // db19
		{
			var baseProperties = conflict.Base;
			var mine = conflict.Mine;
			var theirs = conflict.Theirs;

			return theirs;
		}
	}
}
