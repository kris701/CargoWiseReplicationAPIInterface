using System;
using System.Collections.Generic;
using System.Text;

namespace CargoWiseReplicationAPIInterface.Database.Attributes
{
	public class ReplicationCreateValueOverride : Attribute
	{
		public string Value { get; set; }

		public ReplicationCreateValueOverride(string value)
		{
			Value = value;
		}
	}
}
