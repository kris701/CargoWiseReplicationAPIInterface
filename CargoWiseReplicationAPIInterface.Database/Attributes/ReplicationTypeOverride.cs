namespace CargoWiseReplicationAPIInterface.Database.Attributes
{
	public class ReplicationTypeOverride : Attribute
	{
		public string SQLDataType { get; set; }

		public ReplicationTypeOverride(string sQLDataType)
		{
			SQLDataType = sQLDataType;
		}
	}
}
