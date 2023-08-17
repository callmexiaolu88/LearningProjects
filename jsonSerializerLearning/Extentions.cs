namespace jsonSerializerLearning
{
    public static class Extentions
    {
        public static bool IsNull(this string str) => string.IsNullOrEmpty(str);
    }
}