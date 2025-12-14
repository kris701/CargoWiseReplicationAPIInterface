using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CargoWiseReplicationAPIInterface.Models.Summary
{
	public class SummaryRequest
	{
		[JsonPropertyName("after_lsn")]
		public string AfterLSN { get; set; }
	}
}
