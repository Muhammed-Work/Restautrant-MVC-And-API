namespace RestaurantGorRahsa.Helper
{
    public class toolsHelper
    {
        private static readonly HashSet<string> AllowedVideoExtensions = new HashSet<string>
        {
            ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".webp", // Image extensions
        };

        public static async Task<String> UploadImage(List<IFormFile> lstFiles, string FolderName)
        {
            foreach (IFormFile file in lstFiles)
            {
                if (file.Length > 0)
                {
                    string extension = Path.GetExtension(file.FileName).ToLower();
                    if (AllowedVideoExtensions.Contains(extension))
                    {
                        string imageName = Guid.NewGuid().ToString() + DateTime.Now.Year + extension;
                        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Uploads", FolderName);

                        try
                        {
                            // Ensure the folder path is correct and exists
                            Directory.CreateDirectory(folderPath); // It will create the folder if it doesn't exist

                            // Build the full path for the file
                            var filePaths = Path.Combine(folderPath, imageName);

                            // Ensure the file path length does not exceed the OS limit
                            if (filePaths.Length > 260)
                            {
                                throw new IOException("File path is too long.");
                            }

                            // Save the file
                            using (var stream = System.IO.File.Create(filePaths))
                            {
                                await file.CopyToAsync(stream);
                            }

                            return imageName;
                        }
                        catch (IOException ioEx)
                        {
                            // Log the exception if there's an issue with IO operations (e.g., invalid folder path or permission issues)
                            Console.WriteLine($"IO Exception: {ioEx.Message}");
                            Console.WriteLine($"File path: {folderPath}");
                            return string.Empty;
                        }
                        catch (UnauthorizedAccessException unauthorizedEx)
                        {
                            // Log the unauthorized access exception
                            Console.WriteLine($"Unauthorized Access Exception: {unauthorizedEx.Message}");
                            return string.Empty;
                        }
                        catch (Exception ex)
                        {
                            // Log any other unexpected errors
                            Console.WriteLine($"General Error: {ex.Message}");
                            return string.Empty;
                        }
                    }
                }
            }

            return string.Empty;
        }
    }
}
