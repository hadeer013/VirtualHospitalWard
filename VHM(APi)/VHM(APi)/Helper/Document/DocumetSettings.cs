using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace VHM_APi_.Document
{
    public class DocumetSettings
    {
        public static string UploadFile(IFormFile file,string FolderName)
        {
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", FolderName);

            
            var fileName = $"{ Guid.NewGuid() }{ Path.GetFileName(file.FileName)}";
            
            var filePath = Path.Combine(FolderPath, fileName);
            
            using var fs = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fs);

            return Path.Combine("files","Imgs",fileName);

        }
        public static void DeleteFile(string fileName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",fileName);
            if (File.Exists(filePath))
                File.Delete(filePath);

        }
    }
}
