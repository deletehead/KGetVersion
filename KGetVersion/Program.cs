using System;
using System.IO;
using System.Net;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;

namespace KGetVersion
{
    class Program
    {
        [DllImport("version.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool GetFileVersionInfo(string lptstrFilename, int dwHandle, int dwLen, byte[] lpData);

        [DllImport("version.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetFileVersionInfoSize(string lptstrFilename, out int lpdwHandle);

        [DllImport("version.dll", CharSet = CharSet.Auto)]
        private static extern bool VerQueryValue(byte[] pBlock, string lpSubBlock, out IntPtr lplpBuffer, out uint puLen);

        static void getFileVersionInfo(string filePath, string type)
        {
            // Get the size of the version info block
            int handle;
            int size = GetFileVersionInfoSize(filePath, out handle);

            if (size == 0)
            {
                Console.WriteLine("[-] Failed to get version info size.");
                return;
            }

            // Allocate a buffer to hold the version info
            byte[] buffer = new byte[size];

            // Get the version info
            if (!GetFileVersionInfo(filePath, 0, size, buffer))
            {
                Console.WriteLine("[-] Failed to get version info.");
                return;
            }

            // Query the version info
            IntPtr versionInfoPtr;
            uint versionInfoLen;
            if (!VerQueryValue(buffer, @"\StringFileInfo\040904B0\FileVersion", out versionInfoPtr, out versionInfoLen))
            {
                Console.WriteLine("Failed to query version info.");
                return;
            }

            // Convert the version info to a string
            string version = Marshal.PtrToStringAuto(versionInfoPtr);

            // Parse the version string and extract the build number
            string[] versionParts1 = version.Split(' ');
            string[] versionParts2 = versionParts1[0].Split('.');
            string buildNumber = $"{versionParts2[2]}-{versionParts2[3]}";
            Console.WriteLine($"[|] {type}_{buildNumber} - {version}");
            getAwfsets(buildNumber, type);
        }

        public static string ToUpperFirstLetter(string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            // convert to char array of the string
            char[] letters = source.ToCharArray();
            // upper case the first char
            letters[0] = char.ToUpper(letters[0]);
            // return the array made of the new char array
            return new string(letters);
        }

        static void getAwfsets(string buildNumber, string type)
        {
            string typeCap = ToUpperFirstLetter(type);
            string baseUrl = $"https://raw.githubusercontent.com/deletehead/KGetVersion/master/offsets.csv";
            //Console.WriteLine($"[>] Requesting {baseUrl}...");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl);
            request.Method = "GET";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string csvContent = reader.ReadToEnd();
                    string[] lines = csvContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    bool found = false;
                    foreach (string line in lines)
                    {
                        if (line.StartsWith($"{type}_{buildNumber}."))
                        {
                            found = true;
                            Console.WriteLine($"[+] {typeCap} offset! - {line}");
                        }
                    }
                    if (!found)
                    {
                        Console.WriteLine("[-] Version not found...check the version manually.");
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("[|] Getting ntoskrnl.exe version");
            getFileVersionInfo(@"C:\Windows\System32\ntoskrnl.exe","ntoskrnl");
            Console.WriteLine("[|] Getting fltMgr.sys version");
            getFileVersionInfo(@"C:\Windows\System32\drivers\fltMgr.sys","fltmgr");
            Console.WriteLine("[|] Getting ci.dll version");
            getFileVersionInfo(@"C:\Windows\System32\ci.dll","ci");

        }
    }
}
