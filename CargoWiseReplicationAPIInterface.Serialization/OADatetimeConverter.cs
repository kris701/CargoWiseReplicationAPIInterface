using CsvHelper;

namespace CargoWiseReplicationAPIInterface.Serialization
{
	/// <summary>
	/// A type converter to convert the OA datetime format to a <seealso cref="DateTime"/>
	/// </summary>
	public class OADatetimeConverter : ICsvConverter<DateTime?>
	{
		public DateTime? Read(CsvReader reader, int index)
		{
			var text = reader.GetString(index);
			if (text == null || text == "")
				return null;
			var value = double.Parse(text);
			return DateTime.FromOADate(value);
		}

		public void Write(CsvWriter writer, DateTime? value) => throw new NotImplementedException();
	}
}
