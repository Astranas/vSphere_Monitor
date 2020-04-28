using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public ActionResult Host(string id)
        {
            ViewBag.HostID = id;
            return View();
        }

        public JsonResult RunPowershell(string id)
        {
            var host = GetHostInfo();
            return Json(host, JsonRequestBehavior.AllowGet);

            //string scriptfilepath = "";

            //if (scriptfilepath.Length <= 0)
            //{
            //    return Json(result, JsonRequestBehavior.AllowGet);

            //}
            //else
            //{
            //    PowerShell powershell = PowerShell.Create();
            //    //RunspaceConfiguration runspaceConfiguration = RunspaceConfiguration.Create();
            //    //Runspace runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration)
            //    using (Runspace runspace = RunspaceFactory.CreateRunspace())
            //    {
            //        runspace.Open();
            //        //RunspaceInvoke scriptInvoker = new RunspaceInvoke(runspace);
            //        //scriptInvoker.Invoke("Set-ExecutionPolicy Unrestricted");
            //        powershell.Runspace = runspace;
            //        //powershell.Commands.AddScript("Add-PsSnapin Microsoft.SharePoint.PowerShell");
            //        System.IO.StreamReader sr = new System.IO.StreamReader(scriptfilepath);
            //        powershell.AddScript(sr.ReadToEnd());
            //        //powershell.AddCommand("Out-String");
            //        var results = powershell.Invoke();
            //        if (powershell.Streams.Error.Count > 0)
            //        {
            //            // error records were written to the error stream.
            //            // do something with the items found.
            //        }
            //    }
            //    return Json("temp", JsonRequestBehavior.AllowGet);
            //}

        }
        
        public List<Host> GetHostInfo()
        {
            //string file = System.IO.File.ReadAllText(@"C:\Users\User\source\repos\vSphere_Monitor\vSphere_Monitor\Models\jsonResult.json");
            //var result = JsonConvert.DeserializeObject(file);

            var vmlist = new List<VM>
            {
                new VM
                {
                    Name = "TinyCore1",
                    Cpu_number = 1,
                    Memory = 256
                },
                new VM
                {
                    Name = "centOS7",
                    Cpu_number = 1,
                    Memory = 1
                }
            };

            var hostList = new List<Host>
            {
                new Host
                {
                    Adress = "192.168.1.130",
                    Cpu_total = 12,
                    Cpu_usage = 6,
                    Memory_total = 6,
                    Memory_usage = 2,
                    Vms = vmlist
                }
            };

            return hostList;
        }
    }
}