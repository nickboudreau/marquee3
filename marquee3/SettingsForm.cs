
using System.Text.Json;
using static System.Windows.Forms.LinkLabel;

namespace marquee3
{
    public class SettingsForm : Form
    {
        static int WIDTH = 600;
        static int HEIGHT = 530;
        static int PREVIEW_WIDTH = WIDTH - 75;

        Settings settings;

        TextBox txtFile;
        Button btnBrowse;
        TextBox txtText;

        TextBox txtFont;
        Button btnFont;
        TextBox txtFontColour;
        Button btnTextColour;

        TextBox txtBgColour;
        Button btnBgColour;
        TextBox txtBgImage;
        Button btnBgImage;

        NumericUpDown numFontSize;
        TrackBar slider;

        Panel previewPanel;
        Label previewLabel;

        Button btnOk;
        Button btnCancel;

        Color textColour;
        Color bgColour;
        String bgImagePath = String.Empty;
        Font? selectedFont;

        System.Windows.Forms.Timer timer;
        int xPreview = 0;

        enum FileBrowseType
        {
            NONE,
            TEXT_FILE,
            BG_IMAGE_FILE
        }

        public SettingsForm()
        {
            this.Text = "Screensaver Settings";
            this.Width = WIDTH;
            this.Height = HEIGHT;

            // ======== File selection ========
            Label lblFile = new Label()
            {
                Text = "Text File:",
                Left = 10,
                Top = 25,
                Width = 60
            };

            txtFile = new TextBox()
            {
                Left = 70,
                Top = 21,
                Width = 380
            };
            
            txtFile.TextChanged += (s, e) =>
            {
                if (txtFile.Text.Length > 0)
                {
                    txtText.Text = "";
                    updatePreview();
                }
            };

            btnBrowse = new Button()
            {
                Text = "Browse...",
                Left = 460,
                Top = 21
            };

            btnBrowse.Tag = FileBrowseType.TEXT_FILE;
            btnBrowse.Click += browseFile;

            Label lblText = new Label()
            {
                Text = "Text:",
                Left = 10,
                Top = 55,
                Width = 60
            };

            txtText = new TextBox()
            {
                Left = 70,
                Top = 51,
                Width = 380,
            };

            txtText.TextChanged += (s, e) =>
            {
                if (txtText.Text.Length > 0)
                {
                    txtFile.Text = "";
                    updatePreview();
                }
            };

            GroupBox textGroup = new GroupBox()
            {
                Text = "Text settings",
                Left = 20,
                Top = 10,
                Width = WIDTH - 55,
                Height = 85
            };

            txtFile.Leave += (s, e) =>
            {
                if (txtFile.Text.Length > 0)
                {
                    txtText.Text = "";
                }
            };

            txtText.Leave += (s, e) =>
            {
                if (txtText.Text.Length > 0)
                {
                    txtFile.Text = "";
                }
            };

            textGroup.Controls.Add(lblFile);
            textGroup.Controls.Add(txtFile);
            textGroup.Controls.Add(btnBrowse);
            textGroup.Controls.Add(lblText);
            textGroup.Controls.Add(txtText);

            // ======== Font ========
            Label lblFont = new Label()
            {
                Text = "Font:",
                Left = 10,
                Top = 25,
                Width = 75
            };

            txtFont = new TextBox()
            {
                Left = 90,
                Top = 21,
                Width = 140,
            };

            btnFont = new Button()
            {
                Text = "Set Font...",
                Left = 245,
                Top = 21,
                Width = 85
            };

            btnFont.Click += chooseFont;

            Label lblSize = new Label()
            {
                Text = "Font Size:",
                Left = 150,
                Top = 65
            };

            numFontSize = new NumericUpDown()
            {
                Left = 220,
                Top = 60,
                Width = 60,
                Minimum = 8,
                Maximum = 200,
                Value = 48
            };

            numFontSize.ValueChanged += (s, e) => updatePreview();

            Label lblFontColour = new Label()
            {
                Text = "Font colour:",
                Left = 10,
                Top = 55,
                Width = 75
            };

            txtFontColour = new TextBox()
            {
                Left = 90,
                Top = 51,
                Width = 140,
            };

            btnTextColour = new Button()
            {
                Text = "Set Colour...",
                Left = 245,
                Top = 51,
                Width = 85
            };

            btnTextColour.Click += pickTextColour;

            GroupBox fontGroup = new GroupBox()
            {
                Text = "Font Settings",
                Left = 20,
                Top = 110,
                Width = 340,
                Height = 85
            };

            fontGroup.Controls.Add(lblFont);
            fontGroup.Controls.Add(txtFont);
            fontGroup.Controls.Add(btnFont);
            fontGroup.Controls.Add(lblFontColour);
            fontGroup.Controls.Add(txtFontColour);
            fontGroup.Controls.Add(btnTextColour);

            // ======== Speed ========

            Label lblSlow = new Label()
            {
                Text = "Slow",
                Left = 15,
                Top = 18,
                Width = 35
            };

            Label lblFast = new Label()
            {
                Text = "Fast",
                Left = 155,
                Top = 18,
                Width = 30
            };

            slider = new TrackBar()
            {
                Left = 10,
                Top = 40,
                Width = 175,
                Height = 10,
                Minimum = 1,
                Maximum = 17,
                TickFrequency = 4,
                Value = 5
            };

            slider.ValueChanged += updateSpeed;

            GroupBox speedGroup = new GroupBox()
            {
                Text = "Speed Settings",
                Left = 370,
                Top = 110,
                Width = 195,
                Height = 85
            };

            speedGroup.Controls.Add(lblSlow);
            speedGroup.Controls.Add(lblFast);
            speedGroup.Controls.Add(slider);

            // ======== Background ========

            Label lblBgColour = new Label()
            {
                Text = "Background colour:",
                Left = 10,
                Top = 25,
                Width = 120
            };

            txtBgColour = new TextBox()
            {
                Left = 130,
                Top = 21,
                Width = 310,
            };

            txtBgColour.TextChanged += (s, e) =>
            {
                if (txtBgColour.Text.Length > 0 && txtBgImage != null)
                {
                    txtBgImage.Text = "";
                }

                if (txtBgColour != null && txtBgColour.Text.Length > 0)
                {
                    updatePreview();
                }
            };

            btnBgColour = new Button()
            {
                Text = "Set Colour...",
                Left = 450,
                Top = 21,
                Width = 85
            };

            btnBgColour.Click += pickBgColour;

            Label lblBgImage = new Label()
            {
                Text = "Background image:",
                Left = 10,
                Top = 55,
                Width = 120
            };

            txtBgImage = new TextBox()
            {
                Left = 130,
                Top = 51,
                Width = 310,
            };

            txtBgImage.TextChanged += (s, e) =>
            {
                if (txtBgImage.Text.Length > 0 && txtBgColour != null)
                {
                    txtBgColour.Text = "";
                }


                if (txtBgImage != null && txtBgImage.Text.Length > 0)
                {
                    updatePreview();
                }
            };

            btnBgImage = new Button()
            {
                Text = "Browse...",
                Left = 450,
                Top = 51,
                Width = 85
            };

            btnBgImage.Tag = FileBrowseType.BG_IMAGE_FILE;
            btnBgImage.Click += browseFile;

            GroupBox backgroundGroup = new GroupBox()
            {
                Text = "Background settings",
                Left = 20,
                Top = 210,
                Width = WIDTH - 55,
                Height = 85
            };

            backgroundGroup.Controls.Add(lblBgColour);
            backgroundGroup.Controls.Add(txtBgColour);
            backgroundGroup.Controls.Add(btnBgColour);
            backgroundGroup.Controls.Add(lblBgImage);
            backgroundGroup.Controls.Add(txtBgImage);
            backgroundGroup.Controls.Add(btnBgImage);


            // ======== Preview ========
            previewPanel = new Panel()
            {
                Left = 10,
                Top = 25,
                Width = PREVIEW_WIDTH,
                Height = 100,
                BorderStyle = BorderStyle.FixedSingle
            };

            previewLabel = new Label()
            {
                Text = "Preview Text",
                AutoSize = true
            };

            previewPanel.Controls.Add(previewLabel);

            GroupBox previewGroup = new GroupBox()
            {
                Text = "Preview",
                Left = 20,
                Top = 310,
                Width = WIDTH - 55,
                Height = 135
            };

            previewGroup.Controls.Add(previewPanel);

            // ======== OK / Cancel ========
            btnOk = new Button()
            {
                Text = "OK",
                Width = 70
            };

            btnOk.Left = WIDTH - this.btnOk.Width - 35;
            btnOk.Top = HEIGHT - this.btnOk.Height - 50;
            btnOk.Click += saveSettings;


            btnCancel = new Button()
            {
                Text = "Cancel",
                Width = 70
            };

            btnCancel.Left = WIDTH - this.btnCancel.Width - 110;
            btnCancel.Top = HEIGHT - this.btnCancel.Height - 50;
            btnCancel.Click += closeDialog;

            Controls.AddRange(new Control[]
            {
                textGroup,
                fontGroup, speedGroup,
                backgroundGroup,
                previewGroup,
                btnCancel, btnOk
            });

            loadExistingSettings();
            updatePreview();

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 2;
            timer.Tick += tick;
            timer.Start();
        }



