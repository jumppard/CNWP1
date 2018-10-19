using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestWebApp.DataAccessLayer;
using TestWebApp.ViewModel;

namespace TestWebApp.BusinessLayer
{
    public class RewardoEngine
    {
        // db init
        private RewardoDAL m_RewardoDAL;

        private RewardoCardListViewModel m_rewardoCLVM;

        public RewardoEngine()
        {
            m_RewardoDAL        = new RewardoDAL();
            m_rewardoCLVM       = new RewardoCardListViewModel();
            retrieveRewardoCardViewModel();
        }

        public void retrieveRewardoCardViewModel()
        {
            foreach (var item in m_RewardoDAL.getCardsFromRW())
            {
                RewardoCardViewModel rwcvm = new RewardoCardViewModel();

                rwcvm.CardCRMId         = item.CardCRMId        ;
                rwcvm.CardId            = item.CardId           ; 
                rwcvm.Cardno            = item.Cardno           ; 
                rwcvm.CardPoolId        = item.CardPoolId       ; 
                rwcvm.CardProductId     = item.CardProductId    ; 
                rwcvm.CustomerId        = item.CustomerId       ; 
                rwcvm.DailyLimit        = item.DailyLimit       ; 
                rwcvm.ExpiryDate        = item.ExpiryDate       ; 
                rwcvm.InsertDate        = item.InsertDate       ; 
                rwcvm.LastUpdateDate    = item.LastUpdateDate   ; 
                rwcvm.MerchantId        = item.MerchantId       ; 
                rwcvm.NextDailyLimit    = item.NextDailyLimit   ; 
                rwcvm.OrderInPackage    = item.OrderInPackage   ; 
                rwcvm.OrganizationId    = item.OrganizationId   ; 
                rwcvm.PackageId         = item.PackageId        ; 
                rwcvm.Status            = item.Status           ;
                rwcvm.TransactionLimit  = item.TransactionLimit ;

                m_rewardoCLVM.rclvm.Add(rwcvm);
            }
        }

        public RewardoCardListViewModel getRewardoCardListViewModel()
        {
            return m_rewardoCLVM;
        }

        public short getOrganizationId(string organizationName)
        {
            var result = (from item in m_RewardoDAL.getOrganizationsFromRW() where item.Name == organizationName select item.OrganizationId).FirstOrDefault();
            return result;
        }

        public RewardoCardViewModel getRewardoCardViewModelByCardCrmId(long item, short organziationId)
        {
            var lambda = m_rewardoCLVM.rclvm.Where(i => i.CardCRMId == item && i.OrganizationId == organziationId).FirstOrDefault();

            return lambda;
        }

        /// <summary>
        /// returns List of cards in Rewardo for @param partnerName
        /// </summary>
        /// <param name="partnerName"></param>
        /// <returns></returns>
        public List<long?> getPartnerCardIdsFromRW(string partnerName)
        {
            //RewardoEngine rwEngine = new RewardoEngine();
            /*var organizationId = 144;*/
            var organizationId = getOrganizationId(partnerName); // magic constant

            var result = (from item in getRewardoCardListViewModel().rclvm where item.OrganizationId == organizationId select item.CardCRMId).ToList();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partnerCardsFromCNXML">All cards of specific partner in XML</param>
        /// <param name="pom">out var</param>
        /// <param name="pomRewardoCardListViewModel">All cards of specific partner in REWARDO</param>
        /// <param name="organizationID">Organization ID</param>
        public void createRewardoCardListViewModel(List<long> partnerCardsFromCNXML, RewardoCardListViewModel pom, RewardoCardListViewModel pomRewardoCardListViewModel, short organizationId)
        {
            foreach (var item in partnerCardsFromCNXML)
            {
                if (rewardoCardListViewModelContainsThisCardId(pomRewardoCardListViewModel, item, organizationId)) // if does not contain
                {
                    var rewardoCardViewModel = getRewardoCardViewModelByCardCrmId(item, organizationId);
                    pom.rclvm.Add(rewardoCardViewModel);
                }
                else
                {
                    // Create foo RewardoCardViewModel with CARDNO == "" and add to @param pom
                    RewardoCardViewModel rewardoCardViewModel = new RewardoCardViewModel();
                    rewardoCardViewModel.CardCRMId = item;
                    rewardoCardViewModel.Cardno = "";
                    pom.rclvm.Add(rewardoCardViewModel);
                }
            }
        }

        /// <summary>
        /// returns t/f value. If <param>pomRewardoCardListViewModel</param> includes <param>item</param> => return true else return false
        /// </summary>
        /// <param name="pomRewardoCardListViewModel"></param>
        /// <param name="item"></param>
        /// <param name="organizationId">Organization ID</param>
        /// <returns></returns>
        private bool rewardoCardListViewModelContainsThisCardId(RewardoCardListViewModel pomRewardoCardListViewModel, long item, short organizationId)
        {
            var result = false;
            var pomRclvm = pomRewardoCardListViewModel.rclvm.Where(o => o.OrganizationId == organizationId).ToList();
            if (pomRclvm.Any())
            {
                var i = 0;
                do
                {
                    result = pomRclvm[i].CardCRMId == item ? true : false;
                    i++;
                } while (i < pomRclvm.Count && result == false);
            }
            return result;
        }

        public RewardoDAL getRewardoDAL() { return m_RewardoDAL; }

    }
}