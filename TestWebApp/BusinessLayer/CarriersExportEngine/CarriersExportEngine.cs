using System.Xml.Linq;

namespace TestWebApp.ExportCarriers
{
    public class CarriersExportEngine
    {
        private XElement m_oldXml { get; set; } // old XML
        private XElement m_xmlForFastReport { get; set; } // new xml for Fastreport

        public CarriersExportEngine(XElement oldXml, XElement xmlForFastReport)
        {
            Init(oldXml: oldXml, xmlForFastReport: xmlForFastReport);
        }

        public void Init(XElement oldXml, XElement xmlForFastReport)
        {
            m_oldXml = oldXml;
            m_xmlForFastReport = xmlForFastReport;
        }

        public XElement GetOldXml() { return m_oldXml; }
        public XElement GetXmlForFastReport() { return m_xmlForFastReport; }
    }
}
