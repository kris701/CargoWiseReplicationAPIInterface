using CargoWiseReplicationAPIInterface.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CargoWiseReplicationAPIInterface.Tests.Models
{
	public class GenCustomAddOnValue : BaseReturnData
	{
		public Guid? XV_PK { get; set; }
		public string? XV_Type { get; set; }
		public string? XV_Data { get; set; }
		public bool? XV_IsRuleEnabled { get; set; }
		public Guid? XV_XR_Rule { get; set; }
		public string? XV_ParentTableCode { get; set; }
		public Guid? XV_ParentID { get; set; }
		public int? XV_AutoVersion { get; set; }
		public DateTime? XV_SystemCreateTimeUtc { get; set; }
		public string? XV_SystemCreateUser { get; set; }
		public DateTime? XV_SystemLastEditTimeUtc { get; set; }
		public string? XV_SystemLastEditUser { get; set; }
		public string? XV_Name { get; set; }
		public double? XV_DataAsDecimal { get; set; }
		public string? XV_UploadType { get; set; } = "REP";
	}
}
