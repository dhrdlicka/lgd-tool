using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace LgdTool
{
    public class LogoData
    {
        public string[] ProductDescription { get; set; } = new string[2];
        public string[] Copyright { get; set; } = new string[6];
        public bool Merging { get; set; }
        public byte Height { get; private set; }
        public byte Width { get; private set; }

        byte[] scanLineData;

        public LogoData(byte[] lgd)
        {
            using (MemoryStream stream = new MemoryStream(lgd, 0, lgd.Length))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                ProductDescription[0] = ReadNullTerminatedString(reader);
                ProductDescription[1] = ReadNullTerminatedString(reader);

                Copyright[0] = ReadNullTerminatedString(reader);
                Copyright[1] = ReadNullTerminatedString(reader);
                Copyright[2] = ReadNullTerminatedString(reader);
                Copyright[3] = ReadNullTerminatedString(reader);
                Copyright[4] = ReadNullTerminatedString(reader);
                Copyright[5] = ReadNullTerminatedString(reader);

                Merging = reader.ReadByte() > 0;

                Height = reader.ReadByte();
                Width = reader.ReadByte();

                scanLineData = reader.ReadBytes(Height * Width);
            }
        }

        public Bitmap Logo
        {
            get
            {
                return ScanLinesToBitmap();
            }

            set
            {
                scanLineData = BitmapToScanLines(value);
                Height = (byte)value.Height;
                Width = (byte)(value.Width / 8);
            }
        }

        byte[] BitmapToScanLines(Bitmap bmp)
        {
            if (bmp.Width > 2047)
                throw new InvalidDataException();

            if (bmp.Width % 8 > 0)
                throw new InvalidDataException();

            if (bmp.Height > 255)
                throw new InvalidDataException();

            byte[] bytes = new byte[bmp.Width / 8 * bmp.Height];

            int pos;
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x += 8)
                {
                    pos = ((y % 2) * (bmp.Height / 2) + (y / 2)) * bmp.Width / 8 + x / 8;
                    for (int i = 0; i < 8; i++)
                    {
                        if (bmp.GetPixel(x + 7 - i, y).ToArgb() == Color.White.ToArgb())
                        {
                            bytes[pos] = (byte)(bytes[pos] | 1 << i);
                        }
                        else if (bmp.GetPixel(x + 7 - i, y).ToArgb() != Color.Black.ToArgb())
                        {
                            throw new InvalidDataException();
                        }
                    }
                }
            }

            return bytes;
        }

        string ReadNullTerminatedString(BinaryReader reader)
        {
            byte b;
            List<byte> list = new List<byte>();

            while((b = reader.ReadByte()) != 0)
            {
                list.Add(b);
            }

            return Encoding.ASCII.GetString(list.ToArray());
        }

        Bitmap ScanLinesToBitmap()
        {
            Bitmap bmp = new Bitmap(Width * 8, Height);

            int pos;
            byte data;
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width - 7; x += 8)
                {
                    pos = (y % 2) * (bmp.Height / 2) + (y / 2);
                    data = scanLineData[pos * bmp.Width / 8 + x / 8];
                    for (int i = 0; i < 8; i++)
                    {
                        if ((data & 1 << i) == 1 << i)
                            bmp.SetPixel(x + 7 - i, y, Color.White);
                        else
                            bmp.SetPixel(x + 7 - i, y, Color.Black);
                    }
                }
            }

            return bmp;
        }

        public byte[] ToByteArray()
        {
            using (MemoryStream stream = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(Encoding.ASCII.GetBytes(ProductDescription[0] + '\0'));
                writer.Write(Encoding.ASCII.GetBytes(ProductDescription[1] + '\0'));

                writer.Write(Encoding.ASCII.GetBytes(Copyright[0] + '\0'));
                writer.Write(Encoding.ASCII.GetBytes(Copyright[1] + '\0'));
                writer.Write(Encoding.ASCII.GetBytes(Copyright[2] + '\0'));
                writer.Write(Encoding.ASCII.GetBytes(Copyright[3] + '\0'));
                writer.Write(Encoding.ASCII.GetBytes(Copyright[4] + '\0'));
                writer.Write(Encoding.ASCII.GetBytes(Copyright[5] + '\0'));

                writer.Write(Merging ? (byte)1 : (byte)0);

                writer.Write(Height);
                writer.Write(Width);

                writer.Write(scanLineData);

                return stream.ToArray();
            }
        }
    }
}
