using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebApp.ViewModel
{
    public class HomePageViewModel
    {
        public List<Tuple<string, string>> CardnetTables = new List<Tuple<string, string>>(); // names of relevant tables in CN DB
        public Dictionary<string, Dictionary<string, string>> CardnetTableColumns = new Dictionary<string, Dictionary<string, string>>(); // key = OPERATION TYPE; value = {key = {todays waiting; todays valided; last valided date}}
    }
}