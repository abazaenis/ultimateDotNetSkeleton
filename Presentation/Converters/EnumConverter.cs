namespace UltimateDotNetSkeleton.Presentation.Converters
{
	using System.Text.Json;
	using System.Text.Json.Serialization;

	using UltimateDotNetSkeleton.Application.Exceptions.BadRequest;

	public class EnumConverter<T> : JsonConverter<T?>
		where T : struct, Enum
	{
		public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.Null)
				return null;

			if (reader.TokenType == JsonTokenType.String)
			{
				string? enumString = reader.GetString();
				if (Enum.TryParse(enumString, true, out T result))
					return result;

				throw new InvalidEnumBadRequestException(enumString!, typeof(T).Name);
			}

			if (reader.TokenType == JsonTokenType.Number)
			{
				int enumValue = reader.GetInt32();
				if (Enum.IsDefined(typeof(T), enumValue))
					return (T)Enum.ToObject(typeof(T), enumValue);

				throw new JsonException($"Invalid numeric value '{enumValue}' for nullable enum {typeof(T).Name}");
			}

			throw new JsonException($"Unexpected token {reader.TokenType} for nullable enum {typeof(T).Name}");
		}

		public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
		{
			if (value.HasValue)
			{
				writer.WriteStringValue(value.Value.ToString());
			}
			else
			{
				writer.WriteNullValue();
			}
		}
	}
}