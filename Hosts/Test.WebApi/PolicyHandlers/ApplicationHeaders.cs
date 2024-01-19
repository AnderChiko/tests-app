namespace Test.WebApi.PolicyHandlers
{
    public static class ApplicationHeaders
    {
        public static string ApiVersion = "test-api-version";

        public static string ConfigKey(string headerName)
        {
            return $"headers:{headerName}"; // TODO: To be tested.
        }
    }
}
