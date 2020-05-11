using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Web;
using System.Web.Mvc;
using vSphere_Monitor.Models;

namespace vSphere_Monitor.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
        public ActionResult Cluster()
        {
            return View();
        }
        public ActionResult VM()
        {
            return View();
        }

        public ActionResult Host(string id)
        {
            ViewBag.HostID = id;
            return View();
        }

        public void RunPowershell(string args)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"powershell.exe";
            startInfo.Arguments = args;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            StreamReader myStreamReader = process.StandardError;
            // Read the standard error of net.exe and write it on to console.
            Debug.WriteLine(myStreamReader.ReadLine());
        }
        
        public JsonResult GetHostInfo(string hostId)
        {
            if (hostId != "" && hostId != null)
            {
                string vmhostname = "";
                string vmhostfile = "";
                switch (hostId)
                {
                    case "1":
                        vmhostname = "192.168.1.168";
                        vmhostfile = "192-168-1-168";
                        break;
                    case "2":
                        vmhostname = "192.168.1.169";
                        vmhostfile = "192-168-1-169";
                        break;
                    case "3":
                        vmhostname = "192.168.1.174";
                        vmhostfile = "192-168-1-174";
                        break;
                    case "4":
                        vmhostname = "192.168.1.175";
                        vmhostfile = "192-168-1-175";
                        break;
                }

                string path = @"C:\Users\User\source\repos\vSphere_Monitor\vSphere_Monitor\Models\" + vmhostfile + ".json";

                if (CompareDates(path))
                {
                    RunPowershell(@"-executionpolicy unrestricted -file C:\Users\User\source\repos\vSphere_Monitor\vSphere_Monitor\Models\vApp.ps1 -vmhost " + vmhostname);
                }

                string file = System.IO.File.ReadAllText(@"C:\Users\User\source\repos\vSphere_Monitor\vSphere_Monitor\Models\" + vmhostfile + ".json");

                Host host = JsonConvert.DeserializeObject<Host>(file);

                return Json(host, JsonRequestBehavior.AllowGet);

            } else
            {
                return Json("temp", JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetClusterInfo()
        {
            //RunPowershell(@"-executionpolicy unrestricted -file C:\Users\User\source\repos\vSphere_Monitor\vSphere_Monitor\Models\vApp.ps1 -vmhost " + vmhostname);

            return Json("temp", JsonRequestBehavior.AllowGet);
        }

        public bool CompareDates(string filePath)
        {
            int refreshRate = 200;
            FileInfo info = new FileInfo(filePath);
            info.Refresh();
            if (info.Exists)
            {
                DateTime systemTime = DateTime.Now;
                double diffInSeconds = (systemTime - info.LastWriteTime).TotalSeconds;
                if (diffInSeconds > refreshRate)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            } else
            {
                return false;
            }
        }
    }
}