using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TestWebApp.ViewModel;
using TestWebApp.Models;
using System.Xml.Linq;
using TestWebApp.BusinessLayer;
using Vereyon.Web;
using PagedList;


namespace TestWebApp.Controllers
{
    public class CardOrderController : Controller
    {

        private CardOrderEngine cardOrderEngine;
        private CardOrderListViewModel colvm;

        private List<CardOrderRequest> requests;
        private List<CardOrderAnswer> answers;


        public CardOrderController(){}

        public ActionResult Index(int? page)
        {
            int pageIndex = page == null ? 1 : Convert.ToInt32(page);
            int pageSize = 50;

            contextInit();
            cardOrderEngine.prepareCardOrderListViewModel(out requests, out answers, out colvm);

            // https://www.c-sharpcorner.com/article/paging-in-asp-net-mvc-4-using-pagelist/
            IPagedList<CardOrderViewModel> co = null;
            co = colvm.colVm.ToPagedList(pageIndex, pageSize);

            var gitTest = "B";

            return View("Index",/*colvm*/co);
        }

        // return CardOrderViewModel
        public ActionResult OpenOrder(long id/*, bool? triggerOnload, string triggerOnLoadMessage*/)
        {
            contextInit();
            cardOrderEngine.prepareCardOrderListViewModel(out requests, out answers, out colvm);

            // CardOrderViewModel
            var pom = (from item in colvm.colVm where item.ID == id select item).FirstOrDefault();

            if (pom != null)
            {
                return View("OpenOrder", pom);
            }
            else
            {
                // TODO
                // return another view?
                return View("OpenOrder", pom);
            }
        }

