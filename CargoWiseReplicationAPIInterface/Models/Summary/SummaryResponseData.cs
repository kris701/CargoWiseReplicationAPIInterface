using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CargoWiseReplicationAPIInterface.Models.Summary
{
	public class SummaryResponseData
	{
		[JsonPropertyName("maxLsn")]
		public string MaxLSN { get; set; }
		[JsonPropertyName("items")]
		public List<SummaryResponseDataItem> Items { get; set; }
	}
}
