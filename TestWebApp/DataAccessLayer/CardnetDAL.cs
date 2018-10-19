using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestWebApp.Models;
using System.Xml;
using System.Xml.Linq;
using TestWebApp.ViewModel;
using System.Data.SqlClient;

namespace TestWebApp.DataAccessLayer
{
    public class CardnetDAL
    {
        private test1Entities m_cardnetDB; // test1 DB

        // POS ACCEPTANCE
        private List<PosAcceptanceRequest> m_req001;
        private List<PosAcceptanceAnswer> m_ans001;

        // CARD ORDER
        private List<CardOrderRequest> m_req002;
        private List<CardOrderAnswer> m_ans002;

        // CREDIT ORDER
        private List<CreditOrderRequest> m_req003;
        private List<CreditOrderAnswer> m_ans003;

        // CARD STATUS UPDATE
        private List<CardStatusUpdateRequest> m_req004;
        private List<CardStatusUpdateAnswer> m_ans004;

        // CARD TRANSFER
        private List<CardTransferRequest> m_req008;
        private List<CardTransferAnswer> m_ans008;

        // CONSTANT_UNIVERSAL
        private List<ConstantUniversal> m_constantUniversal;

        // CONSTANT_UNIVERSAL
        private List<ConstantPartner> m_constantPartner;

        private List<Partner> m_partners;

        /// <summary>
        /// Constructor
        /// </summary>
        public CardnetDAL()
        {
            m_cardnetDB = new test1Entities(); // DB is now accessible
        }

        public void initializeDbTable(string item)
        {
            if (item == "partners") { retrievePartners(); }
            else 
            if (item == "constantUniversals") { retrieveConstantUniversal(); }
            else
            if (item == "constantPartners") { retrieveConstantPartner(); }
            

            retrieveRequests(item);
            retrieveAnswers(item);
        }

        // returns order's partner name
        // PARTNER
        public string getPartnerNameByOrderId(long id)
        {
            var partnerId = (from item in m_req002 where item.ID == id select item.PartnerId).FirstOrDefault();
            if (m_partners == null)
            {
                retrievePartners(); 
            }
            var partnerName = (from item in m_partners where item.ID == partnerId select item.OrganizationName).FirstOrDefault();
            return partnerName;
        }

        private void createPosAcceptanceRequestEntity(C____REQ001 item)
        {
            PosAcceptanceRequest pom = new PosAcceptanceRequest();

            pom.AnswerDateTime = item.answer_datetime;
            pom.CreationDateTime = item.creation_datetime;
            pom.ExternalTerminalId = item.external_terminal_id;
            pom.ID = item.id;
            pom.NewAcceptState = item.new_accept_state == true ? "ACTIVE" : "PASSIVE";
            pom.PartnerName = m_partners.Where(p => p.ID == item.partner_id).Select(p => p.OrganizationName).First()/* item.partner_id*/;
            pom.PartnerRequestId = item.partner_request_id;
            pom.RequestId = item.request_id;
            pom.RequestStatus = item.request_status == true ? "SUCCEEDED" : "DECLINED";
            pom.TerminalTypeId = item.terminal_type_id;

            m_req001.Add(pom);
        }

