using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace LgdTool.Viewer
{
    public partial class MainForm : Form
    {
        LogoData lgd;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            switch (Environment.GetCommandLineArgs().Length)
            {
                case 1:
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        lgd = new LogoData(File.ReadAllBytes(ofd.FileName));
                        break;
                    }
                    else
                    {
                        Close();
                        return;
                    }
                case 2:
                    lgd = new LogoData(File.ReadAllBytes(Environment.GetCommandLineArgs()[1]));
                    break;
                default:
                    Close();
                    return;
            }

            BackColor = Color.Blue;
            Invalidate();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            if (lgd != null)
            {
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                using (Bitmap logo = lgd.Logo)
                {
                    ColorMap[] colorMap = new ColorMap[1];
                    colorMap[0] = new ColorMap();
                    colorMap[0].OldColor = Color.Black;
                    colorMap[0].NewColor = Color.Blue;

                    ImageAttributes attr = new ImageAttributes();
                    attr.SetRemapTable(colorMap);

                    e.Graphics.FillRectangle(Brushes.Blue, 0, 0, 640, 400);

                    Rectangle destRect = new Rectangle(320 - (logo.Width / 2), 104 - logo.Height, logo.Width, logo.Height * 2);
                    e.Graphics.DrawImage(logo, destRect, 0, 0, logo.Width, logo.Height, GraphicsUnit.Pixel, attr);

                    using (Font font = new Font("Lucida Console", 14, GraphicsUnit.Pixel))
                    {
                        SizeF size;

                        size = e.Graphics.MeasureString(lgd.ProductDescription[0], font);
                        e.Graphics.DrawString(lgd.ProductDescription[0], font, Brushes.White, 320 - size.Width / 2, 224);

                        size = e.Graphics.MeasureString(lgd.ProductDescription[1], font);
                        e.Graphics.DrawString(lgd.ProductDescription[1], font, Brushes.White, 320 - size.Width / 2, 240);

                        size = e.Graphics.MeasureString(lgd.Copyright[0], font);
                        e.Graphics.DrawString(lgd.Copyright[0], font, Brushes.White, 320 - size.Width / 2, 304);

                        size = e.Graphics.MeasureString(lgd.Copyright[1], font);
                        e.Graphics.DrawString(lgd.Copyright[1], font, Brushes.White, 320 - size.Width / 2, 320);

                        size = e.Graphics.MeasureString(lgd.Copyright[2], font);
                        e.Graphics.DrawString(lgd.Copyright[2], font, Brushes.White, 320 - size.Width / 2, 336);

                        size = e.Graphics.MeasureString(lgd.Copyright[3], font);
                        e.Graphics.DrawString(lgd.Copyright[3], font, Brushes.White, 320 - size.Width / 2, 352);

                        size = e.Graphics.MeasureString(lgd.Copyright[4], font);
                        e.Graphics.DrawString(lgd.Copyright[4], font, Brushes.White, 320 - size.Width / 2, 368);

                        size = e.Graphics.MeasureString(lgd.Copyright[5], font);
                        e.Graphics.DrawString(lgd.Copyright[5], font, Brushes.White, 320 - size.Width / 2, 384);
                    }
                }
            }
        }
    }
}
