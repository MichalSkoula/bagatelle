namespace Bagatelle.Shared
{
    public enum PlatformType
    {
        Windows,
        Android
    }

    public static class Platform
    {
#if ANDROID
        public static PlatformType Current => PlatformType.Android;
#else
        public static PlatformType Current => PlatformType.Windows;
#endif
    }
}
