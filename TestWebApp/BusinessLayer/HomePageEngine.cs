using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestWebApp.DataAccessLayer;
using TestWebApp.ViewModel;


namespace TestWebApp.BusinessLayer
{
    public class HomePageEngine
    {
        private CardOrderEngine m_cardOrderEngine;
        private TablesViewModel m_tablesViewModel;

        public HomePageEngine()
        {
            init();
        }

        public void fillTableViewModel()
        {
            m_cardOrderEngine.getCardnetDAL().retrieveOperationTypes(m_tablesViewModel);
        }

        public void init()
        {
            m_cardOrderEngine = new CardOrderEngine();
            m_cardOrderEngine.setDbTableInitialization("all"); // cardnet db init + rewardo db init
            m_cardOrderEngine.getCardnetDAL().retrieveRequests("all");
            m_cardOrderEngine.getCardnetDAL().retrieveAnswers("all");
            m_cardOrderEngine.getCardnetDAL().retrievePartners();
            m_cardOrderEngine.getCardnetDAL().retrieveConstantUniversal();

            m_tablesViewModel = new TablesViewModel();
            fillTableViewModel();
        }
    }
}