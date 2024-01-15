using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUtility.Models
{
    public class FileActivityLog
    {
        public string ActivityType { get; set; }
        public double Duration { get; set; }
        public string LogMessage { get; set; }
        public bool Error { get; set; }
        public string PrevFile { get; set; }
        public string NewFile { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public short Order { get; set; }
    }
}
