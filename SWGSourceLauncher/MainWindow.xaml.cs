using System;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Compression;
using System.Net;
using System.Windows;
using SWGSourceLauncher.Services;
using static System.Formats.Asn1.AsnWriter;
using System.Windows.Controls;

namespace SWGSourceLauncher
{
    enum LauncherStatus
    {
        ready,
        failed,
        downloadingGame,
        downloadingUpdate
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string rootPath;
        private string versiontext;
        private string gameZip;
        private string patchZip;
        private string zipFile;
        private string gameExe;
        private string fullDownload;
        private string patchDownload;
        private string versionDownload;
        public string displayURL;
        public string onlineVersion;
        public bool useGit = false;

        private LauncherStatus _status;
        internal LauncherStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                switch (_status)
                {
                    case LauncherStatus.ready:
                        LaunchUpdatesButton.Visibility = Visibility.Collapsed;
                        progressBar.Visibility = Visibility.Collapsed;
                        LaunchGameButton.Visibility = Visibility.Visible;
                        LaunchConfigButton.Visibility = Visibility.Visible;
                        break;
                    case LauncherStatus.failed:
                        LaunchUpdatesButton.Content = "Update Failed - Retry";
                        break;
                    case LauncherStatus.downloadingGame:
                        LaunchUpdatesButton.Content = "Downloading Game";
                        if (!useGit)
                        {
                            progressBar.Visibility = Visibility.Visible;
                        }
                        break;
                    case LauncherStatus.downloadingUpdate:
                        LaunchUpdatesButton.Content = "Downloading Update";
                        if (!useGit)
                        {
                            progressBar.Visibility = Visibility.Visible;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        public MainWindow()
        {
            InitializeComponent();

            displayURL = (IniLoader.Instance.Read("PatchNotesURL", "General"));
            if (!String.IsNullOrEmpty(displayURL))
            {
                //reloads the browser with the patch note url in the config
                Browser.Navigate(displayURL);
            }

            rootPath = Directory.GetCurrentDirectory();
            versiontext = (IniLoader.Instance.Read("versionFile", "Patching"));
            gameZip = (IniLoader.Instance.Read("GameZip", "Patching"));
            patchZip = (IniLoader.Instance.Read("PatchZip", "Patching"));
            gameExe = (IniLoader.Instance.Read("GameExe", "General"));
            fullDownload = (IniLoader.Instance.Read("FullDLURL", "Patching"));
            patchDownload = (IniLoader.Instance.Read("PatchURL", "Patching"));
            versionDownload = (IniLoader.Instance.Read("VersionURL", "Patching"));

            string tempGit = (IniLoader.Instance.Read("UseGit", "Patching"));
            useGit = tempGit.Equals("true", StringComparison.OrdinalIgnoreCase);

        }
        private void LaunchGameButton_Click(object sender, RoutedEventArgs e)
        {
            Launcher.PlayGame();
            Environment.Exit(0);
        }
        private void LaunchUpdatesButton_Click(object sender, RoutedEventArgs e)
        {
            if (useGit)
            {
                Launcher.Update();
            }
            else
            {
                CheckForUpdates();
            }
        }

        private void LaunchWebsiteButton_Click(object sender, RoutedEventArgs e)
        {
            Launcher.LaunchWebsite();
        }

        private void LaunchConfigButton_Click(object sender, RoutedEventArgs e)
        {
            Launcher.Config();
        }

        private void CheckForUpdates()
        {
            if (File.Exists(versiontext))
            {
                string localVersion = (File.ReadAllText(versiontext));
                VersionText.Text = localVersion.ToString();
                string localV = (File.ReadAllText(versiontext));

                try
                {
                    WebClient webClient = new WebClient();
                    onlineVersion = (webClient.DownloadString(versionDownload));
                    string onlineV = (webClient.DownloadString(versionDownload));

                    if (onlineV.Equals(localV, StringComparison.Ordinal))

                    {
                        Status = LauncherStatus.ready;
                    }
                    else
                    {
                        InstallPatchFiles(onlineVersion);
                    }
                }
                catch (Exception ex)
                {
                    Status = LauncherStatus.failed;
                    MessageBox.Show($"Error checking for game updates: {ex}");
                }
            }
            else
            {
                InstallEntireGame("0.0.0");
    }
        }
        private void InstallPatchFiles(string onlineVer)
        {
            try
            {
                WebClient webClient = new WebClient();
                Status = LauncherStatus.downloadingUpdate;
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadGameCompletedCallback);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                webClient.DownloadFileAsync(new Uri(patchDownload), patchZip, onlineVer);
                zipFile = patchZip;
            }
            catch (Exception ex)
            {
                Status = LauncherStatus.failed;
                MessageBox.Show($"Error installing game files: {ex}");
            }
        }

        private void InstallEntireGame(string zeroVersion)
        {
            try
            {
                WebClient webClient = new WebClient();
                Status = LauncherStatus.downloadingGame;
                //override since the base game may not include patches we want it at 0.0.0
                onlineVersion = zeroVersion;
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadGameCompletedCallback);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                webClient.DownloadFileAsync(new Uri(fullDownload), gameZip, zeroVersion);
                zipFile = gameZip;
            }
            catch (Exception ex)
            {
                Status = LauncherStatus.failed;
                MessageBox.Show($"Error installing game files: {ex}");
            }
        }
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }
        private void DownloadGameCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                ZipFile.ExtractToDirectory(zipFile, rootPath, true);
                File.Delete(zipFile);

                File.WriteAllText(versiontext, onlineVersion);
                VersionText.Text = onlineVersion;
                //Check for updates again, this could be initial install
                CheckForUpdates();
            }
            catch (Exception ex)
            {
                Status = LauncherStatus.failed;
                MessageBox.Show($"Error finishing download: {ex}");
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            if (useGit)
            {
                Launcher.Update();
                Status = LauncherStatus.ready;
            }
            else
            {
                CheckForUpdates();
            }
        }
    }
}
