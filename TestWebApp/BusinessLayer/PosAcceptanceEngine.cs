using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestWebApp.ViewModel;
using TestWebApp.DataAccessLayer;
using TestWebApp.Models;

namespace TestWebApp.BusinessLayer
{
    public class PosAcceptanceEngine : IReqAnsEngine<PosAcceptanceRequest,PosAcceptanceAnswer,PosAcceptanceListViewModel>
    {
        private CardnetDAL m_cardnetDAL; // data access layer

        private List<PosAcceptanceRequest> m_req001;// = new List<PosAcceptanceRequest>();*/
        private List<PosAcceptanceAnswer> m_ans001;//= new List<PosAcceptanceAnswer>();*/


        public PosAcceptanceEngine()
        {
            try
            {
                // RETRIEVE POS ACCEPTANCE DATA (REQ/ANS)
                m_cardnetDAL = new CardnetDAL();
                m_cardnetDAL.retrievePartners();
                m_cardnetDAL.retrieveConstantPartners();

                retrieveRequests();
                retrieveAnswers();
            }
            catch (Exception EX)
            {

                throw;
            }
        }

        // interface method
        public void retrieveRequests()
        {
            // retrieve card order requests from db into @m_req002
            var pomList = m_cardnetDAL.getCardnetDB().C____REQ001.OrderByDescending(o => o.creation_datetime).ToList();
            m_req001 = new List<PosAcceptanceRequest>();

            foreach (var item in pomList)
            {
                PosAcceptanceRequest posrVM = new PosAcceptanceRequest();
                try
                {
                    posrVM.AnswerDateTime = item.answer_datetime;
                    posrVM.CreationDateTime = item.creation_datetime;
                    posrVM.ExternalTerminalId = item.external_terminal_id;
                    posrVM.ID = item.id;
                    posrVM.NewAcceptState = item.new_accept_state == true ? "ACTIVE" : "PASSIVE";
                    posrVM.PartnerName = m_cardnetDAL.getPartners().Where(p=>p.ID == item.partner_id).Select(p=>p.OrganizationName).First()/* item.partner_id*/;
                    posrVM.PartnerRequestId = item.partner_request_id;
                    posrVM.RequestId = item.request_id;

                    var pomRequestStatus = "";
                    if (item.request_status == true)
                        pomRequestStatus = "SUCCEEDED";
                    else if (item.request_status == false)
                        pomRequestStatus = "DECLINED";
                    else
                        pomRequestStatus = "null"; // nemalo by sa stat nikdy

                    posrVM.RequestStatus = pomRequestStatus;
                    posrVM.TerminalTypeId = item.terminal_type_id;
                    posrVM.AccountTypeId = item.ACCOUNT_TYPE_ID;

                    var pom = m_cardnetDAL.getConstantPartner().Where(p => p.PartnerName == posrVM.PartnerName && p.ConstantName == "AccountTypeId" && p.ConstantValue.Substring(12) == item.ACCOUNT_TYPE_ID.ToString()).Select(p => p.ConstantDescription).FirstOrDefault();
                    posrVM.AccountTypeIdNAME = pom == null ? "ERROR" : pom;

                    m_req001.Add(posrVM);
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }

        // interface method
        public void retrieveAnswers()
        {
            // retrieve card order answers from db into @m_ans002
            var pomList = m_cardnetDAL.getCardnetDB().C____ANS001.ToList();
            m_ans001 = new List<PosAcceptanceAnswer>();

            foreach (var answer in pomList)
            {
                PosAcceptanceAnswer posaVm = new PosAcceptanceAnswer();

                posaVm.AnswerId = answer.ans_id;
                posaVm.ID = answer.id;
                posaVm.InsertDateTime = answer.change_time;
                posaVm.PartnerName = m_cardnetDAL.getPartners().Where(p => p.ID == answer.partner_id).Select(p => p.OrganizationName).First(); 
                posaVm.RequestId = answer.request_id;
                posaVm.RequestMessage = answer.request_message;
                posaVm.RequestStatus = answer.request_status == true ? "SUCCEEDED" : "DECLINED";
                posaVm.StatusAfterRequest = answer.status_after_request == 265 ? "ACTIVE" : "PASSIVE";

                m_ans001.Add(posaVm);
            }
        }

        /// interface method
        public PosAcceptanceListViewModel fillListViewModel(List<PosAcceptanceRequest> posr, List<PosAcceptanceAnswer> posa)
        {
            // create PosAcceptanceListViewModel
            PosAcceptanceListViewModel p_posLVM = new PosAcceptanceListViewModel();

            // fill PosAcceptanceListViewModel
            foreach (var request in posr)
            {
                // create PosAcceptanceViewModel
                PosAcceptanceViewModel posVM = new PosAcceptanceViewModel();

                posVM.AnswerDateTime = request.AnswerDateTime;
                posVM.CreationDateTime = request.CreationDateTime;
                posVM.ExternalTerminalId = request.ExternalTerminalId;
                posVM.ID = request.ID;
                var pom = (from item in posa where item.PartnerName == request.PartnerName && item.RequestId == request.RequestId select item.RequestMessage).FirstOrDefault();
                posVM.Message = pom == null ? "" : pom.ToString();
                posVM.NewAcceptState = request.NewAcceptState;
                posVM.PartnerName = request.PartnerName;
                posVM.PartnerRequestId = request.PartnerRequestId;
                posVM.RequestId = request.RequestId;
                posVM.RequestStatus = request.RequestStatus;
                posVM.TerminalTypeId = request.TerminalTypeId;
                var pom1 = posa.Where(a => a.PartnerName == request.PartnerName && a.RequestId == request.RequestId).Select(a => a.StatusAfterRequest).FirstOrDefault();
                posVM.StatusAfterRequest = pom1 == null ? "" : pom1;
                posVM.AccountTypeId = request.AccountTypeId;
                posVM.AccountTypeIdName = request.AccountTypeIdNAME;

                if (request.RequestStatus == "SUCCEEDED") // card order is "RECEIVED"
                {
                    posVM.RowColor = "#99ff99";
                }
                else if (request.RequestStatus == "DECLINED") // card order is "DECLINED"
                {
                    posVM.RowColor = "#ff6666";
                }
                else if (request.RequestStatus == "null")
                {
                    posVM.RowColor = "white";
                }

                // add CardOrderViewModel into CardOrderListViewModel
                p_posLVM.posVm.Add(posVM);
            }

            p_posLVM.posVm = p_posLVM.posVm.OrderByDescending(o => o.ID).ToList();

            return p_posLVM;
        }

        // interface method
        public List<PosAcceptanceRequest> getRequests() { return m_req001; }

        // interface method
        public List<PosAcceptanceAnswer> getAnswers() { return m_ans001; }

    }
}