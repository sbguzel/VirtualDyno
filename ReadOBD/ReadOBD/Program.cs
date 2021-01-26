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
            int[] speed_time = new int[5000];
            string hex;
            string time_string;
            Int32 time_ms = 0, time_ms_diff = 0, time_offset = 0, time_ms_old = 0;
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
                                    
                                    time_string = lines[i + 1][0].ToString() + lines[i + 1][1].ToString() + lines[i + 1][3].ToString() + lines[i + 1][4].ToString() + lines[i + 1][5].ToString();
                                    time_ms = Int32.Parse(time_string);

                                    if(j == 0)
                                    {
                                        speed_time[j] = 0;
                                    }
                                    else
                                    {
                                        time_ms_diff = time_ms - time_ms_old;
                                        if(time_ms_diff < 0)
                                        {
                                            time_ms_diff += 60000;
                                        }
                                        speed_time[j] = time_ms_diff;
                                    }

                                    Console.WriteLine(speed_time[j] + " " +speed_kph[j]);

                                    time_ms_old = time_ms;
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
