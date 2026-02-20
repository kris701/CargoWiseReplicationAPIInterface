using CargoWiseReplicationAPIInterface.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CargoWiseReplicationAPIInterface.Tests.Models
{
	public class GlbGroup : BaseReturnData
	{
		public Guid? GG_PK { get; set; }
		public string? GG_Desc { get; set; }
		public Guid? GG_ActiveDirectoryObjectGuid { get; set; }
		public bool? GG_IsSales { get; set; }
		public DateTime? GG_SystemCreateTimeUtc { get; set; }
		public string? GG_SystemCreateUser { get; set; }
		public DateTime? GG_SystemLastEditTimeUtc { get; set; }
		public string? GG_SystemLastEditUser { get; set; }
		public bool? GG_IsSystemDefined { get; set; }
		public bool? GG_IsActive { get; set; }
		public bool? GG_IsValid { get; set; }
		public string? GG_Code { get; set; }
		public bool? GG_IsSecurityEnabled { get; set; }
		public Guid? GG_GC { get; set; }
		public string? GG_DomainName { get; set; }
		public string? GG_Category { get; set; }
		public Guid? GG_GG_ParentGroup { get; set; }
		public string? GG_ExternalId { get; set; }
		public string? GG_Type { get; set; }
		public string? GG_SystemCreateBranch { get; set; }
		public string? GG_SystemCreateDepartment { get; set; }
		public string? GG_UploadType { get; set; } = "REP";
	}
}
