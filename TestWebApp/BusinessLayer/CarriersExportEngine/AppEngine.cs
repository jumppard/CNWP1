using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Schema;

namespace TestWebApp.ExportCarriers
{
    /// <summary>
    /// The Application Engine. The logic/design of the application.
    /// </summary>
    public class AppEngine
    {
        private ExportedFileNameBuilder m_fileNameBuilder { get; set; }
        private TestWebApp.DataAccessLayer.CardnetDAL m_dbEngine { get; set; }
        private CarriersExportEngine m_carriersExportEngine { get; set; } // carriers export engine
        private MyFastReport m_fastReportEngine { get; set; } // fast report engine
        private KeyValuePair<string, Dictionary<string, string>> m_exportSubject; // key = {"carrier", "delivery_list" ...}; value = dict with key variableName {exportConstantName, exportConstantValue, templateLocation, oldXml, partnerName, dstPath, cardOrderId, xmlForfastReport, environment} and value variableValue 

        /*------------------------------------*/
        /*------------------------------------*/
        public AppEngine(){}

        public AppEngine(KeyValuePair<string,Dictionary<string,string>> exportSubject, TestWebApp.DataAccessLayer.CardnetDAL db)
        {
            Init(exportSubject, db);
        }

        private void Init(KeyValuePair<string, Dictionary<string, string>> exportSubject, TestWebApp.DataAccessLayer.CardnetDAL db)
        {
            m_exportSubject = exportSubject;
            m_dbEngine = db;
            m_fileNameBuilder = new ExportedFileNameBuilder(destinationPath: exportSubject.Value["dstPath"].ToString(), partnerName: exportSubject.Value["partnerName"].ToString(), cardOrderId: exportSubject.Value["cardOrderId"].ToString(), environment: exportSubject.Value["environment"].ToString());
            if (exportSubject.Key == "carrier")
            {
                m_carriersExportEngine = new CarriersExportEngine(oldXml: XElement.Parse(exportSubject.Value["oldXml"].ToString()), xmlForFastReport: XElement.Parse(exportSubject.Value["xmlForFastReport"].ToString()));
            }
            else if (exportSubject.Key == "delivery_list")
            {
                // TODO do buducna
            }
        }

        /// <summary>
        /// Checks the validity of input against scheme 
        /// </summary>
        /// <param name="exportSubject"></param>
        /// <returns></returns>
        private bool CheckValidity(KeyValuePair<string, Dictionary<string, string>> exportSubject)
        {
            bool result = true;

            //if (exportSubject.Key == "carrier")
            //{
            //    //var key = exportSubject.Key;
            //    //var constantValue = exportSubject.Value["exportConstantValue"].ToString();

            //    //var x = m_dbEngine.getCardnetDB().C___EXPORT_MAP.Where(em => em.CONSTANT_NAME == key && em.CONSTANT_VALUE == constantValue).FirstOrDefault();

            //    ////  1.  validate xml against XSD
            //    ////  2.  if valid    => return true
            //    ////      else        => return false
            //    //if (x != null)
            //    //{
            //    var xdoc = XDocument.Parse(exportSubject.Value["oldXml"].ToString());
            //    XmlSchemaSet schemas = new XmlSchemaSet();

            //    var pomSchemaLocation = exportSubject.Value["schemaLocation"];

            //    schemas.Add("", pomSchemaLocation);
            //    xdoc.Validate(schemas, (o, e) =>
            //    {
            //        result = false;
            //    });
            //    //}
            //    //else
            //    //    result = false;
            //}
            //else if (exportSubject.Key == "delivery_list")
            //{
            //    // TODO do buducna
            //}

            return result;
        }
        /*------------------------------------*/
        /*------------------------------------*/

        //private void Init(string templateLocation, string exportConstantName, string exportConstantValue, string dstPath, string partnerName, long cardOrderId, string environment, DatabaseEngine dbEngine, XElement oldXml = null, XElement xmlForFastReport = null)
        //{
        //    m_exportConstantName = exportConstantName;
        //    m_exportConstantValue = exportConstantValue;
        //    m_templateLocation = templateLocation;
        //    m_dbEngine = new DatabaseEngine(); 
        //    m_fileNameBuilder = new ExportedFileNameBuilder(destinationPath: dstPath, partnerName: partnerName, cardOrderId: cardOrderId.ToString(), environment: environment);
        //    m_carriersExportEngine = new CarriersExportEngine(oldXml: oldXml, xmlForFastReport: xmlForFastReport);
        //}

        /// <summary>
        /// The logic. It's called from another app.
        /// </summary>
        public void Run()
        {
            if (m_exportSubject.Key == "carrier")
            {
                if (CheckValidity(m_exportSubject))
                {
                    var key = m_exportSubject.Key;
                    var constantValue = m_exportSubject.Value["exportConstantValue"].ToString();

                    var pom = m_dbEngine.getCardnetDB().C___EXPORT_MAP.Where(em => em.CONSTANT_NAME == key && em.CONSTANT_VALUE == constantValue).First(); // nemoze sa stat v tomto kroku ze by sa nenasla ziadna hodnota
                    var fastReportTemplateName = "";

                    if (pom != null)
                        fastReportTemplateName = pom.TEMPLATE_FILE_NAME;

                    m_fastReportEngine = new MyFastReport(m_carriersExportEngine.GetXmlForFastReport(), m_exportSubject.Value["templateLocation"].ToString(), fastReportTemplateName, m_fileNameBuilder);
                    m_fastReportEngine.Run();
                }
            }
            else if(m_exportSubject.Value["exportConstantName"].ToString() == "delivery_list")
            {
                // TODO do buducna
            }
        }

        public MyFastReport GetMyFastReport() { return m_fastReportEngine; }
        public ExportedFileNameBuilder GetFileNameBuilder() { return m_fileNameBuilder; }
        public CarriersExportEngine GetCarriersExportEngine() { return m_carriersExportEngine; }
        public KeyValuePair<string, Dictionary<string,string>> GetExportSubject() { return m_exportSubject; }
    }
}
