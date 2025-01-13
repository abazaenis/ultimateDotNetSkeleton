namespace UltimateDotNetSkeleton.Application.DTOs.Token
{
	public record TokenDto
	{
		public TokenDto(string accessToken, string refreshToken)
		{
			AccessToken = accessToken;
			RefreshToken = refreshToken;
		}

		public string AccessToken { get; set; }

		public string RefreshToken { get; set; }
	}
}
