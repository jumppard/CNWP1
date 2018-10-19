using System.Web;
using TestWebApp.Models;
using TestWebApp.ViewModel;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using TestWebApp.DataAccessLayer;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Data.SqlClient;

namespace TestWebApp.BusinessLayer
{
    public class TableEngine
    {
        private CardOrderEngine m_cardOrderEngine;
        private RewardoDAL m_rewardoDAL;
        private TablesViewModel m_tablesViewModel;

        public TableEngine()
        {
            init();
        }

        public void fillTableViewModel(TablesViewModel tablesViewModel)
        {
            m_cardOrderEngine.getCardnetDAL().retrieveOperationTypes(tablesViewModel);
        }

        public void init()
        {
            m_cardOrderEngine = new CardOrderEngine();

            var pomList = new List<string>() { "PosAcceptanceRequests", "PosAcceptanceAnswers", "CardOrderRequests", "CardOrderAnswers", "CreditOrderRequests", "CreditOrderAnswers",
                                                "CardStatusUpdateRequests", "CardStatusUpdateAnswers", "CardTransferRequests", "CardTransferAnswers", "partners", "constantUniversals", "constantPartners" };

            m_cardOrderEngine.setDbTableInitialization("all", pomList);

            m_tablesViewModel = new TablesViewModel();
            fillTableViewModel(m_tablesViewModel);
        }

        private List<CardOrderRequest> getCardOrdersByRequestStatus(int requestStatusConstantValue)
        {
            var result = m_cardOrderEngine.getCardnetDAL().getCardOrderRequests().Where(x => x.RequestStatus == requestStatusConstantValue).Select(x=>x).ToList();
            return result;
        }

        /// <summary>
        /// get the first <card_id> from from item.content
        /// get the creation_datetime from item.creation_datetime
        /// loop in rewardo [CARD] table where card.insert_date >= creation_datetime && card.crm_id == <card_id>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool orderIsProcessing(CardOrderRequest item)
        {
            // initialization of [REWARDO].[CARD] table
            m_rewardoDAL = new RewardoDAL();
            var pomRewardoCards = m_rewardoDAL.getCardsFromRW();

            // get the first <card_id> from from item.content
            var xelement = XElement.Parse(item.Content);
            var firstXMLCardIdElementValue = Convert.ToInt64(xelement.Element("card").Element("card_id").Value);

            // get the creation_datetime from item.creation_datetime
            var creationDatetime = item.CreationDateTime;

            // loop in rewardo[CARD] table where card.insert_date >= creation_datetime && card.crm_id == < card_id >
            var pom = false;
            var i = 0;
            while (pom==false && i < pomRewardoCards.Count)
            {
                pom = pomRewardoCards.Where(x => x.InsertDate >= creationDatetime && x.CardCRMId == firstXMLCardIdElementValue).Select(x => x).FirstOrDefault() == null  ? false : true;
                i += 1;
            }
            return pom;
        }

