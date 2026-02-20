using System.Text.Json.Serialization;

namespace CargoWiseReplicationAPIInterface.Models.Changes
{
	public class ChangesDataItemsChangesData
	{
		[JsonPropertyName("columnName")]
		public string ColumnName { get; set; }
		[JsonPropertyName("value")]
		public object Value { get; set; }
	}
}
