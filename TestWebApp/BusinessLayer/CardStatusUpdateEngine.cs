using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestWebApp.ViewModel;
using TestWebApp.DataAccessLayer;
using TestWebApp.Models;

namespace TestWebApp.BusinessLayer
{
    public class CardStatusUpdateEngine : IReqAnsEngine<CardStatusUpdateRequest, CardStatusUpdateAnswer, CardStatusUpdateListViewModel>
    {

        private CardnetDAL m_cardnetDAL; // data access layer
        private RewardoDAL m_rewardoDAL; // data access layer

        private List<CardStatusUpdateRequest> m_req004;
        private List<CardStatusUpdateAnswer> m_ans004;

        public CardStatusUpdateEngine()
        {
            try
            {
                // RETRIEVE CREDIT ORDER DATA (REQ/ANS)
                m_cardnetDAL = new CardnetDAL();
                m_cardnetDAL.retrievePartners();
                m_cardnetDAL.retrieveConstantPartners();

                m_rewardoDAL = new RewardoDAL();

                retrieveRequests();
                retrieveAnswers();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private string getRequestedStateIDDescription(string partnerName, short? requestedStateId)
        {
            if (m_cardnetDAL.getConstantPartner() == null)
            {
                m_cardnetDAL.retrieveConstantPartners();
            }

            var pomPartnerCode = m_cardnetDAL.getPartners().Where(p => p.OrganizationName == partnerName).First().Code;
            var requestedStateIdConstantValue = m_cardnetDAL.getConstantPartner().Where(c => c.PartnerName == partnerName && c.ConstantName == "CardStatusUpdateRequestedStateId" && c.ConstantValue == pomPartnerCode + "_CSU" + requestedStateId).First().ConstantDescription;

            // z CONSTANT_PARTNER ziskaj na zaklade partnername a requestedStateid danu hodnotu
            return requestedStateIdConstantValue;
        } 

        /// <summary>
        /// Interface method
        /// </summary>
        public void retrieveRequests()
        {
            // retrieve card order requests from db into @m_req002
            var pomList = m_cardnetDAL.getCardnetDB().C____REQ004.OrderByDescending(o => o.creation_datetime).ToList();
            m_req004 = new List<CardStatusUpdateRequest>();

            foreach (var item in pomList)
            {
                CardStatusUpdateRequest csur = new CardStatusUpdateRequest();
                try
                {

                    csur.AnswerDateTime = item.answer_datetime;
                    csur.CreationDateTime = item.creation_datetime;
                    csur.DailyLimit = item.daily_limit;                      
                    csur.ExternalCardId = item.external_card_id;        
                    csur.ID = item.id;
                    csur.PartnerName = m_cardnetDAL.getPartners().Where(p => p.ID == item.partner_id).Select(p => p.OrganizationName).First()/* item.partner_id*/;
                    csur.PartnerRequestId = item.partner_request_id;    
                    csur.RequestId = item.request_id;

                    var pomRequestStatus = "";
                    if (item.request_status == true)
                        pomRequestStatus = "SUCCEEDED";
                    else if (item.request_status == false)
                        pomRequestStatus = "DECLINED";
                    else
                        pomRequestStatus = "null"; // nemalo by sa stat nikdy

                    csur.RequestStatus = pomRequestStatus;
                    csur.TransactionLimit = item.transaction_limit;

                    csur.RequestedStateId = getRequestedStateIDDescription(csur.PartnerName, item.requested_state_id);
                    csur.AccountTypeId = item.ACCOUNT_TYPE_ID;

                    var pom = m_cardnetDAL.getConstantPartner().Where(p => p.PartnerName == csur.PartnerName && p.ConstantName == "AccountTypeId" && p.ConstantValue.Substring(12) == item.ACCOUNT_TYPE_ID.ToString()).Select(p => p.ConstantDescription).FirstOrDefault();
                    csur.AccountTypeIdName = pom == null ? "ERROR" : pom;

                    m_req004.Add(csur);
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }

        /// <summary>
        /// Interface method
        /// </summary>
        public void retrieveAnswers()
        {
            // retrieve card order answers from db into @m_ans002
            var pomList = m_cardnetDAL.getCardnetDB().C____ANS004.ToList();
            m_ans004 = new List<CardStatusUpdateAnswer>();

            foreach (var answer in pomList)
            {
                CardStatusUpdateAnswer csua = new CardStatusUpdateAnswer();

                csua.AnswerId = answer.ans_id;

                var pom = "";
                if (answer.card_status_code == 56) pom = "ACTIVE";
                else if (answer.card_status_code == 54) pom = "PERSONALIZED";
                else if (answer.card_status_code == 936) pom = "TEMP BLOCKED";
                else if (answer.card_status_code == 339) pom = "BLOCKED";
                else if (answer.card_status_code == 59) pom = "CANCELLED";

                csua.CardStatusCode = pom;/*m_rewardoDAL.getConstantsFromRW().Where(cst => cst.ConstantName == "CardStatus" && cst.ConstantId == answer.card_status_code).Select(cst => cst.ConstantValue).First();*/
                csua.ChangeTime = answer.change_time;
                csua.ID = answer.id;
                csua.PartnerName = m_cardnetDAL.getPartners().Where(p => p.ID == answer.partner_id).Select(p => p.OrganizationName).First()/* item.partner_id*/;
                csua.RequestId = answer.request_id;
                csua.RequestMessage = answer.request_message;
                csua.RequestStatus = answer.request_status == true ? "SUCCEEDED" : "DECLINED";

                m_ans004.Add(csua);
            }
        }

        /// <summary>
        /// Interface method.
        /// </summary>
        public CardStatusUpdateListViewModel fillListViewModel(List<CardStatusUpdateRequest> requests, List<CardStatusUpdateAnswer> answers)
        {
            // create CardStatusUpdateListViewModel
            CardStatusUpdateListViewModel p_csuLVM = new CardStatusUpdateListViewModel();

            // fill CardStatusUpdateListViewModel
            foreach (var request in requests)
            {
                // create CardStatusUpdateViewModel
                CardStatusUpdateViewModel csuVM = new CardStatusUpdateViewModel();

                csuVM.AnswerDateTime = request.AnswerDateTime;
                csuVM.CreationDateTime = request.CreationDateTime;
                csuVM.DailyLimit = request.DailyLimit;
                csuVM.ExternalCardId = request.ExternalCardId;                                                         
                var pom = (from item in answers where item.PartnerName == request.PartnerName && item.RequestId == request.RequestId select item.RequestMessage).FirstOrDefault();
                csuVM.Message = pom == null ? "" : pom.ToString();
                csuVM.ID = request.ID;
                csuVM.PartnerName = m_cardnetDAL.getPartners().Where(p => p.OrganizationName == request.PartnerName).Select(p => p.OrganizationName).First()/* item.partner_id*/;
                csuVM.PartnerRequestId = request.PartnerRequestId;
                csuVM.RequestedStateId = request.RequestedStateId;
                csuVM.RequestId = request.RequestId;
                csuVM.RequestStatus = request.RequestStatus;                                                                                                                    
                csuVM.TransactionLimit = request.TransactionLimit;
                csuVM.AccountTypeId = request.AccountTypeId;
                csuVM.AccountTypeIdName = request.AccountTypeIdName;

                if (request.RequestStatus == "SUCCEEDED") // card order is "RECEIVED"
                {
                    csuVM.RowColor = "#99ff99";
                }
                else if (request.RequestStatus == "DECLINED") // card order is "DECLINED"
                {
                    csuVM.RowColor = "#ff6666";
                }
                else if (request.RequestStatus == "null")
                {
                    csuVM.RowColor = "white";
                }

                // add CardOrderViewModel into CardOrderListViewModel
                p_csuLVM.csuvms.Add(csuVM);
            }

            p_csuLVM.csuvms = p_csuLVM.csuvms.OrderByDescending(o => o.ID).ToList();

            return p_csuLVM;
        }

        /// <summary>
        /// Interface method.
        /// </summary>
        public List<CardStatusUpdateRequest> getRequests() { return m_req004; }

        /// <summary>
        /// Interface method.
        /// </summary>
        public List<CardStatusUpdateAnswer> getAnswers() { return m_ans004; }
    }
}