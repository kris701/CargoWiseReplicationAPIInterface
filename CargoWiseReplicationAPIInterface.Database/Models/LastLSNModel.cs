namespace CargoWiseReplicationAPIInterface.Database.Models
{
	/// <summary>
	/// A simple input/output model to contain LSN data for each replication table
	/// </summary>
	public class LastLSNModel
	{
		/// <summary>
		/// The last LSN value
		/// </summary>
		public string LastLSN { get; set; }
		/// <summary>
		/// The Name of the replication table (without schema)
		/// </summary>
		public string TableCode { get; set; }
		/// <summary>
		/// A marker of when it was updated last
		/// </summary>
		public DateTime UpdatedAt { get; set; }
	}
}
