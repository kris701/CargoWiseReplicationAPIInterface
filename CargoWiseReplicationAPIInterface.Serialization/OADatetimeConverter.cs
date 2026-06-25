using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace CargoWiseReplicationAPIInterface.Serialization
{
	/// <summary>
	/// A type converter to convert the OA datetime format to a <seealso cref="DateTime"/>
	/// </summary>
	public class OADatetimeConverter : DefaultTypeConverter
	{
		/// <summary>
		/// Convert from OA format to a <seealso cref="DateTime"/>
		/// </summary>
		/// <param name="text"></param>
		/// <param name="row"></param>
		/// <param name="memberMapData"></param>
		/// <returns></returns>
		public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
		{
			if (text == null || text == "")
				return null;
			var value = double.Parse(text);
			return DateTime.FromOADate(value);
		}
	}
}
