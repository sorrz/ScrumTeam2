using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopGeneral.Data;

namespace ShopGeneral.Services
{
    public class FileOutputService : IFileOutputService
    {

        public void FileOutput(string output, string folderName, string fileName)
        {
            var folderPath = Path.Combine($"outfiles\\{folderName}\\");
            var fullFilePath = Path.Combine(folderPath, $"{fileName}" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
            Directory.CreateDirectory(folderPath);

            using (StreamWriter streamWriter = new StreamWriter(fullFilePath))
            {
                streamWriter.Write(output);
            }
        }
    }
}
