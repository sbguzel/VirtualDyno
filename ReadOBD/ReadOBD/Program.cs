using System;
using System.IO;
namespace ReadOBD
{
    class Program
    {
        static void Main(string[] args)
        {
            String[] lines;
            lines = System.IO.File.ReadAllLines(@"C:\Users\saidb\Documents\MATLAB\VirtualDyno\Trax_15_P_dry_0_50_up_down\20210719151157.txt");

            Read_obd(381, "010D", lines);   //Speed                          [km/h]
            Read_obd(381, "010C", lines);   //rpm                            [rev/min]
            Read_obd(381, "0123", lines);   //Fuel Rail Pressure             [kPa] (gauge)
            Read_obd(381, "012C", lines);   //EGR                            [percent] (target)
            Read_obd(381, "0105", lines);   //Coolent Temperature            [celcius]
            Read_obd(381, "0133", lines);   //Barometric Pressure            [kPa] (Absolute)
            Read_obd(381, "010B", lines);   //Intake Manifold Pressure       [kPa] (Absolute)
            Read_obd(381, "010F", lines);   //Intake Air Temperature         [celcius]
            Read_obd(381, "0110", lines);   //Mass air flow                  [grams/sec]
            Read_obd(381, "0104", lines);   //Calculated Engine Load         [percent]
            Read_obd(381, "0149", lines);   //Accelerator Pedal Position D   [percent]
            Read_obd(381, "014A", lines);   //Accelerator Pedal Position E   [percent]

        }

        public static void Read_obd(int start_row, string code, string[] lines)
        {
            int j = 0;
            int bytes = 0;
            int offset = 0;
            float A_coefficient = 0, B_coefficient = 0;
            string hexA, hexB;
            int A = 0, B = 0;
            string time_string;
            Int32 time_ms = 0, time_ms_diff = 0, time_ms_old = 0;
            float[] data = new float[5000];
            int[] data_time = new int[5000];
            string indicator = "";
            bool separate = true;

            switch (code)
            {
                case "010C":
                    bytes = 2;
                    A_coefficient = 64;
                    B_coefficient = 0.25f;
                    offset = 0;
                    indicator = "RPM,[rev/min]";
                    break;

                case "010D":
                    bytes = 1;
                    A_coefficient = 1;
                    B_coefficient = 0;
                    offset = 0;
                    indicator = "Speed,[km/h]";
                    break;

                case "0123":
                    bytes = 2;
                    A_coefficient = 2560;
                    B_coefficient = 10;
                    offset = 0;
                    indicator = "FuelRailPressure,[kPa](gauge)";
                    break;

                case "012C":
                    bytes = 1;
                    A_coefficient = 0.392157f;
                    B_coefficient = 0;
                    offset = 0;
                    indicator = "EGR,[percent](target)";
                    break;

                case "0105":
                    bytes = 1;
                    A_coefficient = 1;
                    B_coefficient = 0;
                    offset = -40;
                    indicator = "CoolentTemperature,[celcius]";
                    break;

                case "0133":
                    bytes = 1;
                    A_coefficient = 1;
                    B_coefficient = 0;
                    offset = 0;
                    indicator = "BarometricPressure,[kPa](Absolute)";
                    break;

                case "010B":
                    bytes = 1;
                    A_coefficient = 1;
                    B_coefficient = 0;
                    offset = 0;
                    indicator = "IntakeManifoldPressure,[kPa](Absolute)";
                    break;

                case "010F":
                    bytes = 1;
                    A_coefficient = 1;
                    B_coefficient = 0;
                    offset = -40;
                    indicator = "IntakeAirTemperature,[celcius]";
                    break;

                case "0110":
                    bytes = 2;
                    A_coefficient = 2.56f;
                    B_coefficient = 0.01f;
                    offset = 0;
                    indicator = "MassAirFlow,[grams/sec]";
                    break;

                case "0104":
                    bytes = 1;
                    A_coefficient = 0.392157f;
                    B_coefficient = 0;
                    offset = 0;
                    indicator = "CalculatedEngineLoad,[percent]";
                    break;

                case "0149":
                    bytes = 1;
                    A_coefficient = 0.392157f;
                    B_coefficient = 0;
                    offset = 0;
                    indicator = "AcceleratorPedalPositionD,[percent]";
                    break;

                case "014A":
                    bytes = 1;
                    A_coefficient = 0.392157f;
                    B_coefficient = 0;
                    offset = 0;
                    indicator = "AcceleratorPedalPositionE,[percent]";
                    break;

                default:
                    break;
            }

            for (int i = start_row; i < lines.Length; i++)
            {
                if (!string.IsNullOrEmpty(lines[i]))
                {
                    int length = lines[i].Length;
                    if (length > 12)
                    {
                        string temp = lines[i][9].ToString() + lines[i][10].ToString() + lines[i][11].ToString() + lines[i][12].ToString();
                        if (String.Compare(code, temp) == 0)
                        {
                            if (!string.IsNullOrEmpty(lines[i + 1]))
                            {
                                length = lines[i + 1].Length;
                                if (length >= 21 + bytes * 2) 
                                {
                                    if(bytes == 1)
                                    {
                                        hexA = lines[i + 1][22].ToString() + lines[i + 1][23].ToString();
                                        A = int.Parse(hexA, System.Globalization.NumberStyles.HexNumber);
                                    }
                                    else if(bytes == 2)
                                    {
                                        hexA = lines[i + 1][22].ToString() + lines[i + 1][23].ToString();
                                        hexB = lines[i + 1][25].ToString() + lines[i + 1][26].ToString();
                                        A = int.Parse(hexA, System.Globalization.NumberStyles.HexNumber);
                                        B = int.Parse(hexB, System.Globalization.NumberStyles.HexNumber);
                                    }
                                    
                                    data[j] = (A * A_coefficient + B * B_coefficient + offset);

                                    time_string = lines[i + 1][0].ToString() + lines[i + 1][1].ToString() + lines[i + 1][3].ToString() + lines[i + 1][4].ToString() + lines[i + 1][5].ToString();
                                    time_ms = Int32.Parse(time_string);

                                    if (j == 0)
                                    {
                                        data_time[j] = time_ms;
                                    }
                                    else
                                    {
                                        time_ms_diff = time_ms - time_ms_old;
                                        if (time_ms_diff < 0)
                                        {
                                            time_ms_diff += 60000;
                                        }
                                        data_time[j] = data_time[j - 1] + time_ms_diff;

                                    }

                                    string docPath = @"C:\Users\saidb\Documents\MATLAB\VirtualDyno\Trax_15_P_dry_0_50_up_down\CleanData.txt";
                                    if (!File.Exists(docPath))
                                    {
                                        if (separate)
                                        {
                                            File.WriteAllText(docPath, "-1,-1" + "\n");
                                            separate = false;
                                        }
                                        File.AppendAllText(docPath, data_time[j] + "," + data[j] + "\n");
                                        Console.WriteLine("its OK");
                                    }
                                    else
                                    {
                                        if (separate)
                                        {
                                            File.AppendAllText(docPath, "-1,-1" + "\n");
                                            separate = false;
                                        }
                                        File.AppendAllText(docPath, data_time[j] + "," + data[j] + "\n");
                                    }

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
