using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWebApp.ViewModel;
using TestWebApp.Models;
using TestWebApp.BusinessLayer;
using PagedList;

namespace TestWebApp.Controllers
{
    public class PosAcceptanceController : Controller
    {
        public ActionResult Index(int? page)
        {

            int pageIndex = page == null ? 1 : Convert.ToInt32(page);
            int pageSize = 200;

            // 1.   declare new CardOrderListViewModel instance
            // 2.   fill this istance with data from DB
            // 3.   Display this instance by returning View
            // --------------------------------------------------------

            // 1.   declare new PosAcceptanceListViewModel instance
            PosAcceptanceListViewModel posVM = new PosAcceptanceListViewModel();

            // 2.   fill this istance with data from DB
            PosAcceptanceEngine appEngine = new PosAcceptanceEngine();
            var requests = appEngine.getRequests();
            var answers = appEngine.getAnswers();
            posVM = appEngine.fillListViewModel(requests, answers);

            // https://www.c-sharpcorner.com/article/paging-in-asp-net-mvc-4-using-pagelist/
            IPagedList<PosAcceptanceViewModel> pa = null;
            pa = posVM.posVm.ToPagedList(pageIndex, pageSize);

            // 3.   Display this instance with returning View
            return View("Index", /*posVM*/pa);
        }
    }
}