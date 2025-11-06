namespace UltimateDotNetSkeleton.Application.DTOs.Email
{
    public record EmailNotificationDto
    {
        public string? Recipient { get; set; }

        public string? Subject { get; set; }

        public string? Body { get; set; }

        public string? Link { get; set; }

        public string? ButtonLabel { get; set; }

        public Guid? UniqueIdentifier { get; set; }
    }
}