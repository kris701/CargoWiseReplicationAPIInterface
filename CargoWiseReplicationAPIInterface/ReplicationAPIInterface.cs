using CargoWiseReplicationAPIInterface.Models.Changes;
using CargoWiseReplicationAPIInterface.Models.Summary;
using SerializableHttps;
using SerializableHttps.AuthenticationMethods;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CargoWiseReplicationAPIInterface
{
    public class ReplicationAPIInterface
    {
		/// <summary>
		/// The URL to the replication endpoint.
		/// This should be in a format like "https://(url)/Services/api/replication/"
		/// </summary>
		public string URL { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public int PageSize { get; set; } = 1000;

		private SerializableHttpsClient _client;
		private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
		{
			AllowTrailingCommas = true
		};

		public ReplicationAPIInterface(string uRL, string username, string password)
		{
			URL = uRL;
			Username = username;
			Password = password;

			_client = new SerializableHttpsClient();
			_client.SetAuthentication(new ManualAuthenticationMethod(
				new AuthenticationHeaderValue(
				"Basic",
				Convert.ToBase64String(Encoding.GetEncoding("iso-8859-1").GetBytes(string.Format("{0}:{1}",
				Username,
				Password
				))))));
			_client.TimeOut = TimeSpan.FromMinutes(10);
		}

		public async Task<List<SummaryResponseDataItem>> GetSummary(string afterLsn)
		{
			var response = await _client.GetAsync<SummaryRequest, SummaryResponse>(
				new SummaryRequest()
				{
					AfterLSN = afterLsn
				},
				URL + "/change-summary"
			);
			return response.Data.Items;
		}

		public async Task<List<T>> GetDetails<T>(string afterLsn, string maxLsn, string schemaName, string tableName)
		{
			var responseList = new List<ChangesResponse>();

			ChangesResponse? _currentChanges = null;
			_currentChanges = await _client.GetAsync<ChangesRequest, ChangesResponse>(
				new ChangesRequest()
				{
					AfterLSN = afterLsn,
					MaxLSN = maxLsn,
					SchemaName = schemaName,
					TableName = tableName,
					PageSize = PageSize
				}, 
				URL + "/change-detail"
			);

			while (_currentChanges != null)
			{
				responseList.Add(_currentChanges);

				if (_currentChanges.Data.Data.CurrentItemCount == _currentChanges.Data.Data.ItemsPerPage)
				{
					_currentChanges = await _client.GetAsync<ChangesRequest, ChangesResponse>(
						new ChangesRequest()
						{
							AfterLSN = _currentChanges.Data.Data.NextRequestParams.AfterLSN,
							MaxLSN = maxLsn,
							AfterSeqVal = _currentChanges.Data.Data.NextRequestParams.AfterSeqVal,
							AfterCommandId = _currentChanges.Data.Data.NextRequestParams.AfterCommandId,
							AfterOperation = _currentChanges.Data.Data.NextRequestParams.AfterOperation,
							SchemaName = schemaName,
							TableName = tableName,
							PageSize = PageSize
						},
						URL + "/change-detail"
					);
				}
				else
					_currentChanges = null;
			}

			return ConvertChanges<T>(responseList);
		}

		public List<T> ConvertChanges<T>(List<ChangesResponse> changes)
		{
			var returnList = new List<T>();

			foreach (var change in changes)
			{
				foreach (var chamgeItems in change.Data.Data.Items.OrderBy(x => double.Parse(x.Version)))
				{
					var dict = BuildTypeDictionary(chamgeItems.Columns);
					foreach (var chamges in chamgeItems.Changes.OrderBy(x => x.Operation))
					{
						var asJson = MergeDataToJson(chamges.Data, dict);
						var newItem = JsonSerializer.Deserialize<T>(asJson, _options);
						if (newItem != null)
							returnList.Add(newItem);
					}
				}
			}

			return returnList;
		}

		private string MergeDataToJson(List<ChangesDataDataItemsChangesData> data, Dictionary<string, string> columnMap)
		{
			var sb = new StringBuilder();

			sb.Append("{");
			foreach (var change in data)
				sb.Append($"\"{change.ColumnName}\":{ConvertValueToJson(change.Value, columnMap[change.ColumnName])},");
			sb.Append("}");

			return sb.ToString();
		}

		private string ConvertValueToJson(object value, string columnType)
		{
			if (value == null)
				return "null";
			switch (columnType)
			{
				case "INT":
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
					return $"\"{value}\"";
			}
		}

		private Dictionary<string, string> BuildTypeDictionary(List<ChangesDataDataItemsColumns> columns)
		{
			var dict = new Dictionary<string, string>();

			foreach(var column in columns)
			{
				if (!dict.ContainsKey(column.Name))
					dict.Add(column.Name, column.Type.ToUpper());
				else
					dict[column.Name] = column.Type.ToUpper();
			}

			return dict;
		}
	}
}
