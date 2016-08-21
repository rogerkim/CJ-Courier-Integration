using MangoCarrierSystem.Models;
using MangoCarrierSystem.REST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MangoCarrierSystem.Controllers
{
    public class ScannerController : Controller
    {
        LFLRestService webservice = new LFLRestService();

        // GET: Scanner
        public ActionResult Index()
        {
            // check login session
            if (Session["USERID"] == null)
            {
                return RedirectToAction("Index");
            }

            MainViewModel model = new MainViewModel();
            if (TempData["ShippingLabel"] != null)
            {
                model = (MainViewModel)TempData["ShippingLabel"];

            }
            return View(model);
        }

        // GET: Scanner/GetShippingLabel/5
        public async Task<ActionResult> GetShippingLabel(int id)
        {
            var response = await webservice.GetShippingLabel(id);
            TempData["ShippingLabel"] = GetMainViewModel(response); 
            return RedirectToAction("Index");
        }

        private MainViewModel GetMainViewModel(ShippingLabelResponse response)
        {
            var result = new MainViewModel();
            result.AgencyName = response.AgencyName;
            result.DriverCode = response.DriverCode;
            result.DriverName = response.DriverName;
            result.MainTerminalCode = response.MainTerminalCode;
            result.ReceiverShortAddress1 = response.ReceiverShortAddress1;
            result.ReceiverShortAddress2 = response.ReceiverShortAddress2;
            result.RecevierZipcode = response.RecevierZipcode;
            result.SubTerminalCode = response.SubTerminalCode;
            result.TrackingNumber = response.TrackingNumber;
            return result;
        }
    }
}
