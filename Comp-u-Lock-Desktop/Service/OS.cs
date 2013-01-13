﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service
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
                            //Check to see what version of windws you are on
                            switch (Version.Major)
                            {
                                case 6: // Vista Kernal
                                    switch (Version.Minor)
                                    {
                                        case 2:// Win 8
                                            return Windows.Eight;
                                            break;
                                        case 1:// Win 7
                                            return Windows.Seven;
                                            break;
                                        case 0:// Vista
                                            return Windows.Vista;
                                            break;
                                    }
                                    break;
                                case 5: // XP Kernal
                                    switch (Version.Minor)
                                    {
                                        case 1:
                                            return Windows.Xp;
                                            break;
                                        case 2:// 64 bit
                                            return Windows.Xp;
                                            break;
                                        default:
                                            return Windows.Unsupported;
                                            break;
                                    }
                                    break;
                            }
                            break;
                        }
                    default:
                        return Windows.Unsupported;
                        break;
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