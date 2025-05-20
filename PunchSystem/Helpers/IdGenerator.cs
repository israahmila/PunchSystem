namespace PunchSystem.Helpers
{
    public static class IdGenerator
    {
        public static string New(string prefix = "")
        {
            return $"{prefix}-{Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper()}";
        }
    }

}
