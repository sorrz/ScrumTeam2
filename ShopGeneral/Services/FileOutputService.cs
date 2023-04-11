namespace ShopGeneral.Services
{
    public class FileOutputService : IFileOutputService
    {

        public void FileOutput(string output, string folderName, string fileName, string fileEnding)
        {
            var folderPath = Path.Combine($"outfiles\\{folderName}\\");
            var fullFilePath = Path.Combine(folderPath, $"{fileName}" + DateTime.Now.ToString("yyyyMMdd") + $"{fileEnding}");
            Directory.CreateDirectory(folderPath);

            using (StreamWriter streamWriter = new StreamWriter(fullFilePath))
            {
                streamWriter.Write(output);
            }
        }
    }
}
