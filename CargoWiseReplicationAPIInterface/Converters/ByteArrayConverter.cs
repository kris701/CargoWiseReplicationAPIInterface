using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CargoWiseReplicationAPIInterface.Converters
{
	internal class ByteArrayConverter : JsonConverter<byte[]>
	{
		public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options) =>
			JsonSerializer.Serialize(writer, value.AsEnumerable());

		public override byte[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
			reader.TokenType switch
			{
				JsonTokenType.String => reader.GetBytesFromBase64(),
				JsonTokenType.StartArray => JsonSerializer.Deserialize<List<byte>>(ref reader)!.ToArray(),
				JsonTokenType.Null => null,
				_ => throw new JsonException(),
			};
	}
}
