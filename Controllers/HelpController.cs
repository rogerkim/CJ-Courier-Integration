using MangoCarrierSystem.BL;
using MangoCarrierSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MangoCarrierSystem.Controllers
{
    public class HelpController : Controller
    {
        private CJManager carrierManager = new CJManager();

        // GET: API
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
            if (TempData["KoreanAddress"] != null)
            {
                ViewBag.KoreanAddress = (string)TempData["KoreanAddress"];

            }
            return View(model);
        }

        // GET: Help/ShippingLabel/서울시
        public ActionResult ShippingLabel(string address)
        {
            ShippingLabel shippingLabel = carrierManager.GetShippingLabel(address);

            MainViewModel viewModel = new MainViewModel();
            viewModel.AgencyName = shippingLabel.AgencyName;
            viewModel.ZIPCODE = shippingLabel.RecevierZipcode;
            viewModel.MainTerminalCode = shippingLabel.MainTerminalCode;
            viewModel.SubTerminalCode = shippingLabel.SubTerminalCode;
            viewModel.DriverName = shippingLabel.DriverName;
            viewModel.DriverCode = shippingLabel.DriverCode;
            viewModel.ReceiverShortAddress1 = shippingLabel.ReceiverShortAddress1;
            viewModel.ReceiverShortAddress2 = shippingLabel.ReceiverShortAddress2;

            TempData["ShippingLabel"] = viewModel;
            TempData["KoreanAddress"] = address;
            return RedirectToAction("Index");
        }
    }
}
