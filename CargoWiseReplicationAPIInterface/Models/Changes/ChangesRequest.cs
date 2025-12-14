using System.Text.Json.Serialization;

namespace CargoWiseReplicationAPIInterface.Models.Changes
{
	public class ChangesRequest
	{
		[JsonPropertyName("after_lsn")]
		public string AfterLSN { get; set; }
		[JsonPropertyName("max_lsn")]
		public string MaxLSN { get; set; }
		[JsonPropertyName("after_seqval")]
		public string AfterSeqVal { get; set; }
		[JsonPropertyName("after_command_id")]
		public int AfterCommandId { get; set; }
		[JsonPropertyName("after_operation")]
		public int AfterOperation { get; set; }
		[JsonPropertyName("schema_name")]
		public string SchemaName { get; set; }
		[JsonPropertyName("table_name")]
		public string TableName { get; set; }
		[JsonPropertyName("page_size")]
		public int PageSize { get; set; }
	}
}
