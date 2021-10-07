using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace aspnetcore_aad.Models
{
    public class IndexViewModel
    {
        public string KVSecret { get; set; }
        public string TotalAvailableMemory { get; set; }
        public string MemoryUsage { get; set; }
        public string MemoryLimit { get; set; }
        public string HostName { get; set; }
        public bool cGroup { get; set; }
        public string CpuUsage { get; set; }
        public IPAddress[] IpList { get; set; }
    }
}
