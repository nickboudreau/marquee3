
using System.Runtime.CompilerServices;

namespace marquee3
{
    static class Program
    {
        public static int globalX;
        public static int globalY;
        public static string[] lines;
        public static int currentLine = 0;
        public static string currentText;
        
        public static String NO_TEXT_FOUND = "Hello world.";

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                string arg = args.Length > 0 ? args[0].ToLower().Trim() : "/s";

                // change settings
                if (arg.StartsWith("/c"))
                {
                    Application.Run(new SettingsForm());
                    return;
                }
                else if (arg.StartsWith("/p"))
                {
                    return;
                }

                var settings = Settings.load();

                lines = System.IO.File.Exists(settings.textFile)
                    ? System.IO.File.ReadAllLines(settings.textFile)
                    : settings.text.Length > 0 ? new[] { settings.text } : new[] { NO_TEXT_FOUND };

                currentText = lines[0];

                // start from off screen, right side
                var virtualScreen = SystemInformation.VirtualScreen;
                globalX = virtualScreen.Right;

                var forms = Screen.AllScreens
                    .Select(screen => new ScreensaverForm(screen))
                    .ToArray();

                foreach (var form in forms)
                    form.Show();

                Application.Run();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
        }
    }
}