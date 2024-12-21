namespace UltimateDotNetSkeleton.Application.Exceptions.BadRequest
{
    public sealed class IdParametersBadRequestException : BadRequestException
    {
        public IdParametersBadRequestException()
            : base("Parameter ids is null.")
        {
        }
    }
}
