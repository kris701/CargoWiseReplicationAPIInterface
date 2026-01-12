using CargoWiseReplicationAPIInterface.Models.Changes;
using CargoWiseReplicationAPIInterface.Models.Summary;
using SerializableHttps;
using SerializableHttps.AuthenticationMethods;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CargoWiseReplicationAPIInterface.Services
{
	public class ReplicationAPIHTTPService : IReplicationAPIService
	{
		/// <summary>
		/// The URL to the replication endpoint.
		/// This should be in a format like "https://(url)/Services/api/replication/"
		/// </summary>
		public string URL { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public int PageSize { get; set; } = 1000;

		private readonly SerializableHttpsClient _client;

		public ReplicationAPIHTTPService(string uRL, string username, string password)
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

		public async Task<SummaryResponse> GetSummary(string afterLsn)
		{
			return await _client.GetAsync<SummaryRequest, SummaryResponse>(
				new SummaryRequest()
				{
					AfterLSN = afterLsn
				},
				URL + "/change-summary"
			);
		}

		public async Task<ChangesResponse> GetChanges(string afterLsn, string maxLsn, string schemaName, string tableName)
		{
			var detailsResponse = await _client.GetAsync<ChangesRequest, string>(
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
			detailsResponse = ReplaceInvalidCharacters(detailsResponse);
			var response = JsonSerializer.Deserialize<ChangesResponse>(detailsResponse);
			if (response == null)
				throw new Exception("Invalid response!");
			return response;
		}

		public async Task<ChangesResponse?> GetChangesFromLast(ChangesResponse last, string maxLsn, string schemaName, string tableName)
		{
			if (last.Data.Data.CurrentItemCount == last.Data.Data.ItemsPerPage)
			{
				var detailsResponse = await _client.GetAsync<ChangesRequest, string>(
					new ChangesRequest()
					{
						AfterLSN = last.Data.Data.NextRequestParams.AfterLSN,
						MaxLSN = maxLsn,
						AfterSeqVal = last.Data.Data.NextRequestParams.AfterSeqVal,
						AfterCommandId = last.Data.Data.NextRequestParams.AfterCommandId,
						AfterOperation = last.Data.Data.NextRequestParams.AfterOperation,
						SchemaName = schemaName,
						TableName = tableName,
						PageSize = PageSize
					},
					URL + "/change-detail"
				);
				detailsResponse = ReplaceInvalidCharacters(detailsResponse);
				return JsonSerializer.Deserialize<ChangesResponse>(detailsResponse);
			}
			return null;
		}

		private string ReplaceInvalidCharacters(string text)
		{	
			text = text.Replace("\u001e", "");
			text = text.Replace("\\u001e", "");
			return text;
		}
	}
}
