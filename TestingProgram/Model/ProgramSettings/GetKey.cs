using System;
using System.Collections.Generic;
using System.Management;
using System.Text;

namespace DeZ.Settings
{
          static class GetKey
          {
                    private static byte[] pcKey;
                    public static byte[] KeyDef()
                    => new byte[] { 45, 26, 44, 12, 41, 45, 26, 11 };
                    public static byte[] KeyPc()
                    {
                              if (pcKey == null)
                              {
                                        byte[] key = new byte[8];
                                        byte[][] k0 = new byte[8][];
                                        k0[0] = Encoding.ASCII.GetBytes(GetHardwareInfo("Win32_Processor", "Name")[0]);
                                        k0[1] = Encoding.ASCII.GetBytes(GetHardwareInfo("Win32_Processor", "Manufacturer")[0]);
                                        k0[2] = Encoding.ASCII.GetBytes(GetHardwareInfo("Win32_Processor", "Description")[0]);
                                        k0[3] = Encoding.ASCII.GetBytes(GetHardwareInfo("Win32_VideoController", "Name")[0]);
                                        k0[4] = Encoding.ASCII.GetBytes(GetHardwareInfo("Win32_VideoController", "VideoProcessor")[0]);
                                        k0[5] = Encoding.ASCII.GetBytes(GetHardwareInfo("Win32_VideoController", "AdapterRAM")[0]);
                                        k0[6] = Encoding.ASCII.GetBytes(GetHardwareInfo("Win32_Processor", "Manufacturer")[0]);
                                        k0[7] = Encoding.ASCII.GetBytes(GetHardwareInfo("Win32_Processor", "Name")[0]);
                                        for (int i = 0; i < 8; i++)
                                        {
                                                  for (int j = 0; j < k0[i].Length; j++)
                                                            key[i] += k0[i][j];
                                        }
                                        pcKey = key;
                              }

                              return pcKey;
                    }
                    private static List<string> GetHardwareInfo(string WIN32_Class, string ClassItemField)
                    {
                              List<string> result = new List<string>();
                              ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM " + WIN32_Class);

                              try
                              {
                                        foreach (ManagementObject obj in searcher.Get())
                                                  result.Add(obj[ClassItemField].ToString().Trim());
                              }
                              catch (Exception ex)
                              {
                                        Console.WriteLine(ex.Message);
                              }
                              return result;
                    }
          }
}
