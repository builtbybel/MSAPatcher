using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;

namespace MSAPatcher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to MSAPatcher!");
            Console.WriteLine("This application allows you to bypass the network requirement during Windows 11 setup.");
            Console.WriteLine("Additionally, it will copy the required files to your USB drive if necessary.");
            Console.WriteLine("Please choose the method you want to use:");
            Console.WriteLine("1. Classic BypassNRO - This method creates a patched USB stick that must be plugged into the PC during the Windows 11 setup welcome screen. It modifies a registry key to bypass the network requirement.");
            Console.WriteLine("2. Direct Local Account Creation (Windows 11 Home/Pro Only)");
            Console.WriteLine("3. Visit the github page for more information and updates.");
            Console.Write("Enter your choice (1, 2 or 3): ");

            string choice = Console.ReadLine();
            if (choice == "1")
            {
                CopyFilesToUSB();
                ApplyBypassNRO();
            }
            else if (choice == "2")
            {
                CreateLocalAccount();
            }
            else if (choice == "3")
            {
                Process.Start("https://github.com/builtbybel/MSAPatcher");

            }
            else
            {
                Console.WriteLine("Invalid choice. Exiting...");
            }
        }

            static void CopyFilesToUSB()
        {
            string usbDrive = GetUsbDrive();
            if (!string.IsNullOrEmpty(usbDrive))
            {
                try
                {
                    string appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    string batchFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bypassnro.cmd");
                    string targetBatchPath = Path.Combine(usbDrive, "bypassnro.cmd");
                    string targetAppPath = Path.Combine(usbDrive, "MSAPatcher.exe");

                    if (!File.Exists(targetBatchPath) || !File.Exists(targetAppPath))
                    {
                        Console.WriteLine("Copying files to USB drive...");
                        File.Copy(batchFilePath, targetBatchPath, true);
                        File.Copy(appPath, targetAppPath, true);
                        Console.WriteLine($"Files successfully copied to: {usbDrive}");
                    }
                    else
                    {
                        Console.WriteLine("Files already exist on USB drive. Skipping copy.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error copying files: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("No USB drive detected.");
            }
        }

        static void ApplyBypassNRO()
        {
            if (!IsAdministrator())
            {
                Console.WriteLine("Please run as administrator!");
                RestartAsAdmin();
                return;
            }

            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c reg add \"HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\OOBE\" /v BypassNRO /t REG_DWORD /d 1 /f",
                    UseShellExecute = true,
                    Verb = "runas"
                };
                Process process = Process.Start(psi);
                process.WaitForExit();
                Console.WriteLine("BypassNRO successfully set!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            AskForRestart();
        }

        static void CreateLocalAccount()
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c start ms-cxh:localonly",
                    UseShellExecute = true
                };
                Process.Start(psi);
                Console.WriteLine("Local account creation process started.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void AskForRestart()
        {
            Console.WriteLine("\nSetup is complete. Do you want to restart the computer now? (Y/N)");
            string response = Console.ReadLine();
            if (response?.Trim().ToUpper() == "Y")
            {
                Process.Start("shutdown", "/r /t 0");
            }
        }

        static bool IsAdministrator()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        static void RestartAsAdmin()
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = Process.GetCurrentProcess().MainModule.FileName,
                Verb = "runas",
                UseShellExecute = true
            };
            try
            {
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error restarting as admin: {ex.Message}");
            }
        }

        static string GetUsbDrive()
        {
            var drives = DriveInfo.GetDrives()
                                   .Where(d => d.DriveType == DriveType.Removable && d.IsReady)
                                   .Select(d => d.Name)
                                   .FirstOrDefault();
            return drives;
        }
    }
}