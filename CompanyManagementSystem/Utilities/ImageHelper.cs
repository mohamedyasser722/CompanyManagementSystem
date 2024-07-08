namespace CompanyManagementSystem.PL.Utilities
{
    public class ImageHelper
    {


        public static string UploadFile(IFormFile file, string folderName)
        {
            try
            {
                // Get the folder path
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", folderName);

                // Ensure the directory exists
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Get the file name and make it unique
                string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;

                // Combine the folder path and file name
                string filePath = Path.Combine(folderPath, fileName);

                // Save file as stream
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // Return the relative path
                return fileName;
            }
            catch (Exception ex)
            {
                // Handle the error (log it, rethrow it, return a special value, etc.) 
                throw new InvalidOperationException("File upload failed", ex);
            }
        }


        public static void DeleteFile(string FilePath , string FolderName)
        {
            
            try
            {
                // Get the file path
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", FolderName, FilePath);

                // Check if the file exists
                if (File.Exists(fullPath))
                {
                    // Delete the file
                    File.Delete(fullPath);
                }
            }
            catch (Exception ex)
            {
                // Handle the error (log it, rethrow it, return a special value, etc.) 
                throw new InvalidOperationException("File deletion failed", ex);
            }

        }

        //public static IFormFile GetImageAsFormFile(string imagePath)
        //{
        //    string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", "Images");
        //    string FullImagePath = Path.Combine(folderPath, imagePath);
        //    if (!File.Exists(FullImagePath))
        //    {
        //        throw new FileNotFoundException("The specified file does not exist.", FullImagePath);
        //    }

        //    byte[] fileBytes = File.ReadAllBytes(FullImagePath);
        //    var stream = new MemoryStream(fileBytes);
        //    var fileName = Path.GetFileName(FullImagePath);
        //    var file = new FormFile(stream, 0, stream.Length, null, fileName)
        //    {
        //        Headers = new HeaderDictionary(),
        //        ContentType = "image/jpeg" // Or get the actual content type if needed
        //    };

        //    return file;
        //}
    }
}
