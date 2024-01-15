using FileUtility.Interfaces;
using FileUtility.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUtility.Commands
{
    public class CopyToTargetDirectoryCommand : IFileCommand
    {
        public Stopwatch Stopwatch { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public short Order { get; set; }
        public FileActivity FileActivity { get; set; }

        private string _message { get; set; }
        private bool _error { get; set; } = false;
        private string _newTargetDirectory;
        private readonly string _targetDirectory;
        private readonly bool _useFolderHierarchy;



        public CopyToTargetDirectoryCommand(FileActivity fileActivity, string targetDirectory, bool useFolderHierarchy)
        {

            Stopwatch = new Stopwatch();

            FileActivity = fileActivity;
            _targetDirectory = targetDirectory;
            _useFolderHierarchy = useFolderHierarchy;
        }

        public void AddTempFile()
        {
            //intentionally blank
        }

        public void CompleteTransaction()
        {
            FileActivity.ActivityComplete = true;
        }

        public void EndStopwatch()
        {
            DateEnd = DateTime.Now;
            Stopwatch.Stop();
        }

        public void Execute()
        {
            try
            {
                try {
                    if (!Directory.Exists(_newTargetDirectory))
                        Directory.CreateDirectory(_newTargetDirectory);

                    if (File.Exists(FileActivity._NewFile))
                        File.Delete(FileActivity._NewFile);
                }
                catch
                {
                    //ignore errors
                }
               

                File.Copy(FileActivity._PrevFile, FileActivity._NewFile);

                FileActivity.NewFilename = new FileInfo(FileActivity._NewFile).Name;
                FileActivity.NewFilesize = new FileInfo(FileActivity._NewFile).Length;
                FileActivity.NewLocation = new FileInfo(FileActivity._NewFile).DirectoryName;

                FileActivity.ActivityDate = DateTime.Now;

            }
            catch (Exception ex)
            {
                _message = ex.Message;
                _error = true;
            }
        }

        public void SetActivityLog()
        {
            FileActivity.FileActivityLogs.Add(new FileActivityLog()
            {
                LogMessage = _message,
                Error = _error,
                DateStart = DateStart,
                DateEnd = DateEnd,
                ActivityType = "COPY_TO_TARGET",
                PrevFile = FileActivity._PrevFile,
                NewFile = FileActivity._NewFile,
                Duration = Stopwatch.Elapsed.TotalSeconds,
                Order = Order
            }); 
        }

        public void SetCurrentFile()
        {
            FileActivity._CurrentFile = FileActivity._NewFile;
        }

        public void SetNewFile()
        {
            try
            {
                var directoryInfo = new DirectoryInfo(FileActivity.SourceFile);
                var sourceFileInfo = new FileInfo(FileActivity.SourceFile);

                //if the file exists on a subfolder, then include that folder on the targetDirectory if _useFolderHierarchy is enabled
                _newTargetDirectory = _useFolderHierarchy ? directoryInfo.Parent.FullName.Replace(FileActivity.SourcePath, _targetDirectory) : _targetDirectory;

                FileActivity._NewFile = Path.Combine(_newTargetDirectory, new FileInfo(FileActivity._PrevFile).Name);
            }
            catch (Exception ex)
            {
                _message = ex.Message;
                _error = true;
            }
        }

        public void SetPrevFile()
        {
            FileActivity._PrevFile = FileActivity._CurrentFile;
        }

        public void StartStopwatch()
        {
            DateStart = DateTime.Now;
            Stopwatch.Start();

        }
    }
}
