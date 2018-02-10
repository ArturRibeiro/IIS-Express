using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Scorponok.Web.Api.Unit.Test.Integration.Helper
{
    /// <summary>
    /// Classe Helper para executar o IIS Express. Sem precisar rodas todas as aplicação.
    /// </summary>
    internal class IISExpress
    {
        const string IIS_EXPRESS_PATH = @"IIS Express\iisexpress.exe";

        private static Dictionary<string, Process> _process = new Dictionary<string, Process>();

        #region Propriedades

        private string Path { get; set; }
        private int? Port { get; set; }
        #endregion

        private IISExpress(string path, string key, int value)
        {
            this.Path = path;
            this.Port = value;

            var args = $"/{nameof(Path)}:{this.Path} /{nameof(this.Port)}:{this.Port}";

            var programFiles = ProgramFilesx86();
            var iisPAth = System.IO.Path.Combine(programFiles, IIS_EXPRESS_PATH);

            var process = _process[key];

            process.StartInfo = new ProcessStartInfo
            {
                FileName = iisPAth,
                Arguments = args,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            process.Start();
        }

        public static IISExpress Start(string path, KeyValuePair<string, int> port)
        {
            _process.Add(port.Key, new Process());

            return new IISExpress(path, port.Key, port.Value);
        }

        public void Stop()
        {
            foreach (var p in _process)
            {
                if (p.Value == null) continue;

                p.Value.Kill();
                p.Value.WaitForExit();
            }

            _process.Clear();
        }

        static string ProgramFilesx86()
        {
            if (8 == IntPtr.Size || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
                return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);

            return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        }

        internal class NativeMethods
        {
            [DllImport("user32.dll", SetLastError = true)]
            internal static extern IntPtr GetTopWindow(IntPtr hWnd);
            [DllImport("user32.dll", SetLastError = true)]
            internal static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);
            [DllImport("user32.dll", SetLastError = true)]
            internal static extern uint GetWindowThreadProcessId(IntPtr hwnd, out uint lpdwProcessId);
            [DllImport("user32.dll", SetLastError = true)]
            internal static extern bool PostMessage(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        }
    }
}
