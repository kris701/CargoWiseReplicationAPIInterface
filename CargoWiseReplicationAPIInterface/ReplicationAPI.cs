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
			var operationProp = props.First(x => x.Name == "OpCode");
			props = props.Where(x => x.PropertyType.Name.StartsWith("RepData")).ToList();
			var propSet = BuildPropertySet(props);
			var allProperties = new Dictionary<string, PropertyInfo>();
			foreach (var prop in props)
				allProperties.Add(prop.Name, prop);

			while (currentChanges != null)
			{
				ExpandDictionary(typeDict, currentChanges, propSet);

				ConvertAndInsertChanges(currentChanges.Data.Data.Items, typeDict, asType, returnList, allProperties, operationProp);

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

		private void ConvertAndInsertChanges(List<ChangesDataDataItems> changes, Dictionary<string, string> typeDict, Type asType, IList returnList, Dictionary<string, PropertyInfo> allProperties, PropertyInfo operationProp)
		{
			var ordered = changes.OrderBy(x => double.Parse(x.Version));
			foreach (var change in ordered)
			{
				foreach (var chamges in change.Changes)
				{
					var newItem = Activator.CreateInstance(asType);
					operationProp.SetValue(newItem, chamges.Operation);

					foreach(var target in chamges.Data)
					{
						if (allProperties.ContainsKey(target.ColumnName))
						{
							var propVal = (RepData)allProperties[target.ColumnName].GetValue(newItem);
							propVal.Value = ConvertValue(target.Value, typeDict[target.ColumnName]);
							propVal.IsSet = true;
							allProperties[target.ColumnName].SetValue(newItem, propVal);
						}
					}

					returnList.Add(newItem);
				}
			}
		}

		private object? ConvertValue(object? value, string columnType)
		{
			if (value == null)
				return null;
			var strVal = value.ToString();
			if (strVal == null)
				return null;
			if (columnType.StartsWith("DECIMAL("))
				return Convert.ChangeType(strVal, typeof(double), CultureInfo.InvariantCulture);

			switch (columnType)
			{
				case "INT":
				case "SMALLINT":
				case "TINYINT":
					return Convert.ChangeType(strVal, typeof(int), CultureInfo.InvariantCulture);
				case "MONEY":
				case "DECIMAL":
					return Convert.ChangeType(strVal, typeof(double), CultureInfo.InvariantCulture);
				case "BIT":
					if (strVal.ToUpper() == "TRUE")
						return true;
					return false;
				case "DATETIME":
				case "DATETIME2":
				case "SMALLDATETIME":
					var parsed = DateTime.ParseExact(strVal, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
					return parsed;
				default:
					strVal = strVal.Replace("\"", "'");
					strVal = strVal.Replace("\r", "\\r");
					strVal = strVal.Replace("\n", "\\n");
					strVal = strVal.Replace("\\", "\\\\");
					return strVal;
			}
		}

		private IList CreateList(Type myType)
		{
			Type genericListType = typeof(List<>).MakeGenericType(myType);
			return (IList)Activator.CreateInstance(genericListType);
		}
	}
}
