
using System.Text.Json;

namespace marquee3
{
    public class Settings
    {
        public string textFile { get; set; } = "";
        public string text { get; set; } = Program.NO_TEXT_FOUND;
        public int fontSize { get; set; } = 48;
        public string fontName { get; set; } = "Consolas";
        public string textColor { get; set; } = "#00FF00";
        public string backgroundColor { get; set; } = "#000000";
        public string backgroundImagePath { get; set; } = "";
        public int speed { get; set; } = 5;

        public static Settings load()
        {
            string dir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "marquee3"
            );
            string file = Path.Combine(dir, "settings.json");

            if (!File.Exists(file))
                return new Settings();

            string json = File.ReadAllText(file);
            return JsonSerializer.Deserialize<Settings>(json);
        }

        public Color getTextColor() => ColorTranslator.FromHtml(textColor);
        public Color getBackgroundColor() => ColorTranslator.FromHtml(backgroundColor);
        public Font getFont() => new Font(fontName, fontSize);
    }
}