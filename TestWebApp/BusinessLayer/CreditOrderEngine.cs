using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestWebApp.ViewModel;
using TestWebApp.DataAccessLayer;
using TestWebApp.Models;

namespace TestWebApp.BusinessLayer
{
    public class CreditOrderEngine : IReqAnsEngine<CreditOrderRequest, CreditOrderAnswer, CreditOrderListViewModel>
    {
        private CardnetDAL m_cardnetDAL; // data access layer

        private List<CreditOrderRequest> m_req003;// = new List<CreditOrderRequest>();*/
        private List<CreditOrderAnswer> m_ans003;//= new List<CreditOrderAnswer>();*/

        public CreditOrderEngine()
        {
            try
            {
                // RETRIEVE CREDIT ORDER DATA (REQ/ANS)
                m_cardnetDAL = new CardnetDAL();
                m_cardnetDAL.retrievePartners();
                m_cardnetDAL.retrieveConstantUniversal();
                m_cardnetDAL.retrieveConstantPartners();

                retrieveRequests();
                retrieveAnswers();
            }
            catch (Exception)
            {

                throw;
            }
        }

        // interface method
        public void retrieveRequests()
        {
            // retrieve card order requests from db into @m_req002
            var pomList = m_cardnetDAL.getCardnetDB().C____REQ003.OrderByDescending(o => o.creation_datetime).ToList();
            m_req003 = new List<CreditOrderRequest>();

            foreach (var item in pomList)
            {
                CreditOrderRequest cror = new CreditOrderRequest();

                cror.AnswerDateTime = item.answer_datetime;
                cror.CreationDateTime = item.creation_datetime;
                cror.ExternalCardId = item.external_card_id;
                cror.ID = item.id;
                cror.PartnerName = m_cardnetDAL.getPartners().Where(p => p.ID == item.partner_id).Select(p => p.OrganizationName).First()/* item.partner_id*/;
                cror.PartnerRequestId = item.partner_request_id;

                var pomRequestStatus = "";
                if (item.request_status == true)
                    pomRequestStatus = "SUCCEEDED";
                else if (item.request_status == false)
                    pomRequestStatus = "DECLINED";
                else
                    pomRequestStatus = "null"; // nemalo by sa stat nikdy

                cror.RequestStatus = pomRequestStatus;
                cror.TransactionAmount = item.transaction_amount;
                cror.TransactionType = m_cardnetDAL.getConstantUniversals().Where(cst=>cst.ConstantName == "transaction_type" && cst.ConstantValue == item.transaction_type).Select(cst=>cst.ConstantDescription).First();
                cror.RequestId = item.request_id;
                cror.AccountTypeId = item.ACCOUNT_TYPE_ID;

                var pom = m_cardnetDAL.getConstantPartner().Where(p => p.PartnerName == cror.PartnerName && p.ConstantName == "AccountTypeId" && p.ConstantValue.Substring(12) == item.ACCOUNT_TYPE_ID.ToString()).Select(p => p.ConstantDescription).FirstOrDefault();
                cror.AccountTypeIdName = pom == null ? "ERROR" : pom;

                m_req003.Add(cror);
            }
        }

        // interface method
        public void retrieveAnswers()
        {
            // retrieve card order answers from db into @m_ans002
            var pomList = m_cardnetDAL.getCardnetDB().C____ANS003.ToList();
            m_ans003 = new List<CreditOrderAnswer>();

            foreach (var answer in pomList)
            {
                CreditOrderAnswer croa = new CreditOrderAnswer();

                croa.AnswerId = answer.ans_id;
                croa.ChangeTime = answer.change_time;
                croa.ID = answer.id;
                croa.PartnerName = m_cardnetDAL.getPartners().Where(p => p.ID == answer.partner_id).Select(p => p.OrganizationName).First()/* item.partner_id*/;
                croa.RequestId = answer.request_id;
                croa.RequestStatus = answer.request_status == true ? "SUCCEEDED" : "DECLINED";
                croa.TransactionBalance = answer.transaction_balance;
                croa.TransactionFinal = answer.transaction_final;
                croa.TransactionId = answer.transaction_id;   
                croa.RequestMessage = answer.request_message;

                m_ans003.Add(croa);
            }
        }

        // interface method
        public CreditOrderListViewModel fillListViewModel(List<CreditOrderRequest> cror, List<CreditOrderAnswer> croa)
        {
            // create CreditOrderListViewModel
            CreditOrderListViewModel p_crorLVM = new CreditOrderListViewModel();

            // fill CreditOrderListViewModel
            foreach (var request in cror)
            {
                // create CreditOrderViewModel
                CreditOrderViewModel crorVM = new CreditOrderViewModel();

                crorVM.AnswerDateTime = request.AnswerDateTime;/*AnswerDateTime = request.AnswerDateTime;*/
                crorVM.CreationDateTime = request.CreationDateTime;/*CreationDateTime = request.CreationDateTime;*/
                crorVM.ExternalCardId = request.ExternalCardId;/*ExternalTerminalId = request.ExternalTerminalId;*/
                crorVM.ID = request.ID;/*ID = request.ID;*/
                var pom  = (from item in croa where item.PartnerName == request.PartnerName && item.RequestId == request.RequestId select item.RequestMessage).FirstOrDefault();
                crorVM.Message = pom == null ? "" : pom.ToString();
                crorVM.PartnerName = m_cardnetDAL.getPartners().Where(p => p.OrganizationName == request.PartnerName).Select(p => p.OrganizationName).First();
                crorVM.PartnerRequestId = request.PartnerRequestId;/*NewAcceptState = request.NewAcceptState;*/
                crorVM.RequestId = request.RequestId;
                crorVM.RequestStatus = request.RequestStatus;/*PartnerId = request.PartnerId;*/
                crorVM.TransactionAmount = request.TransactionAmount;/*PartnerRequestId = request.PartnerRequestId;*/
                crorVM.TransactionType = request.TransactionType;/*RequestId = request.RequestId;*/
                crorVM.AccountTypeId = request.AccountTypeId;
                crorVM.AccountTypeIdName = request.AccountTypeIdName;

                if (request.RequestStatus == "SUCCEEDED") // card order is "RECEIVED"
                {
                    crorVM.RowColor = "#99ff99";
                }
                else if (request.RequestStatus == "DECLINED") // card order is "DECLINED"
                {
                    crorVM.RowColor = "#ff6666";
                }
                else if (request.RequestStatus == "null")
                {
                    crorVM.RowColor = "white";
                }

                // add CardOrderViewModel into CardOrderListViewModel
                p_crorLVM.crovms.Add(crorVM);
            }

            p_crorLVM.crovms = p_crorLVM.crovms.OrderByDescending(o => o.ID).ToList();

            return p_crorLVM;
        }

        // interface method
        public List<CreditOrderRequest> getRequests() { return m_req003; }

        // interface method
        public List<CreditOrderAnswer> getAnswers() { return m_ans003; }
    }
}