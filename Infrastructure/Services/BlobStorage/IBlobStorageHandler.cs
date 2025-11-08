namespace UltimateDotNetSkeleton.Infrastructure.Services.BlobStorage
{
	public interface IBlobStorageHandler
	{
		Task<Stream> DownloadFileAsync(string containerName, string fileName);

		Task<string> UploadFileAsync(IFormFile file, string containerName, string? fileName = null);

		Task<bool> DeleteFileAsync(string containerName, string fileName);

		Task<bool> FileExistsAsync(string containerName, string fileName);

		string GetFileUrl(string containerName, string fileName);

		string? ExtractFileNameFromUrl(string fileUrl);
	}
}
