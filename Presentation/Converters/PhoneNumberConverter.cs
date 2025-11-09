namespace UltimateDotNetSkeleton.Presentation.Converters
{
	using System;
	using System.Text.Json;
	using System.Text.Json.Serialization;
	using System.Text.RegularExpressions;
	using UltimateDotNetSkeleton.Application.Exceptions.BadRequest;

	public class PhoneNumberConverter : JsonConverter<string>
	{
		private const string BosnianCountryCode = "387";
		private const string BosnianCountryCodePrefix = "+387";
		private static readonly Dictionary<string, int> PrefixLengths = new()
		{
			{ "60", 9 },
			{ "61", 8 },
			{ "62", 8 },
			{ "63", 8 },
			{ "64", 9 },
			{ "65", 8 },
			{ "66", 8 },
			{ "67", 9 },
		};

		public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var value = reader.GetString();

			if (string.IsNullOrEmpty(value))
				return value;

			return NormalizePhoneNumber(value);
		}

		public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value);
		}

		private static string NormalizePhoneNumber(string phoneNumber)
		{
			if (string.IsNullOrWhiteSpace(phoneNumber))
				return string.Empty;

			var digits = ExtractDigits(phoneNumber);
			var normalized = NormalizeBosnianPrefix(digits, phoneNumber);
			normalized = RemoveRedundantZero(normalized);
			ValidatePhoneNumberPrefix(normalized, phoneNumber);

			return normalized;
		}

		private static string ExtractDigits(string phoneNumber) => Regex.Replace(phoneNumber, @"\D", string.Empty);

		private static string NormalizeBosnianPrefix(string digits, string originalNumber)
		{
			if (digits.StartsWith(BosnianCountryCode))
				return "+" + digits;

			if (digits.StartsWith("00" + BosnianCountryCode))
				return string.Concat("+", digits.AsSpan(2));

			if (digits.StartsWith('0'))
			{
				if (digits.Length is 9 or 10)
					return string.Concat(BosnianCountryCodePrefix, digits.AsSpan(1));

				throw new InvalidPhoneNumberBadRequestException($"Broj telefona nije u validnom formatu: {originalNumber}");
			}

			throw new InvalidPhoneNumberBadRequestException($"Broj telefona nije u validnom formatu: {originalNumber}");
		}

		private static string RemoveRedundantZero(string normalized)
		{
			if (normalized.StartsWith(BosnianCountryCodePrefix + "0"))
				return string.Concat(BosnianCountryCodePrefix, normalized.AsSpan(5));

			return normalized;
		}

		private static void ValidatePhoneNumberPrefix(string normalized, string originalNumber)
		{
			var prefix = normalized.Substring(4, 2);

			if (!PrefixLengths.TryGetValue(prefix, out var expectedLength))
			{
				throw new InvalidPhoneNumberBadRequestException($"Prefiks nije validan: {prefix}");
			}

			var digitCount = normalized.Length - 4; // Exclude Bosnian country code
			if (digitCount != expectedLength)
			{
				throw new InvalidPhoneNumberBadRequestException($"Dužina nije validna za prefiks {prefix}: {originalNumber}");
			}
		}
	}
}