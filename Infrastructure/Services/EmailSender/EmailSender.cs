namespace UltimateDotNetSkeleton.Infrastructure.Services.EmailSender
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Microsoft.IdentityModel.Protocols.Configuration;
    using RestSharp;
    using Serilog;
    using UltimateDotNetSkeleton.Application.ConfigurationModels;
    using UltimateDotNetSkeleton.Application.DTOs.Email;
    using UltimateDotNetSkeleton.Application.Exceptions.ServiceUnavailable;

    using ContentType = RestSharp.ContentType;

    public class EmailSender : IEmailSender
	{
		private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		};

		private readonly EmailConfiguration _emailConfig;

		public EmailSender(EmailConfiguration emailConfig)
		{
			_emailConfig = emailConfig;
		}

		public async Task SendVerificationCodeAsync(string recipient, int verificationCode)
		{
			ValidateEmailConfig();

			var client = new RestClient(_emailConfig.Url!);
			var request = new RestRequest(string.Empty, Method.Post);

			AddRequestHeaders(request);

			var variables = new Dictionary<string, string>
			{
				{ "VALIDATION_CODE", verificationCode.ToString() },
				{ "CURRENT_YEAR", DateTime.UtcNow.Year.ToString() },
				{ "UNIQUE", Guid.NewGuid().ToString() },
			};

			var requestBody = BuildRequestBody(recipient, variables, _emailConfig.VerificationTemplateId!);

			request.AddStringBody(requestBody, ContentType.Json);

			var response = await client.ExecuteAsync(request);

			if (!response.IsSuccessful)
			{
				LogEmailSendingFailure(recipient, response);

				throw new EmailServiceUnavailableException();
			}
		}

		public async Task SendTemporaryPasswordAsync(string recipient, string temporaryPassword)
		{
			// TODO: Implement this method.
			throw new NotImplementedException("This method has not yet been implemented.");
		}

		private static void LogEmailSendingFailure(string recipient, RestResponse response, [CallerMemberName] string methodName = "")
		{
			Log.Error(
				"Email sending failed in {Method}. Recipient: {Recipient}, Status: {StatusCode}, Response: {ResponseContent}",
				methodName,
				recipient,
				response.StatusCode,
				response.Content ?? "null");
		}

		private void AddRequestHeaders(RestRequest request)
		{
			request.AddHeader("accept", "application/json");
			request.AddHeader("authkey", _emailConfig.AuthKey!);
			request.AddHeader("content-type", "application/json");
		}

		private void ValidateEmailConfig()
		{
			if (string.IsNullOrWhiteSpace(_emailConfig.AuthKey))
				throw new InvalidConfigurationException("Auth key nije validan.");

			if (string.IsNullOrWhiteSpace(_emailConfig.VerificationTemplateId))
				throw new InvalidConfigurationException("Template ID nije validan.");

			if (string.IsNullOrWhiteSpace(_emailConfig.Url))
				throw new InvalidConfigurationException("URL nije validan.");

			if (string.IsNullOrWhiteSpace(_emailConfig.SenderEmail))
				throw new InvalidConfigurationException("Sender email nije validan.");

			if (string.IsNullOrWhiteSpace(_emailConfig.Domain))
				throw new InvalidConfigurationException("Domena nije validna.");
		}

		private string BuildRequestBody(string recipient, Dictionary<string, string> variables, string templateId)
		{
			var requestBody = new VerificationEmailDto
			{
				Recipients =
				[
					new RecipientDto
					{
						To =
						[
							new EmailRecipientDto
							{
								Email = recipient,
							},
						],
						Variables = variables,
					},
				],
				From = new SenderDto { Email = _emailConfig.SenderEmail! },
				Domain = _emailConfig.Domain!,
				TemplateId = templateId,
			};

			return JsonSerializer.Serialize(requestBody, _jsonSerializerOptions);
		}
	}
}
