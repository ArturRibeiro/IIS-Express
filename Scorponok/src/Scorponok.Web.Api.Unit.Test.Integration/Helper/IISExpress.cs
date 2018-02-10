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

        private readonly Process _process;

        #region Propriedades

        private string Path { get; set; }
        private int? Port { get; set; }
        #endregion

        private IISExpress(string path, int port)
        {
            this.Path = path;
            this.Port = port;

            var iisPAth = System.IO.Path.Combine(ProgramFilesx86(), IIS_EXPRESS_PATH);

            _process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = iisPAth,
                    Arguments = $"/{nameof(Path)}:{this.Path} /{nameof(this.Port)}:{this.Port}",
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            _process.Start();
        }

        public static IISExpress Start(string path, int port)
        {
            return new IISExpress(path, port);
        }

        public void Stop()
        {
            _process.Kill();
            _process.WaitForExit();
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
