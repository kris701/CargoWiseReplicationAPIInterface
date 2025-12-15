using CargoWiseReplicationAPIInterface.Models;

namespace CargoWiseReplicationAPIInterface.Tests.Models
{
	public class GlbCompanyModel : BaseReturnData
	{
		public RepData<Guid?> GC_PK { get; set; } = new RepData<Guid?>();
		public RepData<string?> GC_Code { get; set; } = new RepData<string?>();
		public RepData<string?> GC_Name { get; set; } = new RepData<string?>();
		public RepData<string?> GC_BusinessRegNo { get; set; } = new RepData<string?>();
		public RepData<string?> GC_BusinessRegNo2 { get; set; } = new RepData<string?>();
		public RepData<string?> GC_CustomsRegistrationNo { get; set; } = new RepData<string?>();
		public RepData<string?> GC_Address1 { get; set; } = new RepData<string?>();
		public RepData<string?> GC_Address2 { get; set; } = new RepData<string?>();
		public RepData<string?> GC_City { get; set; } = new RepData<string?>();
		public RepData<string?> GC_Phone { get; set; } = new RepData<string?>();
		public RepData<string?> GC_PostCode { get; set; } = new RepData<string?>();
		public RepData<string?> GC_State { get; set; } = new RepData<string?>();
		public RepData<string?> GC_Fax { get; set; } = new RepData<string?>();
		public RepData<string?> GC_WebAddress { get; set; } = new RepData<string?>();
		public RepData<int?> GC_NoOfAccountingPeriods { get; set; } = new RepData<int?>();
		public RepData<DateTime?> GC_StartDate { get; set; } = new RepData<DateTime?>();
		public RepData<Guid?> GC_OH_OrgProxy { get; set; } = new RepData<Guid?>();
		public RepData<string?> GC_RX_NKLocalCurrency { get; set; } = new RepData<string?>();
		public RepData<string?> GC_RN_NKCountryCode { get; set; } = new RepData<string?>();
		public RepData<string?> GC_LocalDocLanguage { get; set; } = new RepData<string?>();
		public RepData<bool?> GC_IsActive { get; set; } = new RepData<bool?>();
		public RepData<bool?> GC_IsReciprocal { get; set; } = new RepData<bool?>();
		public RepData<bool?> GC_IsGSTRegistered { get; set; } = new RepData<bool?>();
		public RepData<bool?> GC_IsGSTCashBasis { get; set; } = new RepData<bool?>();
		public RepData<bool?> GC_IsWHTRegistered { get; set; } = new RepData<bool?>();
		public RepData<bool?> GC_IsWHTCashBasis { get; set; } = new RepData<bool?>();
		public RepData<bool?> GC_IsValid { get; set; } = new RepData<bool?>();
		public RepData<string?> GC_Email { get; set; } = new RepData<string?>();
		public RepData<string?> GC_AddressMap { get; set; } = new RepData<string?>();
		public RepData<string?> GC_ValidationStatus { get; set; } = new RepData<string?>();
		public RepData<DateTime?> GC_SystemCreateTimeUtc { get; set; } = new RepData<DateTime?>();
		public RepData<string?> GC_SystemCreateUser { get; set; } = new RepData<string?>();
		public RepData<DateTime?> GC_SystemLastEditTimeUtc { get; set; } = new RepData<DateTime?>();
		public RepData<string?> GC_SystemLastEditUser { get; set; } = new RepData<string?>();
	}
}
