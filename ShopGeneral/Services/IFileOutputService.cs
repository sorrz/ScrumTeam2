using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopGeneral.Data;

namespace ShopGeneral.Services
{
    public interface IFileOutputService
    {
        public void FileOutput(string output, string folderName, string fileName, string fileEnding);
    }
}
