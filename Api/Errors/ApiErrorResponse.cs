namespace Api.Errors
{
	public class ApiErrorResponse
	{
        public int StatuesCode { get; set; }

        public string Message { get; set; }

        public string? Details { get; set; }
        public ApiErrorResponse(int statuesCode, string message, string details)
        {
            StatuesCode = statuesCode;
            Message = message;
            Details = details;
        }
    }
}
