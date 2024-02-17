namespace Ecom.API.Errors
{
    public class BaseCommonResponse
    {
        public BaseCommonResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? DefaultMessageForStatusCode(statusCode);
        }

        private string DefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "bad request",
                401 => "not authorize",
                404 => "resource not found",
                500 => "server error",
                _ => null
            };
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
