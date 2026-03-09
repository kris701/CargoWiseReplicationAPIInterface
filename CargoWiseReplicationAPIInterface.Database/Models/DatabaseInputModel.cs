namespace CargoWiseReplicationAPIInterface.Database.Models
{
	/// <summary>
	/// Container model to bulk insert replication data
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class DatabaseInputModel<T>
	{
		/// <summary>
		/// List of items
		/// </summary>
		public List<T> Items { get; set; }

		/// <summary>
		/// Main constructor
		/// </summary>
		/// <param name="items"></param>
		public DatabaseInputModel(List<T> items)
		{
			Items = items;
		}
	}
}
