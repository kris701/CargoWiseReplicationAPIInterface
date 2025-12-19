using CargoWiseReplicationAPIInterface.Exceptions;
using CargoWiseReplicationAPIInterface.Models;
using CargoWiseReplicationAPIInterface.Models.Changes;
using CargoWiseReplicationAPIInterface.Models.Summary;
using CargoWiseReplicationAPIInterface.Services;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace CargoWiseReplicationAPIInterface
{
	/// <summary>
	/// Class containing methods to interact with Cargo Wises Replication API
	/// </summary>
	public class ReplicationAPI
	{
		private readonly IReplicationAPIService _api;
		private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
		{
			AllowTrailingCommas = true
		};

		/// <summary>
		/// Create a new instance based on replication URL, username and password
		/// </summary>
		/// <param name="uRL">This should be in a format like "https://(url)/Services/api/replication/"</param>
		/// <param name="username"></param>
		/// <param name="password"></param>
		public ReplicationAPI(string uRL, string username, string password)
		{
			_api = new ReplicationAPIHTTPService(uRL, username, password);
		}

		/// <summary>
		/// Create a new instance with some other API fetching implementation
		/// </summary>
		/// <param name="api"></param>
		public ReplicationAPI(IReplicationAPIService api)
		{
			_api = api;
		}

		/// <summary>
		/// Get a summary of all the changes that have occured since a given <paramref name="afterLsn"/> LSN
		/// </summary>
		/// <param name="afterLsn"></param>
		/// <returns></returns>
		public async Task<SummaryResponseData> GetSummary(string afterLsn)
		{
			var response = await _api.GetSummary(afterLsn);
			return response.Data;
		}

		/// <summary>
		/// Get all the details for a given table within a LSN window.
		/// This will fetch and convert all the data available inside these, no matter the page size.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="afterLsn"></param>
		/// <param name="maxLsn"></param>
		/// <param name="schemaName"></param>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public async Task<List<T>> GetDetails<T>(string afterLsn, string maxLsn, string schemaName, string tableName) where T : BaseReturnData => await GetDetails(afterLsn, maxLsn, schemaName, tableName, typeof(T));
		/// <summary>
		/// Get all the details for a given table within a LSN window.
		/// This will fetch and convert all the data available inside these, no matter the page size.
		/// </summary>
		/// <param name="afterLsn"></param>
		/// <param name="maxLsn"></param>
		/// <param name="schemaName"></param>
		/// <param name="tableName"></param>
		/// <param name="asType"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public async Task<dynamic> GetDetails(string afterLsn, string maxLsn, string schemaName, string tableName, Type asType)
		{
			if (!asType.IsSubclassOf(typeof(BaseReturnData)))
				throw new Exception($"Invalid type! Must be based on {nameof(BaseReturnData)}!");

			var returnList = CreateList(asType);

			var currentChanges = await _api.GetChanges(afterLsn, maxLsn, schemaName, tableName);

			var typeDict = new Dictionary<string, string>();
			var props = asType.GetProperties().ToList();
			var propSet = BuildPropertySet(props);
			props.RemoveAll(x => x.Name != "OpCode");
			var operationProp = props[0];

			while (currentChanges != null)
			{
				ExpandDictionary(typeDict, currentChanges, propSet);

				ConvertAndInsertChanges(currentChanges.Data.Data.Items, typeDict, asType, returnList, operationProp);

				currentChanges = await _api.GetChangesFromLast(currentChanges, maxLsn, schemaName, tableName);
			}

			return returnList;
		}

		private void ExpandDictionary(Dictionary<string, string> dict, ChangesResponse response, HashSet<string> propSet)
		{
			foreach (var item in response.Data.Data.Items)
				foreach (var col in item.Columns)
					if (propSet.Contains(col.Name) && !dict.ContainsKey(col.Name))
						dict.Add(col.Name, col.Type.ToUpper());
		}

		private HashSet<string> BuildPropertySet(List<PropertyInfo> props)
		{
			var propSet = new HashSet<string>();
			foreach (var prop in props)
				propSet.Add(prop.Name);
			return propSet;
		}

		private void ConvertAndInsertChanges(List<ChangesDataDataItems> changes, Dictionary<string, string> typeDict, Type asType, IList returnList, PropertyInfo operationProp)
		{
			var ordered = changes.OrderBy(x => double.Parse(x.Version));
			foreach (var change in ordered)
			{
				foreach (var chamges in change.Changes)
				{
					var asJson = MergeDataToJson(chamges.Data, typeDict);
					try
					{
						var newItem = JsonSerializer.Deserialize(asJson, asType, _options);
						if (newItem != null)
						{
							operationProp.SetValue(newItem, chamges.Operation);
							returnList.Add(newItem);
						}
					}
					catch(Exception ex) {
						throw new ParsingException(ex.Message, asJson, ex);
					}
				}
			}
		}

		private string MergeDataToJson(List<ChangesDataDataItemsChangesData> data, Dictionary<string, string> columnMap)
		{
			var sb = new StringBuilder();

			sb.Append("{");
			foreach (var change in data)
				if (columnMap.ContainsKey(change.ColumnName))
					sb.Append($"\"{change.ColumnName}\":{ConvertValueToJson(change.Value, columnMap[change.ColumnName])},");
			sb.Append("}");

			return sb.ToString();
		}

		private string ConvertValueToJson(object value, string columnType)
		{
			if (value == null)
				return "null";
			if (columnType.StartsWith("DECIMAL("))
				return $"{value}";
			switch (columnType)
			{
				case "INT":
				case "SMALLINT":
				case "TINYINT":
				case "MONEY":
				case "DECIMAL":
					return $"{value}";
				case "BIT":
					var strValue = value.ToString();
					if (strValue == null)
						return "false";
					if (strValue.ToUpper() == "TRUE")
						return "true";
					return "false";
				case "DATETIME":
				case "DATETIME2":
				case "SMALLDATETIME":
					var strValue2 = value.ToString();
					if (strValue2 == null)
						return "null";
					var parsed = DateTime.ParseExact(strValue2, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
					return $"\"{parsed.ToString("O")}\"";
				default:
					var strValue3 = value.ToString();
					if (strValue3 == null)
						return "null";
					strValue3 = strValue3.Replace("\"", "'");
					strValue3 = strValue3.Replace("\r", "\\r");
					strValue3 = strValue3.Replace("\n", "\\n");
					strValue3 = strValue3.Replace("\\", "\\\\");
					strValue3 = strValue3.Replace("/", "//");
					return $"\"{strValue3}\"";
			}
		}

		private IList CreateList(Type myType)
		{
			Type genericListType = typeof(List<>).MakeGenericType(myType);
			return (IList)Activator.CreateInstance(genericListType);
		}
	}
}
