namespace DatingApp.Api.Helpers
{
    public static class MessageHelpers
    {
        public static string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            if (stringCompare)
                return caller + "-" + other;
            else
                return other + "-" + caller;
        }
    }
}
