using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUtility.Models
{
    public class FileActivity
    {

        public string NewFilename { get; set; }
        public double NewFilesize { get; set; }
        public string NewLocation { get; set; }

        public string OrigFilename { get; set; }
        public double OrigFilesize { get; set; }
        public string OrigLocation { get; set; }

        public bool ActivityComplete { get; set; } = false;
        public DateTime ActivityDate { get; set; }

        public string _PrevFile { get; set; }
        public string _CurrentFile { get; set; }
        public string _NewFile { get; set; }

        public string SourceFile { get;  set; }
        public string SourcePath { get;  private set; }

        public List<FileActivityLog> FileActivityLogs { get; set; } = new List<FileActivityLog>();
        public List<string> _TempFiles = new List<string>();


        public FileActivity(string sourceFile, string sourcePath)
        {

            SourceFile = sourceFile;
            SourcePath = sourcePath;

            var fileInfo = new FileInfo(sourceFile);
            OrigFilename = fileInfo.Name;
            OrigFilesize = fileInfo.Length;
            OrigLocation = fileInfo.DirectoryName;
        }




    }
}
