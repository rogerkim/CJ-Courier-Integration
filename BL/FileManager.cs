using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace MangoCarrierSystem.BL
{
    public class FileManager
    {
        private string[] lineSeparator = new string[] { "\r\n" };
        private string[] tokenSeparator = new string[] { "~" };

        public List<Order> ReadFile(HttpPostedFileBase file)
        {
            string result = string.Empty;

            //http://stackoverflow.com/questions/16030034/asp-net-mvc-read-file-from-httppostedfilebase-without-save
            //You will need first to read the entire stream from the HTTP call into a byte array, then create the FileStream from that array.
            using (var reader = new System.IO.BinaryReader(file.InputStream))
            {
                byte[] binData = reader.ReadBytes(Convert.ToInt32(file.InputStream.Length));
                result = System.Text.Encoding.UTF8.GetString(binData);
            }

            string[] lines = result.Split(lineSeparator, StringSplitOptions.None);

            List<Order> orderList = new List<Order>();
            for (int i = 1; i < lines.Length; i++)
            {
                string[] tokens = lines[i].Split(tokenSeparator, StringSplitOptions.None);
                Order order = new Order();
                for (int j = 0; j < tokens.Length; j++)
                {
                    // Shipping Date
                    DateTime tmpShippingDate = DateTime.MinValue;
                    DateTime.TryParseExact(tokens[0], "yyyyMMdd", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out tmpShippingDate);
                    order.SHIPMENT_DATE = tmpShippingDate;

                    // Box Number
                    int tmpBoxNumber = 0;
                    Int32.TryParse(tokens[1], out tmpBoxNumber);
                    order.BOX_NUMBER = tmpBoxNumber;

                    order.ORDERID = tokens[2].Trim();
                    order.FULL_NAME = tokens[3].Trim();
                    order.PHONE = tokens[4].Trim();
                    order.COUNTRY = tokens[5].Trim();
                    order.ZIPCODE = tokens[6].Trim();
                    order.LOCATION = tokens[7].Trim();
                    order.PROVINCE = tokens[8].Trim();
                    order.ADDRESS = tokens[9].Trim();
                    order.WEIGHT = tokens[10].Trim();
                    order.TOTALOFBOXES = tokens[11].Trim();
                    order.TOTALAMOUNT = tokens[12].Trim();
                    order.CURRENCY_EUR = tokens[13].Trim();
                    order.VOTF = tokens[14].Trim();
                    order.SERVICECODE = tokens[15].Trim();
                    order.SERVICE = tokens[16].Trim();
                    order.MERCHANDISE_TYPE = tokens[17].Trim();
                    order.INVOICING_COMPANY = tokens[18].Trim();
                    order.DEBTOR = tokens[19].Trim();
                    order.STORE_TELEPHONE = tokens[20].Trim();
                    order.DELIVER_TO = tokens[21].Trim();
                    order.STORE = tokens[22].Trim();
                    order.BOX_VOLUME = tokens[23].Trim();
                    order.BOX_MEASUREMENTS = tokens[24].Trim();
                    order.ID_CARD = tokens[25].Trim();
                    order.COD = tokens[26].Trim();
                    order.TRACKING_NR_DEVO = tokens[27].Trim();
                    order.CIF_VALUE = tokens[28].Trim();
                    order.CURRENCY_NON_UE = tokens[29].Trim();
                    order.EMAIL = tokens[30].Trim();
                    order.COMPANY = tokens[31].Trim();
                    order.NAME_PACKSTATION = tokens[32].Trim();
                }
                orderList.Add(order);
            }
            return orderList;
        }
    }
}