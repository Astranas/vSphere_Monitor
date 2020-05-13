using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vSphere_Monitor.Models
{
    public class VM
    {
        private string name;
        private string createDate;
        private int cpu_number;
        private double memory;
        private byte state;
        private string vsanHealth;
        private string host;

        public VM()
        {

        }

        public string Name { get => name; set => name = value; }
        public string CreateDate { get => createDate; set => createDate = value; }
        public int Cpu_number { get => cpu_number; set => cpu_number = value; }
        public double Memory { get => memory; set => memory = value; }
        public byte State { get => state; set => state = value; }
        public string VsanHealth { get => vsanHealth; set => vsanHealth = value; }
        public string Host { get => host; set => host = value; }
    }
}