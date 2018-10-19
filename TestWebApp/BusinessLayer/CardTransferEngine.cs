using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestWebApp.DataAccessLayer;
using TestWebApp.Models;
using TestWebApp.ViewModel;

namespace TestWebApp.BusinessLayer
{
    public class CardTransferEngine : IReqAnsEngine<CardTransferRequest, CardTransferAnswer, CardTransferListViewModel>
    {
        private CardnetDAL m_cardnetDAL; // data access layer

        private List<CardTransferRequest> m_req008;
        private List<CardTransferAnswer> m_ans008;

        public CardTransferEngine()
        {
            try
            {
                // RETRIEVE CARD ORDER DATA (REQ/ANS)
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

        /// <summary>
        /// Interface method
        /// </summary>
        public void retrieveRequests()
        {
            // retrieve card order requests from db into @m_req002
            var pomList = m_cardnetDAL.getCardnetDB().C____REQ008.OrderByDescending(o => o.CREATION_DATETIME).ToList();
            m_req008 = new List<CardTransferRequest>();

            foreach (var item in pomList)
            {
                CardTransferRequest ctr = new CardTransferRequest();
                try
                {
                    ctr.BalanceTransfer = item.BALANCE_TRANSFER == true ? "YES" : "NO";
                    ctr.BonusTransfer = item.BONUS_TRANSFER == true ? "YES" : "NO";
                    ctr.CardnoFrom = item.CARDNO_FROM;
                    ctr.CardnoFromFinalStatus = m_cardnetDAL.getConstantUniversals().Where(c => c.ConstantName == "cardno_from_final_status" && c.ConstantValue == item.CARDNO_FROM_FINAL_STATUS).Select(c => c.ConstantDescription).FirstOrDefault();
                    ctr.CardnoTo = item.CARDNO_TO;
                    ctr.Content = item.CONTENT;
                    ctr.CreationDateTime = item.CREATION_DATETIME;
                    ctr.ID = item.ID;
                    ctr.IsProcessing = item.IS_PROCESSING;
                    ctr.LastUpdateDate = item.LAST_UPDATE;
                    ctr.PartnerId = item.PARTNER_ID;
                    ctr.PartnerName = m_cardnetDAL.getPartners().Where(p => p.ID == item.PARTNER_ID).Select(p => p.OrganizationName).First();
                    ctr.PartnerRequestId = item.PARTNER_REQUEST_ID;
                    ctr.RequestId = item.REQUEST_ID;

                    var pomRequestStatus = "";
                    if (item.REQUEST_STATUS == 1)
                        pomRequestStatus = "SUCCEEDED";
                    else if (item.REQUEST_STATUS == 0)
                        pomRequestStatus = "DECLINED";
                    else
                        pomRequestStatus = "null"; // nemalo by sa stat nikdy

                    ctr.RequestStatus = pomRequestStatus;
                    ctr.SlaDeadline = item.SLA_DEADLINE;
                    ctr.Succesfull = item.SUCCESSFULL;
                    ctr.TransferCase = m_cardnetDAL.getConstantUniversals().Where(c => c.ConstantName == "transfer_case" && c.ConstantValue == item.TRANSFER_CASE).Select(c => c.ConstantDescription).First();
                    ctr.TransferType = m_cardnetDAL.getConstantUniversals().Where(c => c.ConstantName == "transfer_type" && c.ConstantValue == item.TRANSFER_TYPE).Select(c => c.AdditionalValue).First();
                    ctr.AccountTypeId = item.ACCOUNT_TYPE_ID;
                    ctr.AccountTypefinalStatus = item.ACCOUNT_TYPE_FINAL_STATUS;

                    var pom = m_cardnetDAL.getConstantPartner().Where(p => p.PartnerName == ctr.PartnerName && p.ConstantName == "AccountTypeId" && p.ConstantValue.Substring(12) == item.ACCOUNT_TYPE_ID.ToString()).Select(p => p.ConstantDescription).FirstOrDefault();
                    ctr.AccountTypeIdName = pom == null ? "ERROR" : pom;

                    m_req008.Add(ctr);
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
            var pomList = m_cardnetDAL.getCardnetDB().C____ANS008.ToList();
            m_ans008 = new List<CardTransferAnswer>();

            foreach (var answer in pomList)
            {
                CardTransferAnswer cta = new CardTransferAnswer();

                cta.AnswerId = answer.ANS_ID;
                cta.AuthorizationIdFromCard = answer.AUTHORIZATION_ID_FROM_CARD;
                cta.AuthorizationIdToCard = answer.AUTHORIZATION_ID_TO_CARD;
                cta.ChangeTime = answer.CHANGE_TIME;
                cta.CurrentBalanceFromCard = answer.CURRENT_BALANCE_FROM_CARD;
                cta.CurrentBalanceToCard = answer.CURRENT_BALANCE_TO_CARD;
                cta.ID = answer.ID;
                cta.Message = answer.MESSAGE;
                cta.PartnerId = answer.PARTNER_ID;
                cta.RequestId = answer.REQUEST_ID;
                cta.RequestMessage = answer.REQUEST_MESSAGE;
                cta.RequestStatus = answer.REQUEST_STATUS == 1 ? "SUCCEEDED" : "DECLINED";


                m_ans008.Add(cta);
            }
        }

        /// <summary>
        /// Interface method.
        /// Fill T3 with <b>@requests</b> and <b>@answers</b>.
        /// T3 is the combination of <b>requests</b> and <b>answers</b>.
        /// </summary>
        /// <param name="requests"></param>
        /// <param name="answers"></param>
        /// <returns></returns>
        public CardTransferListViewModel fillListViewModel(List<CardTransferRequest> requests, List<CardTransferAnswer> answers)
        {
            // create CardTransferListViewModel
            CardTransferListViewModel p_ctLVM = new CardTransferListViewModel();

            // fill CardTransferListViewModel
            foreach (var request in requests)
            {
                // create CardTransferViewModel
                CardTransferViewModel ctVM = new CardTransferViewModel();

                ctVM.BalanceTransfer = request.BalanceTransfer;
                ctVM.BonusTransfer = request.BonusTransfer;
                ctVM.CardnoFrom = request.CardnoFrom;
                ctVM.CardnoFromFinalStatus = request.CardnoFromFinalStatus;
                ctVM.CardnoTo = request.CardnoTo;
                ctVM.Content = request.Content;
                ctVM.CreationDateTime = request.CreationDateTime;
                ctVM.ID = request.ID;
                ctVM.IsProcessing = request.IsProcessing;
                ctVM.LastUpdateDate = request.LastUpdateDate;
                ctVM.PartnerId = request.PartnerId;
                ctVM.PartnerName = m_cardnetDAL.getPartners().Where(p => p.OrganizationName == request.PartnerName).Select(p => p.OrganizationName).First();
                ctVM.PartnerRequestId = request.PartnerRequestId;
                ctVM.RequestId = request.RequestId;
                ctVM.RequestStatus = request.RequestStatus;
                ctVM.SlaDeadline = request.SlaDeadline;
                ctVM.Succesfull = request.Succesfull;
                ctVM.TransferCase = request.TransferCase;
                ctVM.TransferType = request.TransferType;
                ctVM.AccountTypeId = request.AccountTypeId;
                ctVM.AccountTypefinalStatus = request.AccountTypefinalStatus;
                ctVM.AccountTypeIdName = request.AccountTypeIdName;

                if (request.RequestStatus == "SUCCEEDED") // card order is "RECEIVED"
                {
                    ctVM.RowColor = "#99ff99";
                }
                else if (request.RequestStatus == "DECLINED") // card order is "DECLINED"
                {
                    ctVM.RowColor = "#ff6666";
                }
                else
                {
                    ctVM.RowColor = "white";
                }

                // add CardOrderViewModel into CardOrderListViewModel
                p_ctLVM.ctvms.Add(ctVM);
            }

            return p_ctLVM;
        }

        /// <summary>
        /// Interface method.
        /// Returns all CARD TRANSFER requests)
        /// </summary>
        /// <returns></returns>
        public List<CardTransferRequest> getRequests() { return m_req008; }

        /// <summary>
        /// Interface method.
        /// Returns all CARD TRANSFER answers)
        /// </summary>
        /// <returns></returns>
        public List<CardTransferAnswer> getAnswers() { return m_ans008; }
    }
}