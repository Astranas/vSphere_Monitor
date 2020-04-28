using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vSphere_Monitor.Models
{
    public class VM
    {
        private string name;
        private int cpu_number;
        private double memory;

        public VM()
        {

        }

        public string Name { get => name; set => name = value; }
        public int Cpu_number { get => cpu_number; set => cpu_number = value; }
        public double Memory { get => memory; set => memory = value; }
    }
}