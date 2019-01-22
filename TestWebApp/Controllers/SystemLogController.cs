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
    public class SystemLogController : Controller
    {
        // GET: SystemLog
        public ActionResult Index(int? page)
        {

            int pageIndex = page == null ? 1 : Convert.ToInt32(page);
            int pageSize = 200;

            var systemLogEngine = new SystemLogEngine();
            var notifListViewModel = systemLogEngine.retrieveNotificationsListViewModel();

            // https://www.c-sharpcorner.com/article/paging-in-asp-net-mvc-4-using-pagelist/
            IPagedList<NotificationViewModel> nvm = null;
            nvm = notifListViewModel.m_nlvm.ToPagedList(pageIndex, pageSize);

            return View("Index", nvm);
        }
    }
}