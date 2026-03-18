using CargoWiseReplicationAPIInterface.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CargoWiseReplicationAPIInterface.Tests.Models
{
	public class StmNote : BaseReturnData
	{
		public Guid? ST_PK { get; set; }
		public Guid? ST_ParentID { get; set; }
		public string? ST_Table { get; set; }
		public byte[]? ST_NoteData { get; set; }
		public string? ST_NoteText { get; set; }
		public string? ST_NoteType { get; set; }
		public string? ST_NoteContext { get; set; }
		public Guid? ST_GC_RelatedCompany { get; set; }
		public bool? ST_IsCustomDescription { get; set; }
		public bool? ST_ForceRead { get; set; }
		public string? ST_Description { get; set; }
		public DateTime? ST_SystemCreateTimeUtc { get; set; }
		public string? ST_SystemCreateUser { get; set; }
		public DateTime? ST_SystemLastEditTimeUtc { get; set; }
		public string? ST_SystemLastEditUser { get; set; }
		public string? ST_UploadType { get; set; } = "REP";
	}
}
