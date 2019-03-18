using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Load_forecast_using_ANN.Model
{
    class StaticFunctions
    {
        public static string PathDataCombine(string fileName)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "data", fileName);
        }

        public static bool ReplaceDataFromList(string filePath, List<string> dataList)
        {
            if (!File.Exists(filePath))
                return false;

            File.WriteAllText(filePath, string.Join(" ", dataList));
            return true;
        }
    }
}
