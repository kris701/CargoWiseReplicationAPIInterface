using CargoWiseReplicationAPIInterface.Models;

namespace CargoWiseReplicationAPIInterface.Tests.Models
{
	public class GlbCompanyModel : BaseReturnData
	{
		public Guid? GC_PK { get; set; }
		public string? GC_Code { get; set; }
		public string? GC_Name { get; set; }
		public string? GC_BusinessRegNo { get; set; }
		public string? GC_BusinessRegNo2 { get; set; }
		public string? GC_CustomsRegistrationNo { get; set; }
		public string? GC_Address1 { get; set; }
		public string? GC_Address2 { get; set; }
		public string? GC_City { get; set; }
		public string? GC_Phone { get; set; }
		public string? GC_PostCode { get; set; }
		public string? GC_State { get; set; }
		public string? GC_Fax { get; set; }
		public string? GC_WebAddress { get; set; }
		public int? GC_NoOfAccountingPeriods { get; set; }
		public DateTime? GC_StartDate { get; set; }
		public Guid? GC_OH_OrgProxy { get; set; }
		public string? GC_RX_NKLocalCurrency { get; set; }
		public string? GC_RN_NKCountryCode { get; set; }
		public string? GC_LocalDocLanguage { get; set; }
		public bool? GC_IsActive { get; set; }
		public bool? GC_IsReciprocal { get; set; }
		public bool? GC_IsGSTRegistered { get; set; }
		public bool? GC_IsGSTCashBasis { get; set; }
		public bool? GC_IsWHTRegistered { get; set; }
		public bool? GC_IsWHTCashBasis { get; set; }
		public bool? GC_IsValid { get; set; }
		public string? GC_Email { get; set; }
		public string? GC_AddressMap { get; set; }
		public string? GC_ValidationStatus { get; set; }
		public DateTime? GC_SystemCreateTimeUtc { get; set; }
		public string? GC_SystemCreateUser { get; set; }
		public DateTime? GC_SystemLastEditTimeUtc { get; set; }
		public string? GC_SystemLastEditUser { get; set; }
	}
}
