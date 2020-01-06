using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace TWitcher3
{
    public class ProcessURIGenerator
    {
        [DllImport("USER32.DLL")]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        public static string Generate(Process process)
        {
            var sb = new StringBuilder(100);
            GetClassName(process.MainWindowHandle, sb, sb.Capacity);
            return $"{process.MainWindowTitle}:{sb}:{process.ProcessName}.exe".Replace("\\", "\\\\");
        }
    }

}