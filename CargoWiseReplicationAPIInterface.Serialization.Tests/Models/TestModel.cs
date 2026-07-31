using CargoWiseReplicationAPIInterface.Database.Attributes;
using CargoWiseReplicationAPIInterface.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CargoWiseReplicationAPIInterface.Serialization.Tests.Models
{
	public class TestModel : BaseReturnData
	{
		[ReplicationPrimaryKey]
		public Guid ID { get; set; }
		public string? Value { get; set; }
		public DateTime? Value2 { get; set; }
	}
}
