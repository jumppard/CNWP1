using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestWebApp.Models;
using TestWebApp.ViewModel;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using TestWebApp.DataAccessLayer;
using System.Data.SqlClient;
using System.Data.Entity.Core.EntityClient;

namespace TestWebApp.BusinessLayer
{
    // business layer // aplikacna logika pre card order engine
    public class CardOrderEngine : IReqAnsEngine<CardOrderRequest,CardOrderAnswer,CardOrderListViewModel>
    {
        private CardnetDAL m_cardnetDAL;
        private RewardoEngine rwEngine;

        private string m_sambaPath;
        private string m_environment;

        private string m_destinationPath = "";
        private string m_cnServerIP;
        private string m_cnDbName;
        private string m_cnDbUsername;
        private string m_cnDbUserpassword;

        private string m_rwServerIP;
        private string m_rwDbName;
        private string m_rwDbUsername;
        private string m_rwDbUserpassword;

        public CardOrderEngine(){}

        public void setDbTableInitialization(string whatToinit/*temp*/, List<string> tablesToInit = null)
        {
            init(whatToinit);

            if (whatToinit == "cardnetDB" || whatToinit == "all")
            {
                if (tablesToInit != null)
                {
                    foreach (var item in tablesToInit)
                    {
                        m_cardnetDAL.initializeDbTable(item);
                    }
                }
            }
        }

