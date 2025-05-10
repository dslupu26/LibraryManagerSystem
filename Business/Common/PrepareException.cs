namespace Bussiness.Common
{
    public static class PrepareException
    {
        public static string Prepare(Exception ex)
        {
            ArgumentNullException.ThrowIfNull(ex, nameof(ex));

            var businessException = ex as BusinessException;
            if (businessException != null)
            {
                return ex.Message;
            }
            else
            {
                return $"Unexpected error occured! ex: {ex.Message}.";
            }

        }
    }
}
