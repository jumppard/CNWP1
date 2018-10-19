using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWebApp.ExportCarriers
{
    public class ExportedFileNameBuilder
    {
        private string m_destinationPath { get; set; } // input string which specifies the destination path of generated pdf
        private string m_partnerName { get; set; } // partner name
        private string m_cardOrderId { get; set; } // card order id
        private string m_environment { get; set; } // environment

        public ExportedFileNameBuilder()
        {
            Init();
        }

        public ExportedFileNameBuilder(string destinationPath, string partnerName, string cardOrderId, string environment)
        {
            Init();
            m_destinationPath = destinationPath;
            m_partnerName = partnerName;
            m_cardOrderId = cardOrderId;
            m_environment = environment;
        }

        public void Init()
        {
            m_destinationPath = "";
            m_partnerName = "";
            m_cardOrderId = "";
            m_environment = "";
        }

        public void SetDestinationPath(string newValue) { m_destinationPath = newValue; }
        public void SetPartnerName(string newValue) { m_partnerName = newValue; }
        public void SetCardOrderId(string newValue) { m_cardOrderId = newValue; }
        public void SetEnvironment(string newValue) { m_environment = newValue; }

        public string GetDestinationPath() { return m_destinationPath; }
        public string GetPartnerName() { return m_partnerName; }
        public string GetCardOrderId() { return m_cardOrderId; }
        public string GetEnvironment() { return m_environment; }
    }
}
