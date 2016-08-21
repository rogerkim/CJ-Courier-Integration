using MangoCarrierSystem.CJKE;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MangoCarrierSystem.BL
{
    public class CJManager
    {
        private readonly string CLIENTID = ConfigurationManager.AppSettings["CJ_CLIENTID"];
        private readonly string SENDER_ADDRESS = ConfigurationManager.AppSettings["SENDER_ADDRESS"];

        public ShippingLabel GetShippingLabel(string address)
        {
            AddressSearchWSImplClient service = new AddressSearchWSImplClient();
            MangoCarrierSystem.CJKE.addressSearchResponse[] response = service.getAddressInformationByValue(GetRequest(address));
            return GetShippingLabel(response[0]);
        }

        private addressSearchRequest[] GetRequest(string address)
        {
            addressSearchRequest[] result = new addressSearchRequest[1];
            result[0] = new addressSearchRequest();
            result[0].boxTyp = "2";
            result[0].clntMgmCustCd = CLIENTID;
            result[0].clntNum = CLIENTID;
            result[0].cntrLarcCd = "01";
            result[0].fareDiv = "03";
            result[0].orderNo = "LFLK";
            result[0].prngDivCd = "01";
            result[0].rcvrAddr = address;
            result[0].sndprsnAddr = SENDER_ADDRESS;
            return result;
        }

        private ShippingLabel GetShippingLabel(addressSearchResponse response)
        {
            ShippingLabel result = new ShippingLabel();
            result.RecevierZipcode = response.rcvrZipnum;
            result.ReceiverShortAddress1 = response.rcvrShortAddr;
            result.ReceiverShortAddress2 = response.rcvrClsfAddr;
            result.AgencyName = response.dlvPreArrBranShortNm;
            result.DriverName = response.dlvPreArrEmpNm;
            result.DriverCode = response.dlvPreArrEmpNickNm;
            result.MainTerminalCode = response.dlvClsfCd;
            result.SubTerminalCode = response.dlvSubClsfCd;
            return result;
        }
    }
}