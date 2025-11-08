namespace UltimateDotNetSkeleton.Infrastructure.Services.BlobStorage
{
    using System.IO;
    using Azure;
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Serilog;
    using UltimateDotNetSkeleton.Application.Exceptions.BadRequest;

    public class BlobStorageHandler : IBlobStorageHandler
	{
		private const int MaxFileSizeInBytes = 3 * 1024 * 1024;
		private readonly BlobServiceClient _blobServiceClient;
		private readonly string _baseUrl;

		public BlobStorageHandler(IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("AzureBlobStorage")
				?? throw new InvalidOperationException("Azure Blob Storage connection string is not configured");
			_blobServiceClient = new BlobServiceClient(connectionString);
			_baseUrl = $"https://{ExtractAccountFromConnectionString(connectionString)}.blob.core.windows.net";
		}

		public async Task<Stream> DownloadFileAsync(string containerName, string fileName)
		{
			var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
			var blobClient = containerClient.GetBlobClient(fileName);

			var memoryStream = new MemoryStream();
			await blobClient.DownloadToAsync(memoryStream);
			memoryStream.Position = 0;

			return memoryStream;
		}

		public async Task<string> UploadFileAsync(IFormFile file, string containerName, string? fileName = null)
		{
			if (file == null || file.Length == 0)
				throw new InvalidBlobUploadBadRequestException("Fajl je prazan.");

			if (file.Length > MaxFileSizeInBytes)
				throw new InvalidBlobUploadBadRequestException("Maksimalna veličina fajla je 15MB.");

			fileName = GenerateUniqueFileName(file.FileName);

			using var stream = file.OpenReadStream();

			var containerClient = await GetOrCreateContainerAsync(containerName);
			var blobClient = containerClient.GetBlobClient(fileName);

			var blobHttpHeaders = new BlobHttpHeaders
			{
				ContentType = file.ContentType,
			};

			await blobClient.UploadAsync(stream, new BlobUploadOptions
			{
				HttpHeaders = blobHttpHeaders,
				Conditions = new BlobRequestConditions { IfNoneMatch = ETag.All },
			});

			return GetFileUrl(containerName, fileName);
		}

		public async Task<bool> DeleteFileAsync(string containerName, string fileName)
		{
			var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
			var blobClient = containerClient.GetBlobClient(fileName);

			var response = await blobClient.DeleteIfExistsAsync();

			return response.Value;
		}

		public async Task<bool> FileExistsAsync(string containerName, string fileName)
		{
			var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
			var blobClient = containerClient.GetBlobClient(fileName);

			var response = await blobClient.ExistsAsync();
			return response.Value;
		}

		public string GetFileUrl(string containerName, string fileName)
		{
			return $"{_baseUrl}/{containerName}/{fileName}";
		}

		public string? ExtractFileNameFromUrl(string fileUrl)
		{
			if (string.IsNullOrEmpty(fileUrl))
				return null;

			try
			{
				var uri = new Uri(fileUrl);
				var segments = uri.Segments;
				return segments.Length > 0 ? segments[^1] : null;
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Failed to extract filename from URL: {FileUrl}", fileUrl);
				return null;
			}
		}

		private static string GenerateUniqueFileName(string originalFileName)
		{
			const string separator = "__"; // Do not change this separator, it is used across the stack.
			var extension = Path.GetExtension(originalFileName);
			var nameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
			nameWithoutExtension = SanitizeFileName(nameWithoutExtension);
			var guid = Guid.NewGuid().ToString();

			return $"{guid}{separator}{nameWithoutExtension}{extension}";
		}

		private static string ExtractAccountFromConnectionString(string connectionString)
		{
			var parts = connectionString.Split(';');
			var accountNamePart = parts.FirstOrDefault(p => p.StartsWith("AccountName="));
			return accountNamePart?.Split('=')[1] ?? "unknown";
		}

		private static string SanitizeFileName(string fileName)
		{
			var invalidChars = new char[] { '/', '\\', ':', '*', '?', '"', '<', '>', '|' };

			foreach (var invalidChar in invalidChars)
			{
				fileName = fileName.Replace(invalidChar, '_');
			}

			while (fileName.Contains("__"))
			{
				fileName = fileName.Replace("__", "_");
			}

			return fileName.Trim('_');
		}

		private async Task<BlobContainerClient> GetOrCreateContainerAsync(string containerName)
		{
			var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
			await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
			return containerClient;
		}
	}
}