        // tick for preview animation
        private void tick(object? sender, EventArgs e)
        {
            String text = getText();
            int textWidth = TextRenderer.MeasureText(text, selectedFont).Width;
            xPreview -= slider.Value;

            if (xPreview < 0 - textWidth)
            {
                xPreview = previewPanel.Width;
            }

            previewLabel.Left = xPreview;
            Invalidate();
        }

        void loadExistingSettings()
        {
            settings = Settings.load();

            txtFile.Text = settings.textFile;
            if (txtFile.Text.Length == 0)
            {
                txtText.Text = settings.text;
            }

            textColour = settings.getTextColor();
            bgColour = settings.getBackgroundColor();
            bgImagePath = settings.backgroundImagePath;
            selectedFont = new Font(settings.fontName, settings.fontSize);

            numFontSize.Value = settings.fontSize;
            txtFont.Text = selectedFont.Name;
            txtFontColour.Text = ColorTranslator.ToHtml(textColour);

            if (bgImagePath.Length > 0)
            {
                txtBgImage.Text = settings.backgroundImagePath;
                bgImagePath = txtBgImage.Text;
            }
            else
            {
                txtBgColour.Text = ColorTranslator.ToHtml(bgColour);
            }

            slider.Value = settings.speed;
        }

        void browseFile(object? sender, EventArgs e)
        {

            using (OpenFileDialog dialog = new OpenFileDialog())
            {

                FileBrowseType fileType = FileBrowseType.NONE;

                if (sender is Button btn)
                {
                    if (btn.Tag is FileBrowseType.TEXT_FILE)
                    {
                        fileType = FileBrowseType.TEXT_FILE;
                        dialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                    }
                    else if (btn.Tag is FileBrowseType.BG_IMAGE_FILE)
                    {
                        fileType = FileBrowseType.BG_IMAGE_FILE;
                        dialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.bmp;*.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files (*.*)|*.*";
                    }
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (fileType is FileBrowseType.TEXT_FILE)
                    {
                        txtFile.Text = dialog.FileName;
                        txtText.Text = "";
                    }
                    else if (fileType is FileBrowseType.BG_IMAGE_FILE)
                    {
                        txtBgImage.Text = dialog.FileName;
                        bgImagePath = txtBgImage.Text;
                        txtBgColour.Text = "";
                    }

                    updatePreview();
                }
            }
        }

