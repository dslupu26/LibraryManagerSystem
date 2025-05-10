namespace Bussiness.Common
{
    public class BusinessException : Exception
    {
        public BusinessException(string exception) : base(exception) { }
    }
}
