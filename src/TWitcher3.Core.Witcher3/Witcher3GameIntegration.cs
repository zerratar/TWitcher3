using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using TWitcher3.Core.Memory;

namespace TWitcher3.Core.Games.Witcher3
{
    public class Witcher3GameIntegration : GameIntegration, IWitcher3
    {
        private const string ProcessName = "witcher";
        private const string ModuleName = "witcher3.exe";
        private const int MaxCommandLength = 128;
        private const int VK_ENTER = 13;

        private readonly ILogger logger;

        private readonly DeepPointer consoleTextPointer;
        private readonly DeepPointer consoleKeyPressDoublePointer;

        private IntPtr consoleKeyCodeIntPtr;
        private IntPtr consoleKeyPressStructIntPtr;
        private IntPtr consoleRowCountPointer;
        private IntPtr consoleCursorSizeIntPtr;
        private IntPtr consoleCursorIndexIntPtr;

        private Process process;
        private bool invokingCommand;

        public Witcher3GameIntegration(ILogger logger)
        {
            this.logger = logger;
            // v1.32
            this.consoleTextPointer = new DeepPointer(ModuleName, 0x02a5cb28, 0x48, 0x0);
            this.consoleKeyPressDoublePointer = new DeepPointer(ModuleName, 0x2BD98C8);
        }

        public override bool IsRunning => HasProcess;

        public override void Update()
        {
            if (!EnsureProcessRunning())
            {
                return;
            }

            if (!EnsureBasePointers())
            {
                return;
            }

            // // prefill with 10 chars to make the next command quicker
            //if (!Volatile.Read(ref invokingCommand))
            //{
            //    FillWithWhitespace(10);
            //}
        }

        public bool Notify(string message)
        {
            return this.InvokeCommand($"n('{message}')");
        }

        public override bool Invoke(IGameCommandReference command)
        {
            if (command == null) return false;

            var txt = command.Name;
            if (command.Arguments != null && command.Arguments.Length > 0)
            {
                txt += $"({string.Join(",", command.Arguments)})";
            }

            return this.InvokeCommand(txt);
        }

        private bool InvokeCommand(string command)
        {
            try
            {
                Volatile.Write(ref invokingCommand, true);
                return this.SetCommandText(command) && this.InvokeCommand();
            }
            finally
            {
                Volatile.Write(ref invokingCommand, false);
            }
        }

        private bool InvokeCommand()
        {
            if (this.consoleKeyCodeIntPtr == IntPtr.Zero) return false;
            bool PressEnter() => this.process.WriteValue(consoleKeyCodeIntPtr, VK_ENTER);
            if (!PressEnter())
            {
                return false;
            }

            // for some reason it doesn't get recognized first time?
            var size = GetCommandTextLength();
            while (size > 0)
            {
                PressEnter();
                size = GetCommandTextLength();
            }

            return true;
        }

        private bool SetCommandText(string text)
        {
            var cmd = text + '\0';
            var data = Encoding.Unicode.GetBytes(cmd);

            if (this.consoleKeyCodeIntPtr == IntPtr.Zero)
            {
                return false;
            }

            var len = text.Length;

            FillWithWhitespace(len);

            if (!consoleTextPointer.DerefOffsets(this.process, out var pointer))
            {
                return false;
            }

            this.process.WriteInt(this.consoleCursorSizeIntPtr, len + 1);
            this.process.WriteInt(this.consoleCursorIndexIntPtr, len);
            return this.process.WriteBytes(pointer, data);
        }

        private void FillWithWhitespace(int len)
        {
            var size = GetCommandTextLength();
            while (size < len + 1)
            {
                this.process.WriteValue(consoleKeyCodeIntPtr, ' ');
                size = GetCommandTextLength();
            }

            // stop any further whitespace injections
            WriteByte(0);
        }

        private void WriteByte(byte c)
        {
            if (consoleKeyCodeIntPtr == IntPtr.Zero) return;
            this.process.WriteValue(consoleKeyCodeIntPtr, c);
        }

        private int GetCommandTextLength()
        {
            if (!consoleTextPointer.DerefOffsets(this.process, out _))
            {
                return 0;
            }

            var str = consoleTextPointer.DerefString(process, ReadStringType.UTF16, MaxCommandLength);
            return string.IsNullOrEmpty(str) ? 0 : str.Length;
        }

        private bool EnsureBasePointers()
        {
            if (this.consoleKeyCodeIntPtr == IntPtr.Zero)
            {
                try
                {
                    if (!this.consoleKeyPressDoublePointer.DerefOffsets(process, out var startPtr))
                    {
                        return false;
                    }

                    if (!process.ReadPointer(startPtr, out var structPtr))
                    {
                        return false;
                    }

                    // dx input struct pointer = structPtr
                    this.consoleKeyPressStructIntPtr = structPtr;
                    this.consoleCursorSizeIntPtr = structPtr + 0x50;
                    this.consoleKeyCodeIntPtr = structPtr + 0x54;
                    this.consoleRowCountPointer = structPtr + 0x58;
                    this.consoleCursorIndexIntPtr = structPtr + 0x5C;
                    this.logger.WriteDebug("Witcher 3 Integration is ready.");
                }
                catch (Exception exc)
                {
                    logger.WriteError(exc.ToString());
                    return false;
                }
            }

            return true;
        }

        private bool EnsureProcessRunning()
        {
            if (HasProcess)
                return true;

            this.process = System.Diagnostics
                .Process.GetProcesses()
                .FirstOrDefault(x => x.ProcessName.Contains(ProcessName, StringComparison.OrdinalIgnoreCase));

            return HasProcess;
        }

        private bool HasProcess => this.process != null && !this.process.HasExited;
    }
}
