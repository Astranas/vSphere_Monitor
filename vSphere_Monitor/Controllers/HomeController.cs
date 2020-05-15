using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            Image img = new Image();
            return View(img);
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

        /// <summary>
        /// Runs a .NET ProcessStartInfo to launch a Powershell Script
        /// </summary>
        /// <param name="args">list of arguments to give to the Powershell instance including location of the script</param>
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

        /// <summary>
        /// Retrieves specific VMGuest info
        /// </summary>
        /// <param name="hostId">ID of the expected VMGuest to target</param>
        /// <returns>JSON formated data to view</returns>
        public JsonResult GetHostInfo(string hostId)
        {
            if (hostId != "" && hostId != null)
            {
                string vmhostname = "";
                string vmhostfile = "";
                switch (hostId)
                {
                    case "1":
                        vmhostname = ""; //Adapt to VMGuest name or IP adress
                        vmhostfile = ""; //Adapt to script output file
                        break;
                    case "2":
                        vmhostname = ""; //Adapt to VMGuest name or IP adress
                        vmhostfile = ""; //Adapt to script output file
                        break;
                    case "3":
                        vmhostname = ""; //Adapt to VMGuest name or IP adress
                        vmhostfile = ""; //Adapt to script output file
                        break;
                    case "4":
                        vmhostname = ""; //Adapt to VMGuest name or IP adress
                        vmhostfile = ""; //Adapt to script output file
                        break;
                }

                string path = @""; //Adapt to script output file location

                if (CompareDates(path))
                {
                    RunPowershell(@""); //Adapt to script file location and optionnal parameters given to script
                }

                string file = System.IO.File.ReadAllText(@""); //Adapt to script output file location

                Host host = JsonConvert.DeserializeObject<Host>(file);

                return Json(host, JsonRequestBehavior.AllowGet);

            } else
            {
                return Json("temp", JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// Retrieves all cluster info
        /// </summary>
        /// <returns>JSON formated data to view</returns>
        public JsonResult GetClusterInfo()
        {
            string path = @""; //Adapt to script output file location

            if (CompareDates(path))
            {
                RunPowershell(@""); //Adapt to script file location and optionnal parameters given to script
            }
            string file = System.IO.File.ReadAllText(@""); //Adapt to script output file location

            Cluster clu = JsonConvert.DeserializeObject<Cluster>(file);

            return Json(clu, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retrieves all virtual machines info
        /// </summary>
        /// <returns>JSON formated data to view</returns>
        public JsonResult GetVmInfo()
        {
            string path = @""; //Adapt to script output file location

            if (CompareDates(path))
            {
                RunPowershell(@""); //Adapt to script file location and optionnal parameters given to script
            }
            string file = System.IO.File.ReadAllText(@""); //Adapt to script output file location
            List<VM> vmlist = JsonConvert.DeserializeObject<List<VM>>(file);

            return Json(vmlist, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Compares the creation date of a file to the system time
        /// </summary>
        /// <param name="filePath">The location of the file on the system to get the creation time</param>
        /// <returns>true if the difference between the system time and file creation time exceeds a selected threshold</returns>
        public bool CompareDates(string filePath)
        {
            //time in seconds that define the threshold
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