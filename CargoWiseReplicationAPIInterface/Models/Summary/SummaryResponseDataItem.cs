using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CargoWiseReplicationAPIInterface.Models.Summary
{
	public class SummaryResponseDataItem
	{
		[JsonPropertyName("schemaName")]
		public string SchemaName { get; set; }
		[JsonPropertyName("tableName")]
		public string TableName { get; set; }
	}
}
