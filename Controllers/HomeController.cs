using MangoCarrierSystem.BL;
using MangoCarrierSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MangoCarrierSystem.Controllers
{
    public class HomeController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(HomeController));
        private FileManager fileManager = new FileManager();
        private CJManager carrierManager = new CJManager();

        public ActionResult Index()
        {
            ViewBag.ErrorMsg = TempData["ErrorMsg"];
            return View();
        }

        public ActionResult Step1()
        {
            // check login session
            if (Session["USERID"] == null)
            {
                return RedirectToAction("Index");
            }

            List<MainViewModel> model = new List<MainViewModel>();
            if (TempData["OrdersVM"] != null)
            {
                model = (List<MainViewModel>)TempData["OrdersVM"];

            }
            ViewBag.Message = TempData["Message"];
            return View(model);
        }

        [HttpPost]
        public ActionResult ValidatePassword(FormCollection formCollection)
        {
            string userId = formCollection["userId"];
            string pwd = formCollection["pwd"];

            if (userId.Equals("lflkorea") && pwd.Equals("abcd1234"))
            {
                Session["USERID"] = userId;
                return RedirectToAction("Step1");
            }
            else
            {
                TempData["ErrorMsg"] = "ErrorMsg";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            List<Order> orders = fileManager.ReadFile(file);
            string message = string.Empty;
            // Save orders to DB.
            using (var db = new MangoEntities())
            {
                try
                {
                    foreach (var row in orders)
                    {
                        row.STAUTS = (int)OrderStatus.Code.New;
                        db.Orders.Add(row);
                    }
                    db.SaveChanges();
                    message = orders.Count + " orders are imported. Go to Step 2.";
                }
                catch (Exception ex)
                {
                    message = ex.ToString();
                    log.Error(message);
                }
            }
            TempData["OrdersVM"] = GetOrderVM(orders);
            TempData["Message"] = message;
            return RedirectToAction("Step1");
        }

        private List<MainViewModel> GetOrderVM(List<Order> orders)
        {
            List<MainViewModel> result = new List<MainViewModel>();

            //find the shipping lable with orderid
            using (var db = new MangoEntities())
            {
                foreach (Order item in orders)
                {
                    MainViewModel viewModel = new MainViewModel();

                    // Order
                    viewModel.ID = item.ID.ToString();
                    viewModel.SHIPMENT_DATE = item.SHIPMENT_DATE.ToString();
                    viewModel.BOX_NUMBER = item.BOX_NUMBER.ToString();
                    viewModel.ORDER = item.ORDERID;
                    viewModel.FULL_NAME = item.FULL_NAME;
                    viewModel.PHONE = item.PHONE;
                    viewModel.COUNTRY = item.COUNTRY;
                    viewModel.ZIPCODE = item.ZIPCODE;
                    viewModel.LOCATION = item.LOCATION;
                    viewModel.PROVINCE = item.PROVINCE;
                    viewModel.ADDRESS = item.ADDRESS;
                    viewModel.STATUS = ((OrderStatus.Code)item.STAUTS).ToString();

                    // Shipping Label
                    var label = db.ShippingLabels
                                .Where(s => s.OrderId == item.ID)
                                .FirstOrDefault() ?? new ShippingLabel();

                    viewModel.MainTerminalCode = label.MainTerminalCode;
                    viewModel.SubTerminalCode = label.SubTerminalCode;
                    viewModel.DriverCode = label.DriverCode;
                    viewModel.DriverName = label.DriverName;
                    viewModel.AgencyName = label.AgencyName;
                    result.Add(viewModel);
                }
            }
            return result;
        }

        public ActionResult Step2()
        {
            // check login session
            if (Session["USERID"] == null)
            {
                return RedirectToAction("Index");
            }

            List<MainViewModel> model = new List<MainViewModel>();
            var status = new int[] { (int)OrderStatus.Code.Error, (int)OrderStatus.Code.New, (int)OrderStatus.Code.Verified };
            using (var db = new MangoEntities())
            {
                // Select New, Error, Verified Orders
                var orders = (from O in db.Orders
                              where (status.Contains(O.STAUTS))
                              select O).ToList();
                model = GetOrderVM(orders);

                // select STAUTS, count(STAUTS) as count 
                // from [dbo].[Order]
                // group by STAUTS
                var query = db.Orders
                   .GroupBy(p => p.STAUTS)
                   .Select(g => new { name = g.Key, count = g.Count() }).ToList();
                foreach (var item in query)
                {
                    if (item.name == (int)OrderStatus.Code.New)
                    {
                        ViewBag.NewCount = item.count;
                    }
                    else if (item.name == (int)OrderStatus.Code.Error)
                    {
                        ViewBag.ErrorCount = item.count;
                    }
                    else if (item.name == (int)OrderStatus.Code.Verified)
                    {
                        ViewBag.VerfiedCount = item.count;
                    }
                }
            }
            ViewBag.NewCount = (ViewBag.NewCount == null) ? 0 : ViewBag.NewCount;
            ViewBag.ErrorCount = (ViewBag.ErrorCount == null) ? 0 : ViewBag.ErrorCount;
            ViewBag.VerfiedCount = (ViewBag.VerfiedCount == null) ? 0 : ViewBag.VerfiedCount;
            return View(model);
        }

        public ActionResult ValidateAddress()
        {
            // Validate address for order Status New and Error
            var status = new int[] { (int)OrderStatus.Code.Error, (int)OrderStatus.Code.New };

            using (var db = new MangoEntities())
            {
                try
                {
                    // Get order status New and Error
                    var orderList = (from O in db.Orders
                                     where (status.Contains(O.STAUTS))
                                     select O).ToList();

                    // Retrieve address
                    foreach (Order order in orderList)
                    {
                        // Call CJ API
                        string address = order.PROVINCE + order.ADDRESS;
                        ShippingLabel cjLabel = carrierManager.GetShippingLabel(address);

                        // order is new
                        if (order.STAUTS == (int)OrderStatus.Code.New)
                        {
                            // insert the record to shipping label table.
                            cjLabel = SetShippingLabel(cjLabel, order);
                            db.ShippingLabels.Add(cjLabel);
                        }

                        // order has error
                        if (order.STAUTS == (int)OrderStatus.Code.Error)
                        {
                            // update the shipping label record
                            ShippingLabel updatedLabel = db.ShippingLabels.Where(s => s.OrderId == order.ID).FirstOrDefault();
                            updatedLabel = SetUpdatedShippingLabel(cjLabel, updatedLabel, order);
                        }
                    }
                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    log.Error(ex.ToString());
                }
            }

            return RedirectToAction("Step2");
        }

        private ShippingLabel SetShippingLabel(ShippingLabel cjLabel, Order order)
        {
            // Check zipcode is null or empty.
            if (string.IsNullOrEmpty(cjLabel.RecevierZipcode))
            {
                // Error
                order.STAUTS = (int)OrderStatus.Code.Error;
                cjLabel.OrderId = order.ID;
            }
            else
            {
                // Verified
                order.STAUTS = (int)OrderStatus.Code.Verified;
                cjLabel.OrderId = order.ID;
            }

            return cjLabel;
        }

        private ShippingLabel SetUpdatedShippingLabel(ShippingLabel cjLabel, ShippingLabel updatedLabel, Order order)
        {
            // Check zipcode is null or empty.
            if (string.IsNullOrEmpty(cjLabel.RecevierZipcode))
            {
                // Error, do nothing.
            }
            else
            {
                // Verified
                order.STAUTS = (int)OrderStatus.Code.Verified;
                updatedLabel.OrderId = order.ID;
                updatedLabel.AgencyName = cjLabel.AgencyName;
                updatedLabel.DriverCode = cjLabel.DriverCode;
                updatedLabel.DriverName = cjLabel.DriverName;
                updatedLabel.MainTerminalCode = cjLabel.MainTerminalCode;
                updatedLabel.ReceiverShortAddress1 = cjLabel.ReceiverShortAddress1;
                updatedLabel.ReceiverShortAddress2 = cjLabel.ReceiverShortAddress2;
            }

            return updatedLabel;
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MainViewModel model = new MainViewModel();
            using (var db = new MangoEntities())
            {
                Order order = db.Orders.Find(id);
                model.ID = order.ID.ToString();
                model.ORDER = order.ORDERID;
                model.PROVINCE = order.PROVINCE;
                model.ADDRESS = order.ADDRESS;
                model.STATUS = ((OrderStatus.Code)order.STAUTS).ToString();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult SaveOrder(int id, string province, string address)
        {
            using (var db = new MangoEntities())
            {
                try
                {
                    Order order = db.Orders.Find(id);
                    order.PROVINCE = province;
                    order.ADDRESS = address;
                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    log.Error(ex.ToString());
                }
            }
            return RedirectToAction("Step2", "Home");
        }
    }
}