        public ActionResult ExportXML(long id)
        {
            try
            {
                contextInit();
                cardOrderEngine.initWebConfigFileMembers();

                cardOrderEngine.exportXML(id);
                FlashMessage.Confirmation("Operation succeeded. XML was exported to "+cardOrderEngine.getDestinationPath());
                return RedirectToAction("OpenOrder", new { id = id });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult MarkAsSent(long id)
        {
            try
            {
                contextInit();

                cardOrderEngine.markAsSent(id);
                FlashMessage.Confirmation("Operation succeeded.");
                return RedirectToAction("OpenOrder", new { id = id });
            }
            catch (Exception e)
            {
                throw;
            }
        }

        // REQ002; PARTNER
        // new CardOrderEngine("cn_db_requests_partner");
        public ActionResult ExportDeliveryList(long id, uint packageId)
        {
            try
            {
                cardOrderEngine = new CardOrderEngine();
                cardOrderEngine.initWebConfigFileMembers();

                List<string> pomlist = new List<string>() { "CardOrderRequests", "CardOrderAnswers" };
                cardOrderEngine.setDbTableInitialization("cardnetDB",pomlist);

                // get card order XML
                var pomXelement  = cardOrderEngine.getCardnetDAL().getXMLByOrderId(id);
                // get card order partner name
                var partnerName = cardOrderEngine.getCardnetDAL().getPartnerNameByOrderId(id);

                // get delivery list 
                var pomDeliveryListType = cardOrderEngine.getCardnetDAL().getCardOrderRequests().Where(co => co.ID == id).Select(co => co.DeliveryListId).First();
                var pomDlTemplateName = cardOrderEngine.getCardnetDAL().getCardnetDB().C___EXPORT_MAP.Where(e => e.CONSTANT_NAME == "delivery_list" && e.CONSTANT_VALUE == pomDeliveryListType).First().TEMPLATE_FILE_NAME.ToString();
                var dlTuple = new Tuple<string, string>(pomDeliveryListType, pomDlTemplateName);

                DeliveryListGenerationApp.AppEngine appEngine = new DeliveryListGenerationApp.AppEngine(pomXelement, packageId, partnerName, id, cardOrderEngine.getSambaPath(), cardOrderEngine.getEnvironment(), dlTuple);

                FlashMessage.Confirmation("Operation succeeded. Delivery List was exported to "+ appEngine.getExportManager().getDestinationPath());
                return RedirectToAction("OpenOrder", new { id = id });
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// TODO
        /// Exports carriers from XML to specific folder
        /// </summary>
        /// <param name="id">ID from [REQ002]</param>
        public ActionResult ExportCarriers(long id)
        {
            contextInit();
            cardOrderEngine.initWebConfigFileMembers();

            List<string> pomlist = new List<string>() { "CardOrderRequests", "CardOrderAnswers" };
            cardOrderEngine.setDbTableInitialization("cardnetDB", pomlist);

            var cardOrderRequest = cardOrderEngine.getCardnetDAL().getCardOrderRequests().Where(r => r.ID == id).FirstOrDefault();
            if (cardOrderRequest != null)
            {
                // RETRIEVE VALUES
                var organizationId = cardOrderEngine.getCardnetDAL().getPartners().Where(p => p.ID == cardOrderRequest.PartnerId).First().RwOrganizationId;
                var organizationCarrierConstantValue = cardOrderRequest.CarrierTypeId;
                var originalXML = XElement.Parse(cardOrderRequest.Content);
                var firstNewCardId = cardOrderEngine.getRewardoEngine().getRewardoDAL().getCardsFromRW().Where(c => c.OrganizationId == organizationId).OrderByDescending(c => c.CardCRMId).Select(c=>c.CardCRMId).FirstOrDefault();
                if (firstNewCardId != null) // ak su v db njake karty danej organizacie
                    firstNewCardId += 1;
                else // ak nie su -||-
                    firstNewCardId = 1;


                var key = "carrier";
                var constantValue = cardOrderRequest.CarrierTypeId.ToString();

                var x = cardOrderEngine.getCardnetDAL().getCardnetDB().C___EXPORT_MAP.Where(em => em.CONSTANT_NAME == key && em.CONSTANT_VALUE == constantValue).FirstOrDefault();

                //  1.  validate xml against XSD
                //  2.  if valid    => return true
                //      else        => return false
                if (x != null)
                {
                    // PASS VALUES INTO CONSTRUCTOR PARAMETERS
                    var carriersManager = new GenerateNewXMLForFastReportClassLibrary.AppEngine(organizationCarrierConstantValue,originalXML,Convert.ToInt64(firstNewCardId));
                    var newXML = carriersManager.getNewXMLValue();

                    var oldXml = XElement.Parse(cardOrderRequest.Content);
                    var cardOrderId = id;
                    var partnerName = cardOrderEngine.getCardnetDAL().getPartnerNameByOrderId(id);
                    var templateLocation = @AppDomain.CurrentDomain.BaseDirectory + @"App_Data\";
                    var schemaLocation = @AppDomain.CurrentDomain.BaseDirectory + @"App_Data\"+x.ADDITIONAL_COLUMN1;
                    var dstPath = @cardOrderEngine.getSambaPath() + @cardOrderEngine.getEnvironment() + @"\PARTNERS\" + partnerName + @"\" + "CARD_ORDERS" + @"\" + cardOrderId + @"\CARRIERS\";

                    // key = {"carrier", "delivery_list" ...}; value = dict with key variableName {exportConstantName, exportConstantValue, templateLocation, oldXml, partnerName, dstPath, cardOrderId, xmlForfastReport, environment} and value variableValue 
                    Dictionary<string, string> pomDict = new Dictionary<string, string>();
                    pomDict.Add("exportConstantValue", newXML.Element("carrier_type_id").Value);
                    pomDict.Add("templateLocation", templateLocation);
                    pomDict.Add("schemaLocation", schemaLocation);
                    pomDict.Add("oldXml", oldXml.ToString());
                    pomDict.Add("partnerName", partnerName);
                    pomDict.Add("dstPath", dstPath);
                    pomDict.Add("xmlForFastReport", newXML.ToString());
                    pomDict.Add("cardOrderId", cardOrderRequest.ID.ToString());
                    pomDict.Add("environment", @cardOrderEngine.getEnvironment().ToString().ToLower().Substring(5));



                    KeyValuePair<string, Dictionary<string, string>> exportSubject = new KeyValuePair<string, Dictionary<string, string>>("carrier",pomDict);

                    TestWebApp.ExportCarriers.AppEngine appEngine = new TestWebApp.ExportCarriers.AppEngine(exportSubject, cardOrderEngine.getCardnetDAL());
                    try
                    {
                        appEngine.Run();
                        FlashMessage.Confirmation("Operation succeeded. Carriers were exported to " + appEngine.GetMyFastReport().GetDestinationFilePath());
                    }
                    catch (Exception e)
                    {
                        FlashMessage.Warning("Operation not succeeded. Carriers hasn't been exported. Possible error: XML is invalid..." + e.Message);
                    }
                }
                else
                {
                    FlashMessage.Warning("Operation not succeeded. Carriers hasn't been exported because carrier_type_id ("+cardOrderRequest.CarrierTypeId.ToString()+") is not registered value in the database. If you want to use this value you have to add/register it in the cardnet database");
                }
            }
            return RedirectToAction("OpenOrder", new { id = id });
        }

        private void setViewModelMessage(long id, bool triggerOnLoad, string triggerOnLoadMessage)
        {
            var vm = colvm.colVm.Where(d => d.ID == id).FirstOrDefault();
            if (vm != null) { vm.TriggerOnLoad = triggerOnLoad; vm.TriggerOnLoadMessage = triggerOnLoadMessage; }
        }

        /// <summary>
        /// Shows cards from Rewardo in specified package
        /// </summary>
        /// <param name="id"></param>
        /// <param name="packageId"></param>
        /// REQ002; PARTNER;
        public ActionResult ShowCardsInPackage(long id, uint packageId)
        {
            cardOrderEngine = new CardOrderEngine();

            List<string> tablesToInit = new List<string>() { "CardOrderRequests", "CardOrderAnswers", "partners" };
            cardOrderEngine.setDbTableInitialization("all", tablesToInit);

            // get card order partner name
            var partnerName = cardOrderEngine.getCardnetDAL().getPartnerNameByOrderId(id);

            // get organization ID from REWARDO.ORGANIZATION table according to @param partnerName == ORGANIZATION.NAME == ___PARTNER.ORGANIZATION_NAME
            var organizationID = cardOrderEngine.getRewardoEngine().getOrganizationId(partnerName);

            // get card order XML
            var pomXelement = cardOrderEngine.getCardnetDAL().getXMLByOrderId(id);

            var partnerCardIdsFromRW = cardOrderEngine.getRewardoEngine().getPartnerCardIdsFromRW(partnerName); // from RW
            var partnerCardsFromCNXML = cardOrderEngine.getCardnetDAL().getCardsFromXMLByPartnerAndPackageIdFromCN(id, packageId); // from CN
            var cardsNotInRW = new List<long>();

            var pomRewardoCardListViewModel = cardOrderEngine.getRewardoEngine().getRewardoCardListViewModel();
            var pom1 = new RewardoCardListViewModel();
            cardOrderEngine.getRewardoEngine().createRewardoCardListViewModel(partnerCardsFromCNXML, pom1, pomRewardoCardListViewModel, organizationID);

            pom1.orderId = id;
            pom1.packageId = packageId;
            pom1.rclvm.OrderBy(rcl => rcl.OrderInPackage);

            int pomocnaPremenna = 0;
            // better to use DO-WHILE
            foreach (var i in pom1.rclvm)
                if (i.Cardno == "")
                {
                    pomocnaPremenna += 1;
                }

            if (pomocnaPremenna != 0)
            {
                pom1.infoColor = "red";
                pom1.info = "Uncompleted order in Rewardo. Some (or all) cards are not still in Rewardo.";
                return View("ShowCardsInPackage", pom1);
            }
            else
            {
                pom1.infoColor = "green";
                pom1.info = "Cards are succesfully created in Rewardo.";
                return View("ShowCardsInPackage", pom1);
            }
        }

        private void contextInit()
        {
            // -------------------------------------------------------------------
            // DB INIT
            // -------------------------------------------------------------------
            cardOrderEngine = new CardOrderEngine();

            List<string> tablesToInit = new List<string>();
            tablesToInit.Add("partners");
            tablesToInit.Add("CardOrderRequests");
            tablesToInit.Add("CardOrderAnswers");
            tablesToInit.Add("partners");
            tablesToInit.Add("constantUniversals");
            tablesToInit.Add("constantPartners");
            cardOrderEngine.setDbTableInitialization("all", tablesToInit);
            // -------------------------------------------------------------------
        }
    }
}