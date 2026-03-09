namespace CargoWiseReplicationAPIInterface.Database.Attributes
{
	public class ReplicationUpdateValueOverride : Attribute
	{
		public string Value { get; set; }

		public ReplicationUpdateValueOverride(string value)
		{
			Value = value;
		}
	}
}
