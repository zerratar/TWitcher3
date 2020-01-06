using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace TWitcher3.Core.WinApi
{


    public class LowLevelKeyboardListener
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public event EventHandler<KeyPressedArgs> OnKeyPressed;

        private LowLevelKeyboardProc _proc;
        private IntPtr _hookID = IntPtr.Zero;

        public LowLevelKeyboardListener()
        {
            _proc = HookCallback;
        }

        public void HookKeyboard(Process p = null)
        {
            p = p ?? Process.GetCurrentProcess();
            _hookID = SetHook(p, _proc);
        }

        public void UnHookKeyboard()
        {
            UnhookWindowsHookEx(_hookID);
        }

        private IntPtr SetHook(Process curProcess, LowLevelKeyboardProc proc)
        {
            using (curProcess)
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                if (OnKeyPressed != null) { OnKeyPressed(this, new KeyPressedArgs(vkCode)); }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
    }

    public class KeyPressedArgs : EventArgs
    {
        public int KeyPressed { get; private set; }

        public KeyPressedArgs(int key)
        {
            KeyPressed = key;
        }
    }
    public class KeyboardKeys
    {
        // keep em static readonly so the static ctor can find them
        public static readonly byte VK_F = 0x46;
        public static readonly byte VK_F12 = 0x7B;
        public static readonly byte VK_RETURN = 0x0D;
        public static readonly byte VK_ENTER = 0x0D;
        public static readonly byte VK_SPACE = 0x20;
        public static readonly byte VK_TAB = 0x09;
        public static readonly byte VK_END = 0x23;
        public static readonly byte VK_HOME = 0x24;

        public static readonly byte VK_PAGE_UP = 0x21;
        public static readonly byte VK_PAGE_DOWN = 0x22;

        public static readonly byte VK_VOLUME_DOWN = 0xAE;
        public static readonly byte VK_VOLUME_UP = 0xAF;
        public static readonly byte VK_VOLUME_MUTE = 0xAD;


        public static readonly byte KEYEVENTF_KEYUP = 0x0002;
        public static readonly byte VK_CTRL = 0x11;
        public static readonly byte VK_SNAPSHOT = 0x2C;
        public static readonly byte VK_ALT = 0x12;
        public static readonly byte VK_RIGHT = 0x27;
        public static readonly byte VK_UP = 0x26;
        public static readonly byte VK_LEFT = 0x25;
        public static readonly byte VK_DOWN = 0x28;

        private static readonly ConcurrentDictionary<string, byte> keyCodes
            = new ConcurrentDictionary<string, byte>();

        static KeyboardKeys()
        {
            var vkeys = typeof(KeyboardKeys).GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var v in vkeys)
            {
                if (!v.Name.StartsWith("VK_")) continue;
                try
                {
                    keyCodes[v.Name.Split('_').LastOrDefault().ToLower()] = (byte)v.GetValue(null);
                }
                catch { }
            }
        }

        public static byte GetKey(string name)
        {
            if (keyCodes.TryGetValue(name.ToLower(), out var key))
            {
                return key;
            }

            return 0;
        }
    }

    public class Keyboard
    {
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        public static void KeyPress(byte key)
        {
            const int KEYEVENTF_EXTENDEDKEY = 0x1;
            const int KEYEVENT_KEYUP = 0x2;
            // 0x2D = insert
            keybd_event(key, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
            keybd_event(key, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENT_KEYUP, (UIntPtr)0);
        }
    }
}
