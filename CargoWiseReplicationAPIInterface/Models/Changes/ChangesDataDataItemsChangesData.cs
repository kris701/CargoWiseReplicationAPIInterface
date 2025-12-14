using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CargoWiseReplicationAPIInterface.Models.Changes
{
	public class ChangesDataDataItemsChangesData
	{
		[JsonPropertyName("columnName")]
		public string ColumnName { get; set; }
		[JsonPropertyName("value")]
		public object Value { get; set; }
	}
}
