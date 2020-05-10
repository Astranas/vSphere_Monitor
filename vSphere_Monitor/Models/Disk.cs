using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vSphere_Monitor.Models
{
    public class Disk
    {
        private string uuid;
        private bool isCacheDisk;
        private int capacityGB;
        private double usedPercent;
        private bool isSSD;

        private int freeRounded;
        private int capacityRounded;

        public Disk()
        {

        }

        public string Uuid { get => uuid; set => uuid = value; }
        public bool IsCacheDisk { get => isCacheDisk; set => isCacheDisk = value; }
        public int CapacityGB { get => capacityGB; set => capacityGB = value; }
        public double UsedPercent { get => usedPercent; set => usedPercent = value; }
        public bool IsSSD { get => isSSD; set => isSSD = value; }

        public int FreeRounded { get => freeRounded; set => freeRounded = value; }
        public int CapacityRounded { get => capacityRounded; set => capacityRounded = value; }
    }
}