        public void init(string whatToInit)
        {
            if (whatToInit == "all")
            {
                try
                {
                    m_cardnetDAL = new CardnetDAL();
                    rwEngine = new RewardoEngine();
                }
                catch (Exception e)
                {
                    throw;
                }
            }
            else if (whatToInit == "cardnetDB")
            {
                try
                {
                    m_cardnetDAL = new CardnetDAL();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else if (whatToInit == "rewardoDB")
            {
                try
                {
                    rwEngine = new RewardoEngine();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public void prepareCardOrderListViewModel(out List<CardOrderRequest> requests, out List<CardOrderAnswer> answers, out CardOrderListViewModel colvm)
        {
            // -------------------------------------------------------------------
            // CardOrderListViewModel INIT
            // -------------------------------------------------------------------
            colvm = new CardOrderListViewModel();

            requests = getCardnetDAL().getCardOrderRequests();
            answers = getCardnetDAL().getCardOrderAnswers();
            colvm = fillListViewModel(requests, answers);
            // -------------------------------------------------------------------
        }


        /// <summary>
        /// Fill $colVM with @cor and @coa.
        /// $colVM is the combination of @cor and @coa
        /// </summary>
        /// <param name="colVM"></param>
        /// <param name="cor"></param>
        /// <param name="coa"></param>
        /// interface method
        public CardOrderListViewModel fillListViewModel(List<CardOrderRequest> cor, List<CardOrderAnswer> coa)
        {
            // create CardOrderListViewModel
            CardOrderListViewModel p_colVM = new CardOrderListViewModel();

            // fill CardOrderListViewModel
            foreach (var request in cor)
            {
                // create CardOrderViewModel
                CardOrderViewModel cardOrderVM = new CardOrderViewModel();

                var validStatusDate = "";
                var receivedStatusDate = "";
                var declinedStatusDate = "";
                var sentStatusDate = "";
                var successfullyCanceledStatusDate = "";
                var unsuccessfullyCanceledStatusDate = "";
                var pomRowColor = "";
                var message = "";

                fillCardOrderStatusesDate(out message, out validStatusDate, out receivedStatusDate, out declinedStatusDate, out sentStatusDate, out successfullyCanceledStatusDate, out unsuccessfullyCanceledStatusDate, out pomRowColor, coa, request);
                cardOrderVM = createAndReturnNewCardOrderViewModel(request.CardTypeId, request.Content, request.CreationDateTime, request.ID, message, request.PartnerId, request.PartnerRequestId, request.RequestID,
                                                                                 request.CardCount, request.CarrierTypeId, request.EnvelopeTypeId, request.PlasticTypeId, request.AttachementId, request.PackageAttachementId, request.RequestStatus, request.LastUpdate, request.SlaDeadLine, request.StatusFinal, request.ValidTo, request.IsProcessing, validStatusDate, receivedStatusDate
                                                                                 , declinedStatusDate, sentStatusDate, successfullyCanceledStatusDate, unsuccessfullyCanceledStatusDate, pomRowColor, request.OrderType, request.DeliveryListId);

                // add CardOrderViewModel into CardOrderListViewModel
                p_colVM.colVm.Add(cardOrderVM);
            }

            return p_colVM;

        }

        public void fillCardOrderStatusesDate(out string message, out string validStatusDate, out string receivedStatusDate, out string declinedStatusDate, out string sentStatusDate, out string successfullyCanceledStatusDate, out string unsuccessfullyCanceledStatusDate, out string pomRowColor, List<CardOrderAnswer> coa, CardOrderRequest request)
        {
            var pom1 = (from request1 in m_cardnetDAL.getCardOrderAnswers() where request1.PartnerId == request.PartnerId && request1.RequestId == request.RequestID select request1.Message).LastOrDefault();
            message = pom1 == null ? "" : pom1.ToString();

            var pomValidStatusDate = (from answer in m_cardnetDAL.getCardOrderAnswers()
                                      where answer.PartnerId == request.PartnerId && answer.RequestId == request.RequestID && answer.RequestStatus == 4
                                      select answer.ChangeDateTime).FirstOrDefault();

            validStatusDate = pomValidStatusDate == null ? "todo" : pomValidStatusDate.ToString();


            var pomReceivedStatusDate = (from answer in m_cardnetDAL.getCardOrderAnswers()
                                         where answer.PartnerId == request.PartnerId && answer.RequestId == request.RequestID && answer.RequestStatus == 1
                                         select answer.ChangeDateTime).FirstOrDefault();

            receivedStatusDate = pomReceivedStatusDate == null ? "todo" : pomReceivedStatusDate.ToString();

            var pomDeclinedStatusDate = (from answer in m_cardnetDAL.getCardOrderAnswers()
                                         where answer.PartnerId == request.PartnerId && answer.RequestId == request.RequestID && answer.RequestStatus == 3
                                         select answer.ChangeDateTime).FirstOrDefault();

            declinedStatusDate = pomDeclinedStatusDate == null ? "todo" : pomDeclinedStatusDate.ToString();

            var pomSendStatusDate = (from answer in m_cardnetDAL.getCardOrderAnswers()
                                     where answer.PartnerId == request.PartnerId && answer.RequestId == request.RequestID && answer.RequestStatus == 2
                                     select answer.ChangeDateTime).FirstOrDefault();

            sentStatusDate = pomSendStatusDate == null ? "todo" : pomSendStatusDate.ToString();

            // TODO cancel
            var pomSuccessfullyCanceledStatusDate = (from answer in m_cardnetDAL.getCardOrderAnswers()
                                     where answer.PartnerId == request.PartnerId && answer.RequestId == request.RequestID && answer.RequestStatus == 5
                                     select answer.ChangeDateTime).FirstOrDefault();

            successfullyCanceledStatusDate = pomSuccessfullyCanceledStatusDate == null ? "todo" : pomSuccessfullyCanceledStatusDate.ToString();

            var pomUnsuccessfullyCanceledStatusDate = (from answer in m_cardnetDAL.getCardOrderAnswers()
                                         where answer.PartnerId == request.PartnerId && answer.RequestId == request.RequestID && answer.RequestStatus == 6
                                         select answer.ChangeDateTime).FirstOrDefault();

            unsuccessfullyCanceledStatusDate = pomUnsuccessfullyCanceledStatusDate == null ? "todo" : pomUnsuccessfullyCanceledStatusDate.ToString();

            pomRowColor = "white";
            if (request.RequestStatus == 1) // card order is "RECEIVED"
            {
                pomRowColor = "orange";
            }
            else if (request.RequestStatus == 2) // card order is "SENT"
            {
                pomRowColor = "#99ff99"; // 
            }
            else if (request.RequestStatus == 3) // card order is "DECLINED"
            {
                //cardOrderVM.RowColor = "#ffccb3";
                pomRowColor = "#ff6666";
            }
            else if (request.RequestStatus == 4) // card order is "VALID"
            {
                pomRowColor = "#b3e0ff";
            }
            else if (request.RequestStatus == 5) // card order is "CANCELED"
            {
                pomRowColor = "yellow"; 
            }
            else if (request.RequestStatus == 6) // card order CANCEL request is DECLINED
            {
                pomRowColor = "#ff6666"; // same as DECLINED
            }
        }

        public void markAsSent(long id)
        {
            var cardOrderRequest = (from item in m_cardnetDAL.getCardOrderRequests() where item.ID == id select item).First();
            initWebConfigFileMembers();


            var pomRequestId = cardOrderRequest.RequestID;
            var pomPartnerId = cardOrderRequest.PartnerId;

            // 1.
            // SELECT code
            // FROM m_partners
            // WHERE id == pomPartnerId
            var pomPartnerCode = m_cardnetDAL.getPartners().Where(x => x.ID == pomPartnerId).Select(x => x.Code).First();

            // 2.
            // var pomTableName = ANS+code+002
            // INSERT INTO pomTableName (request_id, change_time, request_status)
            // VALUES (pomRequestId, DateTime.Now, 2)

            var pomTableName = "[dbo].[ANS" + pomPartnerCode.ToUpper() + "002]";
            var pomDatetime = DateTime.Now;
            string queryString = String.Format("INSERT INTO {0} (request_id, request_status) VALUES ({1}, 2)",pomTableName, pomRequestId);
            string connectionString = "Server="+m_cnServerIP+";Database="+m_cnDbName+";User Id="+m_cnDbUsername+";Password="+m_cnDbUserpassword+";";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@pomRequestId", pomRequestId);
                command.Parameters.AddWithValue("@pomDatetime", pomDatetime);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public string getEnvironment() { return m_environment; }
        public string getSambaPath() { return m_sambaPath; }

        /// <summary>
        /// na zaklade @id ziskat XML z REQ002 a uloz ho do daneho priecinka
        /// </summary>
        /// <param name="id">Primary Key from table REQ002</param>
        public void exportXML(long id)
        {
            var xmlStr = (from item in m_cardnetDAL.getCardOrderRequests() where item.ID == id select item.Content).First();
            var partnerId = (from item in m_cardnetDAL.getCardOrderRequests() where item.ID == id select item.PartnerId).First();
            var cardOrderId = id;

            XElement xml = XElement.Parse(xmlStr);

            string partnerName = (from partner in m_cardnetDAL.getPartners() where partner.ID == partnerId select partner.OrganizationName).First();

            var pathStringNew = @m_sambaPath + @m_environment + @"\PARTNERS\" + partnerName + @"\" + "CARD_ORDERS" + @"\" + cardOrderId + @"\XML\Exported_from_Web_Portal\";

            if (!Directory.Exists(pathStringNew))
                Directory.CreateDirectory(pathStringNew);

            var envPom = m_environment.Substring(5).ToLower();
            xml.Save(pathStringNew + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + partnerName + "_" + id + "_" + envPom + ".xml");
            m_destinationPath = pathStringNew + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + partnerName + "_" + id + "_" + envPom + ".xml";
        }

        public CardOrderViewModel createAndReturnNewCardOrderViewModel(short? cardTypeId, string content, DateTime? creationDateTime, long id, string message, int? partnerId, string partnerRequestId,
                                                                        long? requestId, int? cardCount, string carrierTypeId1, string envelopeTypeId, string plasticTypeId, string attachementId, string packageAttachementId, int? requestStatus, DateTime? lastUpdate, string slaDeadLine, bool? statusFinal, string validTo,
                                                                        bool? isProcessing, string validStatusDate, string receivedStatusDate, string declinedStatusDate, string sentStatusDate, string successfullyCanceledStatusDate, string unsuccessfullyCanceledStatusDate, string rowColor, string orderType, string deliveryListId)
        {
            CardOrderViewModel covm = new CardOrderViewModel();

            covm.CardTypeId = cardTypeId;

            var pomCardType = rwEngine.getRewardoDAL().getCardProductsFromRW().Where(ct => ct.CardProductID == cardTypeId).Select(ct => ct.CardProductName).FirstOrDefault();

            covm.CardTypeName = pomCardType == null ? "DOESN'T EXIST (" + cardTypeId.ToString() + ")" : pomCardType;
            covm.Content = content;
            covm.CreationDateTime = creationDateTime;
            covm.ID = id;
            var pom1 = (from item1 in m_cardnetDAL.getCardOrderAnswers() where item1.PartnerId == partnerId && item1.RequestId == requestId select item1.Message).LastOrDefault();
            covm.Message = pom1 == null ? "" : pom1.ToString();
            covm.PartnerId = partnerId;
            covm.PartnerName = m_cardnetDAL.getPartners().Where(p=>p.ID == partnerId).Select(p=>p.OrganizationName).First(); // TODO
            covm.PartnerRequestId = partnerRequestId;
            covm.RequestID = requestId;
            covm.CardCount = cardCount;

            covm.CarrierTypeId = carrierTypeId1 == null ? "NULL" : m_cardnetDAL.getConstantPartner().Where(c => c.PartnerId == partnerId && c.ConstantName == "CarrierTypeId" && c.ConstantValue == carrierTypeId1).Select(c => c.ConstantDescription).First(); // TODO
            covm.EnvelopeTypeId = envelopeTypeId == null ? "NULL" : m_cardnetDAL.getConstantPartner().Where(c => c.PartnerId == partnerId && c.ConstantName == "EnvelopeTypeId" && c.ConstantValue == envelopeTypeId).Select(c => c.ConstantDescription).First(); // TODO;
            covm.PlasticTypeId = plasticTypeId == null ? "NULL" : m_cardnetDAL.getConstantPartner().Where(c => c.PartnerId == partnerId && c.ConstantName == "PlasticTypeId" && c.ConstantValue == plasticTypeId).Select(c => c.ConstantDescription).First(); // TODO;
            covm.AttachementId = attachementId == null ? "NULL" : m_cardnetDAL.getConstantPartner().Where(c => c.PartnerId == partnerId && c.ConstantName == "AttachementId" && c.ConstantValue == attachementId).Select(c => c.ConstantDescription).First(); // TODO;
            covm.PackageAttachementId = packageAttachementId == null ? "NULL" : m_cardnetDAL.getConstantPartner().Where(c => c.PartnerId == partnerId && c.ConstantName == "PackageAttachementId" && c.ConstantValue == packageAttachementId).Select(c => c.ConstantDescription).First(); // TODO;

            covm.OrderType = orderType.Substring(0, 3) == "CAN" ? partnerRequestId : "normal";
            covm.RequestStatus = requestStatus;
            covm.RequestStatusName = requestStatus == null ? "INSERTED" : m_cardnetDAL.getConstantUniversals().Where(c=>c.ConstantName == "card_order_state" && c.ConstantValue == requestStatus).Select(c=>c.ConstantDescription).First(); // TODO
            covm.LastUpdate = lastUpdate;
            covm.SlaDeadLine = slaDeadLine;
            covm.StatusFinal = statusFinal;
            covm.ValidTo = validTo;
            covm.isProcessing = true;
            covm.TriggerOnLoad = false;
            covm.TriggerOnLoadMessage = "";

            var pomValidStatusDate = (from answer in m_cardnetDAL.getCardOrderAnswers()
                                      where answer.PartnerId == partnerId && answer.RequestId == requestId && answer.RequestStatus == 4
                                      select answer.ChangeDateTime).FirstOrDefault();

            covm.ValidStatusDate = pomValidStatusDate == null ? "todo" : pomValidStatusDate.ToString();


            var pomReceivedStatusDate = (from answer in m_cardnetDAL.getCardOrderAnswers()
                                         where answer.PartnerId == partnerId && answer.RequestId == requestId && answer.RequestStatus == 1
                                         select answer.ChangeDateTime).FirstOrDefault();

            covm.ReceivedStatusDate = pomReceivedStatusDate == null ? "todo" : pomReceivedStatusDate.ToString();

            var pomDeclinedStatusDate = (from answer in m_cardnetDAL.getCardOrderAnswers()
                                         where answer.PartnerId == partnerId && answer.RequestId == requestId && answer.RequestStatus == 3
                                         select answer.ChangeDateTime).FirstOrDefault();

            covm.DeclinedStatusDate = pomDeclinedStatusDate == null ? "todo" : pomDeclinedStatusDate.ToString();

            var pomSendStatusDate = (from answer in m_cardnetDAL.getCardOrderAnswers()
                                     where answer.PartnerId == partnerId && answer.RequestId == requestId && answer.RequestStatus == 2
                                     select answer.ChangeDateTime).FirstOrDefault();

            covm.SentStatusDate = pomSendStatusDate == null ? "todo" : pomSendStatusDate.ToString();

            // TODO cancel
            var pomsuccessfullyCanceledStatusDate = (from answer in m_cardnetDAL.getCardOrderAnswers()
                                     where answer.PartnerId == partnerId && answer.RequestId == requestId && answer.RequestStatus == 5
                                     select answer.ChangeDateTime).FirstOrDefault();

            covm.CanceledStatusDate = pomsuccessfullyCanceledStatusDate == null ? "todo" : pomsuccessfullyCanceledStatusDate.ToString();

            var pomunsuccessfullyCanceledStatusDate = (from answer in m_cardnetDAL.getCardOrderAnswers()
                                         where answer.PartnerId == partnerId && answer.RequestId == requestId && answer.RequestStatus == 6
                                         select answer.ChangeDateTime).FirstOrDefault();

            covm.CancelDeclinedStatusDate = pomunsuccessfullyCanceledStatusDate == null ? "todo" : pomunsuccessfullyCanceledStatusDate.ToString();

            covm.RowColor = rowColor;

            if (orderType.Substring(0, 3) != "CAN" && covm.RequestStatusName == "CANCEL RECEIVED")
            {
                covm.RequestStatusName = "CANCELED";
            }

            covm.DeliveryListId = deliveryListId == null ? "NULL" : m_cardnetDAL.getConstantPartner().Where(c=>c.PartnerId == partnerId && c.ConstantName == "DeliveryListId" && c.ConstantValue == deliveryListId).Select(c=>c.ConstantDescription).First();

            return covm;
        }

        public CardOrderViewModel getViewModelById(CardOrderListViewModel colvm, long id)
        {
            var pomVm = (from item in colvm.colVm where item.ID == id select item).FirstOrDefault();
            return pomVm;
        }

        public void initWebConfigFileMembers()
        {
            m_sambaPath = System.Web.Configuration.WebConfigurationManager.AppSettings["mySambaPath"];
            m_environment = System.Web.Configuration.WebConfigurationManager.AppSettings["myEnvironment"];

            var pomCn = new EntityConnectionStringBuilder(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["test1Entities"].ConnectionString).ProviderConnectionString;
            var cardnetConnString = new SqlConnectionStringBuilder(pomCn);

            m_cnServerIP =        cardnetConnString.DataSource;
            m_cnDbName =          cardnetConnString.InitialCatalog;
            m_cnDbUsername =      cardnetConnString.UserID;
            m_cnDbUserpassword =  cardnetConnString.Password;

            var pomRw = new EntityConnectionStringBuilder(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["REWARDO_CARDNETEntities"].ConnectionString).ProviderConnectionString;
            var rewardoConnString = new SqlConnectionStringBuilder(pomRw);

            m_rwServerIP =          rewardoConnString.DataSource;
            m_rwDbName =            rewardoConnString.InitialCatalog;
            m_rwDbUsername =        rewardoConnString.UserID;
            m_rwDbUserpassword =    rewardoConnString.Password;
        }

        public RewardoEngine getRewardoEngine() { return rwEngine; }
        
        public CardnetDAL getCardnetDAL() { return m_cardnetDAL; }

        public string getDestinationPath() { return m_destinationPath; }

        public string getCnServerIP() { return m_cnServerIP; }
        public string getCnDbName() { return m_cnDbName; }
        public string getCnUserName() { return m_cnDbUsername; }
        public string getCnUserPassword() { return m_cnDbUserpassword; }

        public string getRwServerIP() { return  m_rwServerIP; }
        public string getRwDbName() { return    m_rwDbName; }
        public string getRwUserName() { return  m_rwDbUsername; }
        public string getRwUserPassword() { return  m_rwDbUserpassword; }
    }
}