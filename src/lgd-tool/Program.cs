using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LgdTool
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1) return;
            FileInfo file = new FileInfo(args[0]);
            
            if(file.Extension.ToLower() == ".lgd")
            {
                LogoData lgd = new LogoData(File.ReadAllBytes(file.FullName));

                lgd.Logo.Save(file.FullName + ".bmp");

                using (StreamWriter writer = new StreamWriter(file.FullName + ".txt"))
                {
                    writer.WriteLine(lgd.ProductDescription[0]);
                    writer.WriteLine(lgd.ProductDescription[1]);
                    writer.WriteLine(lgd.Copyright[0]);
                    writer.WriteLine(lgd.Copyright[1]);
                    writer.WriteLine(lgd.Copyright[2]);
                    writer.WriteLine(lgd.Copyright[3]);
                    writer.WriteLine(lgd.Copyright[4]);
                    writer.WriteLine(lgd.Copyright[5]);
                }
            }
            else
            {
                Console.WriteLine("usage: lgd-tool <logo data file>");
            }
        }
    }
}
