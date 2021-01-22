using System;
using System.IO;
namespace ReadOBD
{
    class Program
    {
        static void Main(string[] args)
        {
            String[] lines;
            int[] speed_kph = new int[5000];
            string hex;

            lines = System.IO.File.ReadAllLines(@"D:\saidburakguzel\Projects\Practice\C#\OBD2_read\ReadOBD\OBD_Log.txt");
            //System.Console.WriteLine("Contents of WriteLines2.txt = ");
            //foreach (string line in lines)
            //{
            //    // Use a tab to indent each line of the file.
            //    Console.WriteLine("\t" + line);
            //}

            //9 10 11 12

            Console.WriteLine(lines.Length.ToString());

            string speed_code = "010D";
            int j = 0;

            for (int i = 200; i < lines.Length; i++)
            {
                if (!string.IsNullOrEmpty(lines[i]))
                {
                    int length = lines[i].Length;
                    if(length>12)
                    {
                        string temp = lines[i][9].ToString() + lines[i][10].ToString() + lines[i][11].ToString() + lines[i][12].ToString();
                        if(String.Compare(speed_code, temp) == 0)
                        {
                            if(!string.IsNullOrEmpty(lines[i+1]))
                            {
                                length = lines[i+1].Length;
                                if (length>23)
                                {
                                    hex = lines[i + 1][22].ToString() + lines[i + 1][23].ToString();
                                    //Console.WriteLine("Hex:" + hex);
                                    speed_kph[j] = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
                                    Console.WriteLine(speed_kph[j]);
                                    j += 1;
                                }
                                
                            }
                            
                        }

                    }

                }
                
            }

        }

    }
}
