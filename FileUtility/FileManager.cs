using FileUtility.Commands;
using FileUtility.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUtility
{
    public class FileManager
    {
        public List<FileActivity> FileActivities;

        public FileManager()
        {
        }

        public List<FileActivity> EnumerateFiles(string sourceDirectory, string[] filterArray)
        {

            var enumeratedFiles = new List<FileActivity>();


            if (Directory.Exists(sourceDirectory))
            {

                foreach (var fileFilter in filterArray)
                {
                    var files = Directory.EnumerateFiles(sourceDirectory, fileFilter, SearchOption.AllDirectories);

                    foreach (var file in files)
                    {
                        var fileInfo = new FileInfo(file);

                        enumeratedFiles.Add(new FileActivity(file, sourceDirectory)
                        {
                            OrigLocation = fileInfo.DirectoryName,
                            OrigFilename = fileInfo.Name,
                            OrigFilesize = fileInfo.Length,
                        });

                    }
                }
            }


            return enumeratedFiles;

        }
    }
}
