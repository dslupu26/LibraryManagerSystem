using Common;

namespace Bussiness
{
    public class CurrentUserProviderMock : ICurrentUserProvider
    {
        public string CurrentUser => "THE USER";
    }
}
