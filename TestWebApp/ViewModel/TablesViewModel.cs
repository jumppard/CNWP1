using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebApp.ViewModel
{
    public class TablesViewModel
    {
        /// <summary>
        /// Widget ktory sa vyuzije na HOMEPAGE, kde bude zoznam aktualne spracovavanych objednavok
        /// </summary>
        public List<CardOrderViewModel> colVm = new List<CardOrderViewModel>();

        /// <summary>
        /// Widget ktory sa vyuzije na HOMEPAGE, kde bude zoznam dnes odoslanych objednavok
        /// </summary>
        public List<CardOrderViewModel> colVm_todaySent = new List<CardOrderViewModel>(); 

        public List<Tuple<string,string>> CardnetTables = new List<Tuple<string,string>>(); // names of relevant tables in CN DB
        public Dictionary<string, Dictionary<string,string>> CardnetTableColumns = new Dictionary<string, Dictionary<string,string>>(); // key = OPERATION TYPE; value = {key = {todays waiting; todays valided; last valided date}}
    }
}