        /// <summary>
        /// Metoda sluzi ako logika pre automatizovane nastavenie card order-ov, konkretne ich [isProcessing] stlpec na TRUE
        /// ak je tento stlpec nastaveny na TRUE => objednavka sa realne fyzicky spracovava
        /// </summary>
        public void freshCardOrdersWidget()
        {
            var receivedCardOrders  = getCardOrdersByRequestStatus(1);
            var todaySentOrders     = getCardOrdersByRequestStatus(2).Where(x=>x.LastUpdate>=DateTime.Today)/*.Select(x=>x)*/;

            var joinOrders = receivedCardOrders;
            joinOrders.AddRange(todaySentOrders); // ALL received + TODAY sent

            foreach (var item in joinOrders)
            {
                if (item.RequestStatus == 1) // RECEIVED ORDER
                {
                    CardOrderViewModel covm = new CardOrderViewModel();
                    //if (item.IsProcessing == false)
                    //{
                    //    var pom = orderIsProcessing(item);

                    //    if (pom)
                    //    {
                    //        var pomTableName = "[dbo].[____REQ002]";
                    //        string queryString = String.Format("UPDATE FROM {0} SET is_processing = true  WHERE id = {1}", pomTableName, item.ID);
                    //        string connectionString = "Server=192.168.20.102;Database=dev;User Id=xskok;Password=emo7veR3;";

                    //        using (SqlConnection connection = new SqlConnection(connectionString))
                    //        {
                    //            SqlCommand command = new SqlCommand(queryString, connection);
                    //            command.Parameters.AddWithValue("@pomRequestId", item.ID);
                    //            connection.Open();
                    //            command.ExecuteNonQuery();
                    //        }

                            var validStatusDate = "";
                            var receivedStatusDate = "";
                            var declinedStatusDate = "";
                            var sentStatusDate = "";
                            var successfullyCanceledStatusDate = "";
                            var unsuccessfullyCanceledStatusDate = ""; // unnecessary
                            var pomRowColor = "";
                            var message = "";

                            m_cardOrderEngine.fillCardOrderStatusesDate(out message, out validStatusDate, out receivedStatusDate, out declinedStatusDate, out sentStatusDate, out successfullyCanceledStatusDate, out unsuccessfullyCanceledStatusDate, out pomRowColor, m_cardOrderEngine.getCardnetDAL().getCardOrderAnswers(), item);
                            covm = m_cardOrderEngine.createAndReturnNewCardOrderViewModel(item.CardTypeId, item.Content, item.CreationDateTime, item.ID, message, item.PartnerId, item.PartnerRequestId, item.RequestID,
                                                                                   item.CardCount, item.CarrierTypeId, item.EnvelopeTypeId, item.PlasticTypeId, item.AttachementId, item.PackageAttachementId, item.RequestStatus, item.LastUpdate, item.SlaDeadLine, item.StatusFinal, item.ValidTo, item.IsProcessing, validStatusDate, receivedStatusDate
                                                                                   , declinedStatusDate, sentStatusDate, successfullyCanceledStatusDate, unsuccessfullyCanceledStatusDate, pomRowColor, item.OrderType, item.DeliveryListId);

                    this.m_tablesViewModel.colVm.Add(covm);
                    //    }
                    //}
                    //else
                    //{
                    //    var validStatusDate = "";
                    //    var receivedStatusDate = "";
                    //    var declinedStatusDate = "";
                    //    var sentStatusDate = "";
                    //    var canceledStatusDate = "";
                    //    var pomRowColor = "";
                    //    var message = "";

                    //    m_cardOrderEngine.fillCardOrderStatusesDate(out message, out validStatusDate, out receivedStatusDate, out declinedStatusDate, out sentStatusDate, out canceledStatusDate, out pomRowColor, m_cardOrderEngine.getCardnetDAL().getCardOrderAnswers(), item);
                    //    covm = m_cardOrderEngine.createAndReturnNewCardOrderViewModel(item.CardTypeId, item.Content, item.CreationDateTime, item.ID, message, item.PartnerId, item.PartnerRequestId, item.RequestID,
                    //                                                                    item.CardCount, item.CarrierTypeId, item.EnvelopeTypeId, item.PlasticTypeId, item.AttachementId, item.PackageAttachementId, item.RequestStatus, item.LastUpdate, item.SlaDeadLine, item.StatusFinal, item.ValidTo, item.IsProcessing, validStatusDate, receivedStatusDate
                    //                                                                    , declinedStatusDate, sentStatusDate, canceledStatusDate, pomRowColor);

                    //    this.m_tablesViewModel.colVm.Add(covm);
                    //}

                }
                else // request_status == 2 // SENT ORDER
                {
                    CardOrderViewModel covm_todaysent = new CardOrderViewModel();
                    var validStatusDate = "";
                    var receivedStatusDate = "";
                    var declinedStatusDate = "";
                    var sentStatusDate = "";
                    var successfullyCanceledStatusDate = "";
                    var unsuccessfullyCanceledStatusDate = "";
                    var pomRowColor = "";
                    var message = "";

                    m_cardOrderEngine.fillCardOrderStatusesDate(out message, out validStatusDate, out receivedStatusDate, out declinedStatusDate, out sentStatusDate, out successfullyCanceledStatusDate, out unsuccessfullyCanceledStatusDate, out pomRowColor, m_cardOrderEngine.getCardnetDAL().getCardOrderAnswers(), item);
                    covm_todaysent = m_cardOrderEngine.createAndReturnNewCardOrderViewModel(item.CardTypeId, item.Content, item.CreationDateTime, item.ID, message, item.PartnerId, item.PartnerRequestId, item.RequestID,
                                                                                    item.CardCount, item.CarrierTypeId, item.EnvelopeTypeId, item.PlasticTypeId, item.AttachementId, item.PackageAttachementId, item.RequestStatus, item.LastUpdate, item.SlaDeadLine, item.StatusFinal, item.ValidTo, item.IsProcessing, validStatusDate, receivedStatusDate
                                                                                    , declinedStatusDate, sentStatusDate, successfullyCanceledStatusDate, unsuccessfullyCanceledStatusDate, pomRowColor, item.OrderType, item.DeliveryListId);

                    this.m_tablesViewModel.colVm_todaySent.Add(covm_todaysent);
                }
            }
        }

        public CardnetDAL getCardnetDAL() { return m_cardOrderEngine.getCardnetDAL(); }
        public TablesViewModel getTablesViewModel() { return m_tablesViewModel; }
    }
}