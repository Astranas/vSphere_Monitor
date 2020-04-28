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
        private List<VM> vms;

        public Host()
        {

        }

        public string Adress { get => adress; set => adress = value; }
        public double Memory_usage { get => memory_usage; set => memory_usage = value; }
        public double Memory_total { get => memory_total; set => memory_total = value; }
        public double Cpu_usage { get => cpu_usage; set => cpu_usage = value; }
        public double Cpu_total { get => cpu_total; set => cpu_total = value; }
        public List<VM> Vms { get => vms; set => vms = value; }
    }
}