        void chooseFont(object sender, EventArgs e)
        {
            using (FontDialog dialog = new FontDialog())
            {
                dialog.Font = selectedFont;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    selectedFont = dialog.Font;
                    numFontSize.Value = (decimal)dialog.Font.Size;
                    updatePreview();
                    txtFont.Text = selectedFont.Name;
                }
            }
        }

        void pickTextColour(object sender, EventArgs e)
        {
            using (ColorDialog dialog = new ColorDialog())
            {
                dialog.Color = textColour;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textColour = dialog.Color;
                    txtFontColour.Text = ColorTranslator.ToHtml(textColour);
                    updatePreview();
                }
            }
        }

        void updateSpeed(object? sender, EventArgs e)
        {
            updatePreview();
        }

        void pickBgColour(object? sender, EventArgs e)
        {
            using (ColorDialog dialog = new ColorDialog())
            {
                dialog.Color = bgColour;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    bgColour = dialog.Color;
                    txtBgColour.Text = ColorTranslator.ToHtml(bgColour);
                    txtBgImage.Text = "";
                    bgImagePath = txtBgImage.Text;
                    updatePreview();
                }
            }
        }

        void updatePreview()
        {
            if (selectedFont == null) return;

            selectedFont = new Font(selectedFont.FontFamily, (float)numFontSize.Value);

            if (txtBgImage.Text != null && File.Exists(txtBgImage.Text))
            {
                previewLabel.BackColor = Color.Transparent;
                previewPanel.BackgroundImage = Image.FromFile(txtBgImage.Text);
            }
            else
            {
                try
                {
                    Color previewColour = ColorTranslator.FromHtml(txtBgColour.Text);
                    previewPanel.BackgroundImage = null;
                    previewPanel.BackColor = previewColour;
                }
                catch
                {
                    // do nothing, invalid colour code
                }
            }

            previewLabel.ForeColor = textColour;
            previewLabel.Font = selectedFont;

            previewLabel.Left = xPreview;
            previewLabel.Top = (previewPanel.Height - previewLabel.Height) / 2;

            previewLabel.Text = getText();
        }

        private String getText()
        {
            String ret = "Preview Text";

            if (txtFile.Text.Length > 0 && File.Exists(txtFile.Text))
            {
                String[] lines = File.ReadAllLines(txtFile.Text);
                if (lines.Length > 0)
                {
                    ret =lines[0];
                }
            }
            else
            {
                if (txtText.Text.Length > 0)
                {
                    ret = txtText.Text;
                }
            }

            return ret;
        }

        void saveSettings(object? sender, EventArgs e)
        {
            var settings = new Settings
            {
                textFile = txtFile.Text,
                text = txtText.Text,
                fontName = selectedFont.FontFamily.Name,
                fontSize = (int)numFontSize.Value,
                textColor = ColorTranslator.ToHtml(textColour),
                backgroundColor = ColorTranslator.ToHtml(bgColour),
                backgroundImagePath = bgImagePath,
                speed = slider.Value
            };

            string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            string dir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "marquee3"
            );
            string file = Path.Combine(dir, "settings.json");

            Directory.CreateDirectory(dir);
            File.WriteAllText(file, json);
            this.closeDialog(this, EventArgs.Empty);
        }

        void closeDialog(object? sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}