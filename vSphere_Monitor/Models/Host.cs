using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vSphere_Monitor.Models
{
    public class Host
    {
        private string adress;
        private double memory_usage;
        private double memory_total;
        private double cpu_usage;
        private double cpu_total;
        private double storage_free;
        private double storage_total;
        private List<VM> vms;
        private List<Disk> localDisks;
        private List<Disk> vSANdisks;

        public Host()
        {

        }

        public string Adress { get => adress; set => adress = value; }
        public double Memory_usage { get => memory_usage; set => memory_usage = value; }
        public double Memory_total { get => memory_total; set => memory_total = value; }
        public double Cpu_usage { get => cpu_usage; set => cpu_usage = value; }
        public double Cpu_total { get => cpu_total; set => cpu_total = value; }
        public double Storage_free { get => storage_free; set => storage_free = value; }
        public double Storage_total { get => storage_total; set => storage_total = value; }
        public List<VM> Vms { get => vms; set => vms = value; }
        public List<Disk> LocalDisks { get => localDisks; set => localDisks = value; }
        public List<Disk> VSANDisks { get => vSANdisks; set => vSANdisks = value; }
    }
}