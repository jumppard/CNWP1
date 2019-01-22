using System;
using System.Collections.Generic;
using System.Linq;
using TestWebApp.ViewModel;
using TestWebApp.DataAccessLayer;
using TestWebApp.Models;

namespace TestWebApp.BusinessLayer
{
    public class SystemLogEngine
    {

        private CardnetDAL m_cardnetDAL; // data access layer
        private List<Notification> m_notifs;
        private List<NotificationViewModel> m_notifsViewModels;


        public SystemLogEngine()
        {
            Init();
        }

        private void Init()
        {
            m_cardnetDAL = new CardnetDAL();
            m_notifs = new List<Notification>();
            m_notifsViewModels = new List<NotificationViewModel>();
        }

        private void retrieveNotifications()
        {
            var pomList = m_cardnetDAL.getCardnetDB().C___NOTIFICATION.OrderByDescending(o => o.INSERTED).ToList();

            foreach (var item in pomList)
            {
                Notification notif = new Notification();
                try
                {
                    notif.AppId = item.APP_ID;
                    notif.ID = item.ID;
                    notif.Inserted = item.INSERTED;
                    notif.LastUpdated = item.LASTUPDATED;
                    notif.NotificationTypeId = item.NOTIFICATION_TYPE_ID;
                    notif.Status = item.STATUS;
                    notif.Subject = item.SUBJECT;
                    notif.Message = item.MESSAGE;

                    m_notifs.Add(notif);
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }

        public NotificationListViewModel retrieveNotificationsListViewModel()
        {
            retrieveNotifications();

            if (m_notifs.Any())
            {
                foreach (var notif in m_notifs)
                {
                    CreateNotificationViewModel(notif);
                }
            }

            NotificationListViewModel nlvm = new NotificationListViewModel();
            nlvm.m_nlvm = m_notifsViewModels.OrderByDescending(n=>n.ID).ToList();

            return nlvm;
        }

        private void CreateNotificationViewModel(Notification notif)
        {
            var appId = m_cardnetDAL.getCardnetDB().C___APPLICATION.Where(a => a.ID == notif.AppId).First();

            var message = notif.Message;
            if (message != "")
            {
                var firstIndexOfString = message.IndexOf("<h1>", 0) + 4;
                var lastIndexOfString = message.IndexOf("</h1>", 0);

                message = message.Substring(firstIndexOfString, lastIndexOfString-firstIndexOfString);
            }

            var pomPartnerName = "null";
            try
            {
                pomPartnerName = m_cardnetDAL.getCardnetDB().C___PARTNER.Where(p => p.id == appId.PARTNER_ID).First().organization_name;
            }
            catch (Exception)
            {
                //throw;
            }

            var pomNotificationStatusInfo = m_cardnetDAL.getCardnetDB().C___CONSTANT_UNIVERSAL.Where(cu => cu.constant_name == "notification_status_id" && cu.constant_value == notif.Status).First().constant_description;
            var pomNotificationType = m_cardnetDAL.getCardnetDB().C___CONSTANT_UNIVERSAL.Where(cu => cu.constant_name == "notification_type_id" && cu.constant_value == notif.NotificationTypeId).First().constant_description;

            NotificationViewModel nvm = new NotificationViewModel()
            {
                AppId = notif.AppId,
                ID = notif.ID,
                Inserted = notif.Inserted,
                LastUpdated = notif.LastUpdated,
                NotificationTypeId = notif.NotificationTypeId,
                Status = notif.Status,
                Subject = notif.Subject,
                Message = message,

                PartnerName = pomPartnerName,
                AppName = appId.NAME,
                StatusInfo = pomNotificationStatusInfo,
                NotificationTypeName = pomNotificationType,
            };

            m_notifsViewModels.Add(nvm);
        }
    }
}