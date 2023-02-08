using SWGSourceLauncher.Services;
using System.Diagnostics;

namespace SWGSourceLauncher;
class Launcher
{

    public static void PlayGame()
    {

        Process.Start(IniLoader.Instance.Read("GameExe", "General"));

    }

    public static void LaunchWebsite()
    {
        string url = (IniLoader.Instance.Read("HomepageURL", "General"));

        var startInfo = new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        };
        Process.Start(startInfo);
    
    }

    public static void Update()
    {
        Process.Start(IniLoader.Instance.Read("Updater", "General"));
    }

    public static void Config()
    {
        Process.Start(IniLoader.Instance.Read("SettingsExe", "General"));
    }

}
