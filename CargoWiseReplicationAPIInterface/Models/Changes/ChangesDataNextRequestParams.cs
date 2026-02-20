using System.Text.Json.Serialization;

namespace CargoWiseReplicationAPIInterface.Models.Changes
{
	public class ChangesDataNextRequestParams
	{
		[JsonPropertyName("after_lsn")]
		public string AfterLSN { get; set; }
		[JsonPropertyName("after_seqval")]
		public string AfterSeqVal { get; set; }
		[JsonPropertyName("after_command_id")]
		public int AfterCommandId { get; set; }
		[JsonPropertyName("after_operation")]
		public int AfterOperation { get; set; }
	}
}
