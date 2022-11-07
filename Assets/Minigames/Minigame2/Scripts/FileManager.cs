using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TripasDeGato
{
    class FileManager
    {
        public static async void WriteRawDataToFileAsync(string fileName, string rawData)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                await writer.WriteAsync(rawData);
            }
        }

        public static async Task<string> ReadRawDataFromFileAsync(string fileName)
        {
            string content = "";
            try
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    content += await reader.ReadToEndAsync();
                }
            }
            catch
            {
                Debug.Log("Could not open file.");
            }

            return content;
        }
    }
}