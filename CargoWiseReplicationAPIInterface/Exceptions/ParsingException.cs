using System;
using System.Collections.Generic;
using System.Text;

namespace CargoWiseReplicationAPIInterface.Exceptions
{
	public class ParsingException : Exception
	{
		public string JsonBody { get; set; } = "";

		public ParsingException()
		{
		}

		public ParsingException(string? message, string jsonBody) : base(message)
		{
			JsonBody = jsonBody;
		}

		public ParsingException(string? message, string jsonBody, Exception? innerException) : base(message, innerException)
		{
			JsonBody = jsonBody;
		}
	}
}