        private void createCardOrderRequestEntity(C____REQ002 item)
        {
            CardOrderRequest pomCor = new CardOrderRequest();

            var pomSlaDeadLine = "";
            if (item.request_status == 3)
            {
                pomSlaDeadLine = "";
            }
            else if (item.request_status == 1 || item.request_status == 2)
            {
                pomSlaDeadLine = item.sla_deadline == null ? "todo" : item.sla_deadline.Value.Date.ToString("dd.MM.yyyy");
            }
            else if (item.request_status == null || item.request_status == 4)
            {
                pomSlaDeadLine = "SLA NOT SET YET";
            }

            pomCor.CardTypeId = item.card_type_id;
            pomCor.Content = item.content;
            pomCor.CardCount = item.card_count;

            pomCor.CarrierTypeId = item.carrier_type_id;
            pomCor.EnvelopeTypeId = item.envelope_type_id;
            pomCor.PlasticTypeId = item.plastic_type_id;
            pomCor.AttachementId = item.attachement_id;
            pomCor.PackageAttachementId = item.package_attachement_id;

            pomCor.ValidTo = item.valid_to.Value.Date.ToString("dd.MM.yyyy");
            pomCor.CreationDateTime = item.creation_datetime;
            pomCor.ID = item.id;
            pomCor.LastUpdate = item.last_update;
            pomCor.PartnerId = item.partner_id;
            pomCor.PartnerRequestId = item.partner_request_id;
            pomCor.RequestID = item.request_id;
            pomCor.RequestStatus = item.request_status;
            pomCor.StatusFinal = item.status_final;
            pomCor.SlaDeadLine = pomSlaDeadLine;
            pomCor.IsProcessing = item.is_processing;

            pomCor.OrderType = item.partner_request_id;
            pomCor.DeliveryListId = item.delivery_list_id;

            m_req002.Add(pomCor);
        }

        private void createCreditOrderRequestEntity(C____REQ003 item)
        {
            CreditOrderRequest pom = new CreditOrderRequest();

            pom.AnswerDateTime = item.answer_datetime;
            pom.CreationDateTime = item.creation_datetime;
            pom.ExternalCardId = item.external_card_id;
            pom.ID = item.id;
            pom.PartnerName = m_partners.Where(p => p.ID == item.partner_id).Select(p => p.OrganizationName).First()/* item.partner_id*/;
            pom.PartnerRequestId = item.partner_request_id;
            pom.RequestId = item.request_id;
            pom.RequestStatus = item.request_status == true ? "SUCCEEDED" : "DECLINED";
            pom.TransactionAmount = item.transaction_amount;
            pom.TransactionType = m_constantUniversal.Where(cst => cst.ConstantName == "transaction_type" && cst.ConstantValue == item.transaction_type).Select(cst => cst.ConstantDescription).First();

            m_req003.Add(pom);
        }

        private void createCardStatusUpdateRequestEntity(C____REQ004 item)
        {
            CardStatusUpdateRequest pom = new CardStatusUpdateRequest();

            pom.AnswerDateTime = item.answer_datetime;
            pom.CreationDateTime = item.creation_datetime;
            pom.DailyLimit = item.daily_limit;
            pom.ExternalCardId = item.external_card_id;
            pom.ID = item.id;
            pom.PartnerName = m_partners.Where(p => p.ID == item.partner_id).Select(p => p.OrganizationName).First()/* item.partner_id*/;
            pom.PartnerRequestId = item.partner_request_id;
            //pom.RequestedStateId = item.request_status == true ? "SUCCEEDED" : "DECLINED";
            pom.RequestId = item.request_id;
            pom.RequestStatus = item.request_status == true ? "SUCCEEDED" : "DECLINED";
            pom.TransactionLimit = item.transaction_limit;

            m_req004.Add(pom);
        }

