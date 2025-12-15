using CargoWiseReplicationAPIInterface.Models;

namespace CargoWiseReplicationAPIInterface.Tests.Models
{
	public class AccTransactionHeader : BaseReturnData
	{
		public RepData<Guid?> AH_PK { get; set; } = new RepData<Guid?>();
		public RepData<string?> AH_TransactionNum { get; set; } = new RepData<string?>();
		public RepData<string?> AH_Desc { get; set; } = new RepData<string?>();
		public RepData<string?> AH_Ledger { get; set; } = new RepData<string?>();
		public RepData<string?> AH_TransactionType { get; set; } = new RepData<string?>();
		public RepData<string?> AH_TransactionCategory { get; set; } = new RepData<string?>();
		public RepData<string?> AH_InvoiceTerm { get; set; } = new RepData<string?>();
		public RepData<int?> AH_InvoiceTermDays { get; set; } = new RepData<int?>();
		public RepData<string?> AH_ReceiptType { get; set; } = new RepData<string?>();
		public RepData<double?> AH_LocalTotal { get; set; } = new RepData<double?>();
		public RepData<double?> AH_LocalTaxAmountOtherTaxes { get; set; } = new RepData<double?>();
		public RepData<double?> AH_InvoiceAmount { get; set; } = new RepData<double?>();
		public RepData<double?> AH_GSTAmount { get; set; } = new RepData<double?>();
		public RepData<double?> AH_OSTotal { get; set; } = new RepData<double?>();
		public RepData<double?> AH_OSTaxAmountOtherTaxes { get; set; } = new RepData<double?>();
		public RepData<double?> AH_OutstandingAmount { get; set; } = new RepData<double?>();
		public RepData<double?> AH_ExchangeRate { get; set; } = new RepData<double?>();
		public RepData<string?> AH_RX_NKTransactionCurrency { get; set; } = new RepData<string?>();
		public RepData<DateTime?> AH_PostDate { get; set; } = new RepData<DateTime?>();
		public RepData<DateTime?> AH_InvoiceDate { get; set; } = new RepData<DateTime?>();
		public RepData<DateTime?> AH_DueDate { get; set; } = new RepData<DateTime?>();
		public RepData<DateTime?> AH_FullyPaidDate { get; set; } = new RepData<DateTime?>();
		public RepData<Guid?> AH_JH { get; set; } = new RepData<Guid?>();
		public RepData<Guid?> AH_OH { get; set; } = new RepData<Guid?>();
		public RepData<string?> AH_GC { get; set; } = new RepData<string?>();
		public RepData<DateTime?> AH_SystemCreateTimeUtc { get; set; } = new RepData<DateTime?>();
		public RepData<DateTime?> AH_SystemLastEditTimeUtc { get; set; } = new RepData<DateTime?>();
		public RepData<string?> AH_UploadType { get; set; } = new RepData<string?>("REP");
	}
}
