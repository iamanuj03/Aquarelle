using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Aquarelle.Classes
{
    public static class PATHS
    {
        private static readonly string pathToChange = @"C:\Users\Anuj\source\repos\Aquarelle\Aquarelle\";
        public static readonly string USERS_FOLDER_PATH = pathToChange + @"DATA\";
        public static readonly string USER_READ_FOLDER_PATH = pathToChange + @"DATA\ADMIN\FILES_READ\";
        public static readonly string EXTRACTED_INFO_FILE_PATH = pathToChange + @"DATA\ADMIN\EXTRACTED_DATA\ExtractedData.txt";
        public static readonly string LOGIN_INFO_FILE_PATH = pathToChange + @"DATA\ADMIN\USERS\users.txt";
    }
}