using System;

namespace Data.Enviroment
{
    public static class OS
    {
        public static Version Version
        {
            get
            {
                return Environment.OSVersion.Version;
            }
        }

        public static PlatformID Platform
        {
            get { return Environment.OSVersion.Platform; }
        }

        public static Windows Name
        {
            get
            {
                switch (Platform)
                {
                    case PlatformID.Win32NT:
                        {
                            switch (Version.Major)
                            {
                                case 6:
                                    switch (Version.Minor)
                                    {
                                        case 2:
                                            return Windows.Eight;
                                        case 1:
                                            return Windows.Seven;
                                        case 0:
                                            return Windows.Vista;
                                    }
                                    break;
                                case 5:
                                    switch (Version.Minor)
                                    {
                                        case 1:
                                            return Windows.Xp;
                                        case 2:
                                            return Windows.Xp;
                                        default:
                                            return Windows.Unsupported;
                                    }
                            }
                            break;
                        }
                    default:
                        return Windows.Unsupported;
                }
                return Windows.Unsupported;
            }
        }
    }

    public enum Windows
    {
        Eight,
        Seven,
        Vista,
        Xp,
        Unsupported
    }
}