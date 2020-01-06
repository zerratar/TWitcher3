using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace TWitcher3
{
    public class WindowFinder
    {
        // For Windows Mobile, replace user32.dll with coredll.dll
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public static IntPtr FindWindow(string caption)
        {
            return FindWindow(String.Empty, caption);
        }
    }

    public static class IconExtractor
    {
        public static Bitmap GetIcon(this Process process)
        {
            if (process.HasExited) return null;
            var srcIcon = Icon.ExtractAssociatedIcon(process.MainModule.FileName);
            return new Icon(srcIcon, 64, 64).ToBitmap();
        }
    }
}