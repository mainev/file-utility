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
    public class CopyToTempDirectoryCommand : IFileCommand
    {
        public Stopwatch Stopwatch { get; set; } = new Stopwatch();
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public short Order { get; set; }
        public FileActivity FileActivity { get; set; }

        private string _message { get; set; }
        private bool _error { get; set; } = false;

        private readonly string _tempDirectory;

        public CopyToTempDirectoryCommand(FileActivity fileActivity, string tempDirectory)
        {
            FileActivity = fileActivity;
            _tempDirectory = tempDirectory;
        }


        public void AddTempFile()
        {
            FileActivity._TempFiles.Add(FileActivity._CurrentFile);
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

                if (File.Exists(FileActivity._NewFile))
                    File.Delete(FileActivity._NewFile);


                if (!Directory.Exists(Path.GetDirectoryName(FileActivity._NewFile)))
                    Directory.CreateDirectory(Path.GetDirectoryName(FileActivity._NewFile));

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

        public void SetCurrentFile()
        {
            FileActivity._CurrentFile = FileActivity._NewFile;
        }

        public void SetNewFile()
        {
            FileActivity._NewFile = Path.Combine(_tempDirectory, Path.GetFileName(FileActivity._PrevFile));
        }

        public void SetPrevFile()
        {
            FileActivity._PrevFile = FileActivity.SourceFile;
        }

        public void StartStopwatch()
        {
            DateStart = DateTime.Now;
            Stopwatch.Start();
        }


        public void SetActivityLog()
        {
            FileActivity.FileActivityLogs.Add(new FileActivityLog()
            {
                LogMessage = _message,
                Error = _error,
                DateStart = DateStart,
                DateEnd = DateEnd,
                ActivityType = "COPY_TO_TEMP",
                PrevFile = FileActivity._PrevFile,
                NewFile = FileActivity._NewFile,
                Duration = Stopwatch.Elapsed.TotalSeconds,
                Order = Order
            });
        }
    }
}
