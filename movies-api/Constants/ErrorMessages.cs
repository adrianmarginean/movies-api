namespace movies_api.Constants
{
    public static class ErrorMessages
    {
        public const string DataRetrievalFailed = "Failed to retrieve data from the remote source.";
        public const string InvalidJson = "The retrieved JSON data is invalid or empty.";
        public const string HttpRequestError = "An error occurred during the HTTP request.";
        public const string JsonDeserializationError = "An error occurred while deserializing JSON data.";
        public const string GeneralError = "An error occurred.";
    }

}
