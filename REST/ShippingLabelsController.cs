using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MangoCarrierSystem;
using MangoCarrierSystem.Models;

namespace MangoCarrierSystem.REST
{
    public class ShippingLabelsController : ApiController
    {
        private MangoEntities db = new MangoEntities();

        // GET: api/ShippingLabels/5
        [ResponseType(typeof(ShippingLabelResponse))]
        public IHttpActionResult GetShippingLabel(int id)
        {
            var result = new ShippingLabelResponse();
            ShippingLabel shippingLabel = db.ShippingLabels.Find(id);
            if (shippingLabel == null)
            {
                return NotFound();
            }
            else
            {
                result = GetShippingLabelResponse(shippingLabel);
            }
            return Ok(result);
        }

        private ShippingLabelResponse GetShippingLabelResponse(ShippingLabel source)
        {
            var result = new ShippingLabelResponse();
            result.AgencyName = source.AgencyName;
            result.DriverCode = source.DriverCode;
            result.DriverName = source.DriverName;
            result.MainTerminalCode = source.MainTerminalCode;
            result.ReceiverShortAddress1 = source.ReceiverShortAddress1;
            result.ReceiverShortAddress2 = source.ReceiverShortAddress2;
            result.RecevierZipcode = source.RecevierZipcode;
            result.SubTerminalCode = source.SubTerminalCode;
            result.TrackingNumber = source.TrackingNumber;
            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ShippingLabelExists(int id)
        {
            return db.ShippingLabels.Count(e => e.ID == id) > 0;
        }
    }
}