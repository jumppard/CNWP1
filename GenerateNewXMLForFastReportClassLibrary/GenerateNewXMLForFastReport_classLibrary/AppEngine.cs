using System.Linq;
using System.Xml.Linq;

namespace GenerateNewXMLForFastReportClassLibrary
{
    public class AppEngine
    {
        public XElement m_xml; // new xml
        public string m_fileName;
        public long m_CardId;

        public AppEngine(string organizationCarrierConstantValue, XElement originalXML, long firstNewCardId)
        {
            //if (organizationCarrierConstantValue == "bbb_CA1" || organizationCarrierConstantValue == "aaa_CA1" || organizationCarrierConstantValue == "aaa_CA2" || organizationCarrierConstantValue == "aaa_CA3" || organizationCarrierConstantValue == "aaa_CA4")
            //{
            m_CardId = firstNewCardId;
            updateXML(originalXML);
            //}
        }

        public void updateXML(XElement originalXML)
        {
            //carrier
            var cards = originalXML.Elements("card");
            foreach (var item in cards)
            {
                var pom = item.Element("print").Element("carrier_id").Value;
                var pom1 = item.Element("card_id").Value;
                updateCarrier(pom, pom1, originalXML);
            }

            //  todo
            //  order by package id, then order in package

            // set new xml value
            setNewXmlValue(originalXML);
        }

        /// <param name="pom"></param> // carrier_id value
        /// <param name="pom1"></param> // card_id
        public void updateCarrier(string pom, string pom1, XElement originalXML)
        {
            // get specific carrier
            XElement pom2 = (from item in originalXML.Elements()
                             where (item.Name == "carrier" && item.Attributes("carrier_id").Count() != 0 && item.Attribute("carrier_id").Value == pom)
                             select item).FirstOrDefault();

            //update
            pom2.SetAttributeValue("card_id", pom1);
        }

        public void setNewXmlValue(XElement xml) { m_xml = xml; }

        public XElement getNewXMLValue() { return m_xml; }

    }
}
