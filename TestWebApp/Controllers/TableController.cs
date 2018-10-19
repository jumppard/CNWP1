using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWebApp.BusinessLayer;

namespace TestWebApp.Controllers
{
    public class TableController : Controller
    {
        // GET: Table
        public ActionResult Index()
        {
            //DataAccessLayer.CardnetDAL m_cardnetDAL = new DataAccessLayer.CardnetDAL("cn_db","all","all");
            TableEngine te = new TableEngine();
            //ViewBag.PosAcceptanceUnprocessedRequestsCount = te.getCardnetDAL().getPosAcceptanceRequests().Count - te.getCardnetDAL().getPosAcceptanceAnswers().Count;
            //ViewBag.CardOrderUnprocessedRequestsCount = te.getCardnetDAL().getCardOrderRequests().Count - te.getCardnetDAL().getCardOrderAnswers().Count;
            //ViewBag.CreditOrderUnprocessedRequestsCount = te.getCardnetDAL().getCreditOrderRequests().Count - te.getCardnetDAL().getCreditOrderAnswers().Count;
            //ViewBag.CardStatusUpdateUnprocessedRequestsCount = te.getCardnetDAL().getCardStatusUpdateRequests().Count - te.getCardnetDAL().getCardStatusUpdateAnswers().Count;

            return View(te.getTablesViewModel());
        }
        

    }
}