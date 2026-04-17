
namespace marquee3
{
    public class ScreensaverForm : Form
    {
        System.Windows.Forms.Timer timer;
        string[] lines = Array.Empty<string>();
        int currentLine = 0;
        string currentText = String.Empty;

        Settings settings;
        Point lastMousePosition;

        private String backgroundImagePath = String.Empty;
        private Image? backgroundImage;
        private Brush brush;
        private Font font;
        Rectangle virtualScreen;

        public ScreensaverForm(Screen screen)
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Bounds = screen.Bounds;

            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.DoubleBuffered = true;

            Cursor.Hide();

            settings = Settings.load();

            loadText();
            loadImage();
            startLine();

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 2;     // 30 fps
            timer.Tick += timerTick;
            timer.Start();

            lastMousePosition = Cursor.Position;
            brush = new SolidBrush(settings.getTextColor());
            font = settings.getFont();
            virtualScreen = SystemInformation.VirtualScreen;
        }

        void loadText()
        {
            if (File.Exists(settings.textFile))
                lines = File.ReadAllLines(settings.textFile);
            else
                lines = new[] { settings.text };
        }

        private void setRandomYPosition()
        {
            Random rand = new Random();
            Program.globalY = rand.Next(this.Height - TextRenderer.MeasureText(currentText, settings.getFont()).Height);
        }

        void startLine()
        {
            if (lines.Length == 0) lines = new[] { Program.NO_TEXT_FOUND };

            currentText = lines[currentLine];
            setRandomYPosition();
        }

        private void timerTick(object? sender, EventArgs e)
        {
            var settings = this.settings;
            var virtualScreen = SystemInformation.VirtualScreen;

            Program.globalX -= settings.speed;

            int textWidth = TextRenderer.MeasureText(Program.currentText, settings.getFont()).Width;

            // when text fully leaves left side of virtual desktop
            if (Program.globalX < virtualScreen.Left - textWidth)
            {
                setRandomYPosition();
                Program.currentLine = (Program.currentLine + 1) % Program.lines.Length;
                Program.currentText = Program.lines[Program.currentLine];

                // restart from right edge again
                Program.globalX = virtualScreen.Right;
            }

            // exit on mouse move
            if (Cursor.Position != lastMousePosition)
            {
                Application.Exit();
            }

            Invalidate();
        }

        private void loadImage()
        {
            if (settings.backgroundImagePath != "" && File.Exists(settings.backgroundImagePath))
            {
                backgroundImage = Image.FromFile(settings.backgroundImagePath);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (backgroundImage != null)
            {
                e.Graphics.DrawImageUnscaled(backgroundImage, new Rectangle(0, 0, this.Width, this.Height));
            }
            else
            {
                e.Graphics.Clear(settings.getBackgroundColor());
            }

            int localX = Program.globalX - this.Bounds.Left;
            int localY = Program.globalY;

            e.Graphics.DrawString(
                Program.currentText,
                font,
                brush,
                localX,
                localY
            );
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            Application.Exit();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Application.Exit();
        }
    }
}