        private void createCardTransferRequestEntity(C____REQ008 item)
        {

            long? ID = null;
            long? RequestId = null;
            int? PartnerId = null;
            string PartnerName = null;
            string PartnerRequestId = null;
            string TransferType = null;
            string Content = null;
            string TransferCase = null;
            string BalanceTransfer = null;
            string BonusTransfer = null;
            string CardnoFromFinalStatus = null;
            long? CardnoFrom = null;
            long? CardnoTo = null;
            DateTime? CreationDateTime = null;
            DateTime? LastUpdateDate = null;
            string RequestStatus = null;
            byte? Succesfull = null;
            bool? IsProcessing = null;
            DateTime? SlaDeadline = null;

            CardTransferRequest pom = new CardTransferRequest();

            try
            {

                pom.BalanceTransfer = item.BALANCE_TRANSFER == true ? "YES" : "NO";
                pom.BonusTransfer = item.BONUS_TRANSFER == true ? "YES" : "NO";
                pom.CardnoFrom = item.CARDNO_FROM;
                pom.CardnoFromFinalStatus = getConstantUniversals().Where(c => c.ConstantName == "cardno_from_final_status" && c.ConstantValue == item.CARDNO_FROM_FINAL_STATUS).Select(c => c.ConstantDescription).FirstOrDefault();
                pom.CardnoTo = item.CARDNO_TO;
                pom.Content = item.CONTENT;
                pom.CreationDateTime = item.CREATION_DATETIME;
                pom.ID = item.ID;
                pom.IsProcessing = item.IS_PROCESSING;
                pom.LastUpdateDate = item.LAST_UPDATE;
                pom.PartnerId = item.PARTNER_ID;
                pom.PartnerName = getPartners().Where(p => p.ID == item.PARTNER_ID).Select(p => p.OrganizationName).First();
                pom.PartnerRequestId = item.PARTNER_REQUEST_ID;
                pom.RequestId = item.REQUEST_ID;

                var pomRequestStatus = "";
                if (item.REQUEST_STATUS == 1)
                    pomRequestStatus = "SUCCEEDED";
                else if (item.REQUEST_STATUS == 0)
                    pomRequestStatus = "DECLINED";
                else
                    pomRequestStatus = "null"; // nemalo by sa stat nikdy

                pom.RequestStatus = pomRequestStatus;
                pom.SlaDeadline = item.SLA_DEADLINE;
                pom.Succesfull = item.SUCCESSFULL;
                pom.TransferCase = getConstantUniversals().Where(c => c.ConstantName == "transfer_case" && c.ConstantValue == item.TRANSFER_CASE).Select(c => c.ConstantDescription).First();
                pom.TransferType = getConstantUniversals().Where(c => c.ConstantName == "transfer_type" && c.ConstantValue == item.TRANSFER_TYPE).Select(c => c.AdditionalValue).First();

                //// ---------
                //ID = item.ID;
                //RequestId = null;
                //PartnerId = null;
                //PartnerName = null;
                //PartnerRequestId = null;
                //TransferType = null;
                //Content = null;
                //TransferCase = null;
                //BalanceTransfer = null;
                //BonusTransfer = null;
                //CardnoFromFinalStatus = null;
                //CardnoFrom = null;
                //CardnoTo = null;
                //CreationDateTime = null;
                //LastUpdateDate = null;
                //RequestStatus = null;
                //Succesfull = null;
                //IsProcessing = null;
                //SlaDeadline = null;

                //// ---------

                m_req008.Add(pom);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void createPosAcceptanceAnswerEntity(C____ANS001 item)
        {
            PosAcceptanceAnswer pom = new PosAcceptanceAnswer();

            pom.AnswerId = item.ans_id;
            pom.ID = item.id;
            pom.InsertDateTime = item.change_time;
            pom.PartnerName = m_partners.Where(p => p.ID == item.partner_id).Select(p => p.OrganizationName).First();
            pom.RequestId = item.request_id;
            pom.RequestMessage = item.request_message;
            pom.RequestStatus = item.request_status == true ? "SUCCEEDED" : "DECLINED";
            pom.StatusAfterRequest = item.status_after_request == 1 ? "ACTIVE" : "PASSIVE";

            m_ans001.Add(pom);
        }

        private void createCardOrderAnswerEntity(C____ANS002 item)
        {
            PosAcceptanceAnswer pom = new PosAcceptanceAnswer();

            CardOrderAnswer pomCoa = new CardOrderAnswer();

            pomCoa.AnswerId = item.answer_id;
            pomCoa.ChangeDateTime = item.change_time;
            pomCoa.ID = item.id;
            pomCoa.InsertDateTime = item.insert_time;

            pomCoa.Message = item.message;

            pomCoa.PartnerId = item.partner_id;
            pomCoa.RequestId = item.request_id;
            pomCoa.RequestStatus = item.request_status;
            pomCoa.Additional_column1 = item.additional_column1;

            m_ans002.Add(pomCoa);
        }

        private void createCreditOrderAnswer(C____ANS003 item)
        {
            CreditOrderAnswer pom = new CreditOrderAnswer();

            pom.AnswerId = item.ans_id;
            pom.ChangeTime = item.change_time;
            pom.ID = item.id;
            pom.PartnerName = m_partners.Where(p => p.ID == item.partner_id).Select(p => p.OrganizationName).First()/* item.partner_id*/;
            pom.RequestId = item.request_id;
            pom.RequestMessage = item.request_message;
            pom.RequestStatus = item.request_status == true ? "SUCCEEDED" : "DECLINED";
            pom.TransactionBalance = item.transaction_balance;
            pom.TransactionFinal = item.transaction_final;
            pom.TransactionId = item.transaction_id;

            m_ans003.Add(pom);
        }

        private void createCardStatusUpdateAnswer(C____ANS004 item)
        {
            CardStatusUpdateAnswer pom = new CardStatusUpdateAnswer();

            pom.AnswerId = item.ans_id;
            pom.ID = item.id;
            pom.ChangeTime = item.change_time;
            pom.PartnerName = m_partners.Where(p => p.ID == item.partner_id).Select(p => p.OrganizationName).First()/* item.partner_id*/;
            pom.RequestId = item.request_id;
            pom.RequestMessage = item.request_message;
            pom.RequestStatus = item.request_status == true ? "SUCCEEDED" : "DECLINED";

            m_ans004.Add(pom);
        }

        private void createCardTransferAnswer(C____ANS008 item)
        {
            CardTransferAnswer pom = new CardTransferAnswer();

            pom.AnswerId = item.ANS_ID;
            pom.AuthorizationIdFromCard = item.AUTHORIZATION_ID_FROM_CARD;
            pom.AuthorizationIdToCard = item.AUTHORIZATION_ID_TO_CARD;
            pom.ChangeTime = item.CHANGE_TIME;
            pom.CurrentBalanceFromCard = item.CURRENT_BALANCE_FROM_CARD;
            pom.CurrentBalanceToCard = item.CURRENT_BALANCE_TO_CARD;
            pom.ID = item.ID;
            pom.Message = item.MESSAGE;
            pom.PartnerId = item.PARTNER_ID;
            pom.RequestId = item.REQUEST_ID;
            pom.RequestMessage = item.REQUEST_MESSAGE;
            pom.RequestStatus = item.REQUEST_STATUS == 1 ? "SUCCEEDED" : "DECLINED";

            m_ans008.Add(pom);
        }

        public void retrieveRequests(string whichOne)
        {
            if (m_partners == null)
            {
                retrievePartners();
            }

            if (m_constantUniversal == null)
            {
                retrieveConstantUniversal();
            }

            if (whichOne == "all")
            {
                // retrieve pos acceptance requests from db into @m_req001
                var req001s = m_cardnetDB.C____REQ001.OrderByDescending(o => o.id).ToList();
                m_req001 = new List<PosAcceptanceRequest>();
                foreach (var item in req001s)
                {
                    createPosAcceptanceRequestEntity(item);
                }

                // retrieve card order requests from db into @m_req002
                var pomList = m_cardnetDB.C____REQ002.OrderByDescending(o => o.id).ToList();
                m_req002 = new List<CardOrderRequest>();
                foreach (var item in pomList)
                {
                    createCardOrderRequestEntity(item);
                }

                // retrieve credit order requests from db into @m_req003
                var req003s = m_cardnetDB.C____REQ003.OrderByDescending(o => o.id).ToList();
                m_req003 = new List<CreditOrderRequest>();
                foreach (var item in req003s)
                {
                    createCreditOrderRequestEntity(item);
                }

                // retrieve card status update requests from db into @m_req004
                var req004s = m_cardnetDB.C____REQ004.OrderByDescending(o => o.id).ToList();
                m_req004 = new List<CardStatusUpdateRequest>();
                foreach (var item in req004s)
                {
                    createCardStatusUpdateRequestEntity(item);
                }
            }
            else if (whichOne == "PosAcceptanceRequests")
            {
                // retrieve pos acceptance requests from db into @m_req001
                var req001s = m_cardnetDB.C____REQ001.OrderByDescending(o => o.id).ToList();
                m_req001 = new List<PosAcceptanceRequest>();
                foreach (var item in req001s)
                {
                    createPosAcceptanceRequestEntity(item);
                }
            }
            else if (whichOne == "CardOrderRequests")
            {
                // retrieve card order requests from db into @m_req002
                var pomList = m_cardnetDB.C____REQ002.OrderByDescending(o => o.id).ToList();
                m_req002 = new List<CardOrderRequest>();
                foreach (var item in pomList)
                {
                    createCardOrderRequestEntity(item);
                }
            }
            else if (whichOne == "CreditOrderRequests")
            {
                // retrieve credit order requests from db into @m_req003
                var req003s = m_cardnetDB.C____REQ003.OrderByDescending(o => o.id).ToList();
                m_req003 = new List<CreditOrderRequest>();
                foreach (var item in req003s)
                {
                    createCreditOrderRequestEntity(item);
                }
            }
            else if (whichOne == "CardStatusUpdateRequests")
            {
                // retrieve card status update requests from db into @m_req004
                var req004s = m_cardnetDB.C____REQ004.OrderByDescending(o => o.id).ToList();
                m_req004 = new List<CardStatusUpdateRequest>();
                foreach (var item in req004s)
                {
                    createCardStatusUpdateRequestEntity(item);
                }
            }
            else if (whichOne == "CardTransferRequests")
            {
                // retrieve card status update requests from db into @m_req004
                var req008s = m_cardnetDB.C____REQ008.OrderByDescending(o => o.ID).ToList();
                m_req008 = new List<CardTransferRequest>();
                foreach (var item in req008s)
                {
                    createCardTransferRequestEntity(item);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="whichOne"></param>
        public void retrieveAnswers(string whichOne)
        {

            if (whichOne == "all")
            {
                // retrieve pos acceptance answers from db into @m_ans001
                var ans001s = m_cardnetDB.C____ANS001.ToList();
                m_ans001 = new List<PosAcceptanceAnswer>();
                foreach (var item in ans001s)
                {
                    createPosAcceptanceAnswerEntity(item);
                }

                // retrieve card order answers from db into @m_ans002
                var pomList = m_cardnetDB.C____ANS002.ToList();
                m_ans002 = new List<CardOrderAnswer>();
                foreach (var item in pomList)
                {
                    createCardOrderAnswerEntity(item);
                }

                // retrieve credit order answers from db into @m_ans003
                var ans003s = m_cardnetDB.C____ANS003.ToList();
                m_ans003 = new List<CreditOrderAnswer>();
                foreach (var item in ans003s)
                {
                    createCreditOrderAnswer(item);
                }

                // retrieve card status update answers from db into @m_ans004
                var ans004s = m_cardnetDB.C____ANS004.ToList();
                m_ans004 = new List<CardStatusUpdateAnswer>();
                foreach (var item in ans004s)
                {
                    createCardStatusUpdateAnswer(item);
                }
            }
            else if (whichOne == "PosAcceptanceAnswers")
            {
                // retrieve pos acceptance answers from db into @m_ans001
                var ans001s = m_cardnetDB.C____ANS001.ToList();
                m_ans001 = new List<PosAcceptanceAnswer>();
                foreach (var item in ans001s)
                {
                    createPosAcceptanceAnswerEntity(item);
                }
            }
            else if (whichOne == "CardOrderAnswers")
            {
                // retrieve card order answers from db into @m_ans002
                var pomList = m_cardnetDB.C____ANS002.ToList();
                m_ans002 = new List<CardOrderAnswer>();
                foreach (var item in pomList)
                {
                    createCardOrderAnswerEntity(item);
                }
            }
            else if (whichOne == "CreditOrderAnswers")
            {
                // retrieve credit order answers from db into @m_ans003
                var ans003s = m_cardnetDB.C____ANS003.ToList();
                m_ans003 = new List<CreditOrderAnswer>();
                foreach (var item in ans003s)
                {
                    createCreditOrderAnswer(item);
                }
            }
            else if (whichOne == "CardStatusUpdateAnswers")
            {
                // retrieve card status update answers from db into @m_ans004
                var ans004s = m_cardnetDB.C____ANS004.ToList();
                m_ans004 = new List<CardStatusUpdateAnswer>();
                foreach (var item in ans004s)
                {
                    createCardStatusUpdateAnswer(item);
                }
            }
            else if (whichOne == "CardTransferAnswers")
            {
                // retrieve card status update answers from db into @m_ans004
                var ans008s = m_cardnetDB.C____ANS008.ToList();
                m_ans008 = new List<CardTransferAnswer>();
                foreach (var item in ans008s)
                {
                    createCardTransferAnswer(item);
                }
            }
        }

        public void retrieveConstantPartners()
        {
            var pomList = m_cardnetDB.C___CONSTANT_PARTNER.ToList();
            m_constantPartner = new List<ConstantPartner>();

            if (m_partners == null)
            {
                retrievePartners();
            }

            foreach (var constant in pomList)
            {
                ConstantPartner constP = new ConstantPartner();

                constP.ConstantDescription = constant.constant_description;
                constP.ConstantName = constant.constant_name;
                constP.ConstantValue = constant.constant_value;
                constP.Id = constant.id;
                constP.Inserted = constant.insert_date;
                constP.LastUpdated = constant.last_update_date;
                constP.PartnerName = m_partners.Where(p => p.ID == constant.partner_id).Select(p => p.OrganizationName).First();

                m_constantPartner.Add(constP);
            }
        }

        // private?
        public void retrievePartners()
        {
            // retrieve card order answers from db into @m_ans002
            var pomList = m_cardnetDB.C___PARTNER.ToList();
            m_partners = new List<Partner>();

            foreach (var partner in pomList)
            {
                Partner pomPartner = new Partner();

                pomPartner.Code = partner.code;
                pomPartner.CreationDateTime = partner.creation_datetime;
                pomPartner.ID = partner.id;
                pomPartner.OrganizationName = partner.organization_name;
                pomPartner.PartnerTypeID = partner.partner_type_id;
                pomPartner.RwOrganizationId = Convert.ToInt32(partner.rw_id);

                m_partners.Add(pomPartner);
            }
        }

        public void retrieveConstantUniversal()
        {
            var pomList = m_cardnetDB.C___CONSTANT_UNIVERSAL.ToList();
            m_constantUniversal = new List<ConstantUniversal>();

            foreach (var item in pomList)
            {
                ConstantUniversal pomConst = new ConstantUniversal();

                pomConst.AdditionalValue = item.additional_value1;
                pomConst.ConstantDescription = item.constant_description;
                pomConst.ConstantName = item.constant_name;
                pomConst.ConstantValue = item.constant_value;
                pomConst.Id = item.id;
                pomConst.InsertDateTime = item.insert_date;
                pomConst.LastUpdateDate = item.last_update_date;

                m_constantUniversal.Add(pomConst);
            }
        }

        public void retrieveConstantPartner()
        {
            var pomList = m_cardnetDB.C___CONSTANT_PARTNER.ToList();
            m_constantPartner = new List<ConstantPartner>();

            foreach (var item in pomList)
            {
                ConstantPartner pomConst = new ConstantPartner();

                pomConst.ConstantDescription = item.constant_description;
                pomConst.ConstantName = item.constant_name;
                pomConst.ConstantValue = item.constant_value;
                pomConst.Id = item.id;
                pomConst.Inserted = item.insert_date;
                pomConst.LastUpdated = item.last_update_date;
                pomConst.PartnerId= item.partner_id;
                // TOOD pomConst.PartnerName = ?? // necessary???

                m_constantPartner.Add(pomConst);
            }
        }

        public XElement getXMLByOrderId(long id)
        {
            var pomString = (from item in m_req002 where item.ID == id select item.Content).FirstOrDefault();
            var pomXElement = XElement.Parse(pomString);
            return pomXElement;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="packageId"></param>
        /// <returns></returns>
        public List<long> getCardsFromXMLByPartnerAndPackageIdFromCN(long id, uint packageId)
        {
            // result variable
            var pomCardIds = new List<long>();

            var pomXMLString = (from item in m_req002 where item.ID == id select item.Content).FirstOrDefault();
            if (pomXMLString != null || pomXMLString != "")
            {
                // get all card ids from package with id @packageId
                var pomXML = XElement.Parse(pomXMLString); // now I have an regular XML file in memory

                foreach (XElement item in pomXML.Elements("card"))
                    if (Convert.ToUInt32(item.Element("package_id").Value) == packageId)
                        pomCardIds.Add(Convert.ToUInt32(item.Element("card_id").Value));
            }
            return pomCardIds;
        }

        /// <summary>
        /// Retrieves TablesViewModel
        /// </summary>
        /// <returns></returns>
        public /*List<Tuple<string,string>*/void retrieveOperationTypes(TablesViewModel tblViewModel)
        {
            // retrieve constant unversal data from db into @m_constantUniversal
            var constantUniversal = m_cardnetDB.C___CONSTANT_UNIVERSAL.Where(o => o.constant_name == "operation_type").OrderByDescending(o => o.insert_date).ToList();
            var pom1 = new List<Tuple<string,string>>();
            var pomD = new Dictionary<string, Dictionary<string,string>>();

            int todaysWaitings;
            int allWaitings;
            int todaysProcessed;
            DateTime? lastProcessedDateTime; //= new DateTime();

            foreach (var item in constantUniversal)
            {
                todaysWaitings = 0;
                allWaitings = 0;
                todaysProcessed = 0;
                lastProcessedDateTime = new DateTime();

                var pomInnerDict = new Dictionary<string, string>();
                pom1.Add(new Tuple<string,string>( item.constant_description, item.additional_value1));

                if (item.constant_description == "POS ACCEPTANCE")
                {
                    // get todays waitings
                    todaysWaitings = (from i in m_req001 where i.CreationDateTime >= DateTime.Today select i).Count() - (from j in m_ans001 where j.InsertDateTime >= DateTime.Today select j).Count();
                    allWaitings = m_req001.Count() - m_ans001.Count();
                    todaysProcessed = (from j in m_ans001 where j.InsertDateTime >= DateTime.Today select j).Count();
                    lastProcessedDateTime = m_ans001.Max(x => x.InsertDateTime);
                }
                else if (item.constant_description == "CARD ORDER")
                {
                    todaysWaitings = (from j in m_ans002 where j.InsertDateTime >= DateTime.Today && j.Additional_column1 == null && j.RequestStatus == 4 select j).Count();
                    allWaitings = (from j in m_ans002 where j.Additional_column1 == null && j.RequestStatus == 4 select j).Count();
                    todaysProcessed = (from j in m_ans002 where j.InsertDateTime >= DateTime.Today && j.RequestStatus == 1 select j).Count();
                    lastProcessedDateTime = m_ans002.Max(x => x.InsertDateTime);
                }
                else if (item.constant_description == "CREDIT ORDER")
                {
                    todaysWaitings = (from i in m_req003 where i.CreationDateTime >= DateTime.Today select i).Count() - (from j in m_ans003 where j.ChangeTime >= DateTime.Today select j).Count();
                    allWaitings = m_req003.Count() - m_ans003.Count();
                    todaysProcessed = (from j in m_ans003 where j.ChangeTime >= DateTime.Today select j).Count();
                    lastProcessedDateTime = m_ans003.Max(x => x.ChangeTime);
                }
                else if (item.constant_description == "CARD STATUS UPDATE")
                {
                    todaysWaitings = (from i in m_req004 where i.CreationDateTime >= DateTime.Today select i).Count() - (from j in m_ans004 where j.ChangeTime >= DateTime.Today select j).Count();
                    allWaitings = m_req004.Count() - m_ans004.Count();
                    todaysProcessed = (from j in m_ans004 where j.ChangeTime >= DateTime.Today select j).Count();
                    lastProcessedDateTime = m_ans004.Max(x => x.ChangeTime);
                }
                else if (item.constant_description == "CARD TRANSFER")
                {
                    // TENTO REQUEST MOZE MAT VIACERO ODPOVEDI, TAKZE VYPOCET JE TROCHU KOMPLIKOVANEJSI

                    var requests    = 0;
                    var answers     = 0;
                    var todayRequests    = 0;
                    var todayAnswers     = 0;

                    foreach (var p in m_partners)
                    {
                        var pomTodayRequestsCount = m_req008.Where(r => r.PartnerId == p.ID && r.CreationDateTime >= DateTime.Today).Select(r => r.RequestId).Count();
                        var pomTodayAnswersCount = m_ans008.Where(a => a.PartnerId == p.ID && a.ChangeTime >= DateTime.Today).Select(a => a.RequestId).Distinct().Count();

                        var pomRequestsCount = m_req008.Where(r => r.PartnerId == p.ID).Select(r => r.RequestId).Count();
                        var pomAnswersCount  = m_ans008.Where(a => a.PartnerId == p.ID).Select(a => a.RequestId).Distinct().Count();

                        requests += pomRequestsCount;
                        answers += pomAnswersCount;

                        todayRequests += pomTodayRequestsCount;
                        todayAnswers +=  pomTodayAnswersCount;
                    }


                    todaysWaitings = todayRequests - todayAnswers;//(from i in m_req008 where i.CreationDateTime >= DateTime.Today select i).Count() - (from j in m_ans008 where j.ChangeTime >= DateTime.Today select j).Count();
                    allWaitings = requests - answers;//m_req008.Count() - m_ans008.Count();
                    todaysProcessed = (from j in m_ans008 where j.ChangeTime >= DateTime.Today select j).Count();
                    lastProcessedDateTime = m_ans008.Max(x => x.ChangeTime);
                }

                pomInnerDict["TODAYSWAITING"] = todaysWaitings.ToString();
                pomInnerDict["ALLWAITING"] = allWaitings.ToString();
                pomInnerDict["TODAYSPROCESSED"] = todaysProcessed.ToString();
                pomInnerDict["LASTPROCESSEDDATETIME"] = lastProcessedDateTime.ToString() == "" ? "null" : lastProcessedDateTime.ToString();

                if (pomInnerDict.Any())
                {
                    pomD.Add(item.constant_description, new Dictionary<string, string>());
                    pomD[item.constant_description] = pomInnerDict;
                }
            }

            tblViewModel.CardnetTables = pom1;
            tblViewModel.CardnetTableColumns = pomD;
             
        }

        public List<CardOrderRequest> getCardOrdersByIsProcessingStatus(bool status)
        {
            var result = new List<CardOrderRequest>();

            result = m_req002.Where(x => x.IsProcessing == true).Select(x => x).ToList();

            return result;
        }

        public List<PosAcceptanceRequest> getPosAcceptanceRequests() { return m_req001; }
        public List<PosAcceptanceAnswer> getPosAcceptanceAnswers() { return m_ans001; }

        public List<CardOrderRequest> getCardOrderRequests() { return m_req002; }
        public List<CardOrderAnswer> getCardOrderAnswers() { return m_ans002; }

        public List<CreditOrderRequest> getCreditOrderRequests() { return m_req003; }
        public List<CreditOrderAnswer> getCreditOrderAnswers() { return m_ans003; }

        public List<CardStatusUpdateRequest> getCardStatusUpdateRequests() { return m_req004; }
        public List<CardStatusUpdateAnswer> getCardStatusUpdateAnswers() { return m_ans004; }

        public List<CardTransferRequest> getCardTransferRequests() { return m_req008; }
        public List<CardTransferAnswer> getCardTransferAnswers() { return m_ans008; }

        public List<Partner> getPartners() { return m_partners; }
        public List<ConstantUniversal> getConstantUniversals() { return m_constantUniversal; }
        public List<ConstantPartner> getConstantPartner() { return m_constantPartner; }

        public test1Entities getCardnetDB() { return m_cardnetDB; }
    }
}