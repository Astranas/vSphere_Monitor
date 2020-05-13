using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vSphere_Monitor.Models
{
    public class Cluster
    {

        private string name;
        private bool vsanEnabled;
        private bool drsEnabled;
        private bool haEnabled;
        private List<Host> hosts;

        public Cluster()
        {

        }

        public string Name { get => name; set => name = value; }
        public bool VsanEnabled { get => vsanEnabled; set => vsanEnabled = value; }
        public bool DrsEnabled { get => drsEnabled; set => drsEnabled = value; }
        public bool HaEnabled { get => haEnabled; set => haEnabled = value; }
        public List<Host> Hosts { get => hosts; set => hosts = value; }
    }
}