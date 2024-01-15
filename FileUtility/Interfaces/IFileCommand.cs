using FileUtility.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUtility.Interfaces
{
    public interface IFileCommand
    {
        Stopwatch Stopwatch { get; set; }
        DateTime DateStart { get; set; }
        DateTime DateEnd { get; set; }
        short Order { get; set; }
        FileActivity FileActivity { get; set; }

        void StartStopwatch();
        void SetPrevFile();
        void SetNewFile();
        void Execute();
        void EndStopwatch();
        void SetActivityLog();
        void SetCurrentFile();
        void AddTempFile();
        void CompleteTransaction();
        
    }
}
