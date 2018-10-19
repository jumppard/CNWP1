using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestWebApp.Models;

namespace TestWebApp.DataAccessLayer
{
    // data access layer 
    // service layer
    public class RewardoDAL
    {
        private REWARDO_CARDNETEntities rewardoDB;
        private List<RewardoCard> m_cardsRW;
        private List<RewardoOrganization> m_organizationsRW;
        private List<RewardoCardProduct> m_CardProductsRW;
        private List<RewardoConstant> m_ConstantsRW;


        /// <summary>
        /// Constructor
        /// </summary>
        public RewardoDAL()
        {
            rewardoDB = new REWARDO_CARDNETEntities(); // DB is now accessible
            retieveCardsFromRewardo();
            retieveOrganizationsFromRewardo();
            retrieveCardProductsFromRW();
        }

        /// <summary>
        /// retrieves all constants from rewardo into @m_ConstantsRW
        /// </summary>
        private void retrieveConstantsFromRewardo()
        {
            // retrieve cards from rw db into m_cardsRW
            var pomList = rewardoDB.CONSTANT.ToList();

            m_ConstantsRW = new List<RewardoConstant>();

            foreach (var item in pomList)
            {
                RewardoConstant pomRWC = new RewardoConstant();

                pomRWC.ConstantId = item.CONSTANT_ID;
                pomRWC.ConstantName = item.CONSTANTNAME;
                pomRWC.ConstantValue = item.CONSTANTVALUE;
                pomRWC.InsertDate = item.INSERTDATE;
                pomRWC.LastUpdateDate = item.LASTUPDATEDATE;
                pomRWC.Status = item.STATUS;

                m_ConstantsRW.Add(pomRWC);
            }
        }

        /// <summary>
        /// retrieves all cards from rewardo into @m_cardsRW
        /// </summary>
        private void retieveCardsFromRewardo()
        {
            // retrieve cards from rw db into m_cardsRW
            var pomList = rewardoDB.CARD.ToList();

            m_cardsRW = new List<RewardoCard>();

            foreach (var item in pomList)
            {
                RewardoCard pomRWC = new RewardoCard();

                pomRWC.CardCRMId = item.CARD_CRM_ID;
                pomRWC.CardId = item.CARD_ID;
                pomRWC.Cardno = item.CARDNO;
                pomRWC.CardPoolId = item.CARDPOOL_ID;
                pomRWC.CardProductId = item.CARDPRODUCT_ID;
                pomRWC.CustomerId = item.CUSTOMER_ID;
                pomRWC.DailyLimit = item.DAILY_LIMIT;
                pomRWC.ExpiryDate = item.EXPIRYDATE;
                pomRWC.InsertDate = item.INSERTDATE;
                pomRWC.LastUpdateDate = item.LASTUPDATEDATE;
                pomRWC.MerchantId = item.MERCHANT_ID;
                pomRWC.NextDailyLimit = item.NEXT_DAILY_LIMIT;
                pomRWC.OrderInPackage = item.ORDER_IN_PACKAGE;
                pomRWC.OrganizationId = item.ORGANIZATION_ID;
                pomRWC.PackageId = item.PACKAGE_ID;
                pomRWC.Status = item.STATUS;
                pomRWC.TransactionLimit = item.TRANSACTION_LIMIT;

                m_cardsRW.Add(pomRWC);
            }
        }

        /// <summary>
        /// retrieves all organizations from rewardo into @m_organizationsRW
        /// </summary>
        private void retieveOrganizationsFromRewardo()
        {
            // retrieve organizations from rw db into m_organizationsRW
            var pomList = rewardoDB.ORGANIZATION.ToList();

            m_organizationsRW = new List<RewardoOrganization>();

            foreach (var item in pomList)
            {
                RewardoOrganization pomRWO = new RewardoOrganization();

                pomRWO.OrganizationId = item.ORGANIZATION_ID;
                pomRWO.Name = item.NAME;
                pomRWO.LongName = item.LONG_NAME;
                pomRWO.InsertDate = item.INSERTDATE;
                pomRWO.LastUpdateDate = item.LASTUPDATEDATE;
                pomRWO.Status = item.STATUS;

                m_organizationsRW.Add(pomRWO);
            }
        }

        /// <summary>
        /// retrieves all card products from rewardo into @m_CardProductsRW
        /// </summary>
        private void retrieveCardProductsFromRW()
        {
            // retrieve card products from rw db into m_CardProductsRW
            var pomList = rewardoDB.CARDPRODUCT.ToList();

            m_CardProductsRW = new List<RewardoCardProduct>();

            foreach (var item in pomList)
            {
                RewardoCardProduct pomRWCP = new RewardoCardProduct();

                pomRWCP.CardProductID = item.CARDPRODUCT_ID;
                pomRWCP.CardProductName = item.NAME;
                pomRWCP.DailyLimit = item.DAILY_LIMIT;
                pomRWCP.InsertDate = item.INSERTDATE;
                pomRWCP.LastUpdateDate = item.LASTUPDATEDATE;
                pomRWCP.NextDailyLimit = item.NEXT_DAILY_LIMIT;
                pomRWCP.OrganizationID = item.ORGANIZATION_ID;
                pomRWCP.Status = item.STATUS;
                pomRWCP.TransactionLimit = item.TRANSACTION_LIMIT;

                m_CardProductsRW.Add(pomRWCP);
            }
        }

        /// <summary>
        /// returns cards from rewardo => @m_cardsRW
        /// </summary>
        /// <returns>rewardo constants</returns>
        public List<RewardoConstant> getConstantsFromRW() { return m_ConstantsRW; }

        /// <summary>
        /// returns cards from rewardo => @m_cardsRW
        /// </summary>
        /// <returns>rewardo cards</returns>
        public List<RewardoCard> getCardsFromRW() { return m_cardsRW; }

        /// <summary>
        /// returns organizations from rewardo => @m_organizationsRW
        /// </summary>
        /// <returns>rewardo organizations</returns>
        public List<RewardoOrganization> getOrganizationsFromRW() { return m_organizationsRW; }

        /// <summary>
        /// returns card products from rewardo => @m_CardProductsRW
        /// </summary>
        /// <returns></returns>
        public List<RewardoCardProduct> getCardProductsFromRW(){ return m_CardProductsRW; }
        
        public bool cardIsInRW(short organizationID, long cardCrmId)
        {
            var query = (from card in m_cardsRW where card.OrganizationId == organizationID && card.CardCRMId == cardCrmId select card.Cardno).FirstOrDefault();
            var result = query != null ? true : false;
            return result;
        }

    }
}