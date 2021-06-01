using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Aquarelle.Classes
{
    public class clsFile
    {
        public String fileId { get; set; }
        public String fileName { get; set; }
        public String filePath { get; set; }
        public String ownerId { get; set; }
        public Boolean isNewFile { get; set; }

        public clsFile(string name, string path, string ownerId, int index)
        {
            this.fileId = ownerId + "_" + index.ToString();
            this.fileName = name;
            this.filePath = path;
            this.isNewFile = true;
        }
    }
}