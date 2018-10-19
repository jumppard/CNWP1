using System;
using System.Xml.Linq;
using FastReport;
using FastReport.Export;
using System.IO;
using System.Web;

namespace TestWebApp.ExportCarriers
{
    /// <summary>
    /// Implements integration (initialization and usage) of Fast Report. 
    /// </summary>
    public class MyFastReport
    {
        private ExportedFileNameBuilder m_fileNameBuilder { get; set; }
        private XElement m_xmlForfastReport { get; set; }
        private string m_fastReportTemplateLocation { get; set; }
        private string m_fastReportTemplateName { get; set; }
        private string m_destinationFilePath { get; set; }

        public MyFastReport(XElement xmlForFastReport, string fastReportTemplateLocation, string fastReportTemplateName, ExportedFileNameBuilder fileNameBuilder)
        {
            Init(xmlForFastReport, fastReportTemplateLocation, fastReportTemplateName, fileNameBuilder);
        }

        private void Init(XElement xmlForFastReport, string fastReportTemplateLocation, string fastReportTemplateName, ExportedFileNameBuilder fileNameBuilder)
        {
            m_xmlForfastReport = xmlForFastReport;
            m_fastReportTemplateLocation = fastReportTemplateLocation;
            m_fastReportTemplateName = fastReportTemplateName;
            m_destinationFilePath = "";
            m_fileNameBuilder = fileNameBuilder;
        }

        /// <summary>
        /// The logic.
        /// </summary>
        public void Run()
        {
            ExportReport();
        }

        /// <summary>
        /// Report generation process.
        /// </summary>
        private void ExportReport()
        {
            try
            {
                //var filePath = HttpContext.Current.Request.PhysicalApplicationPath + "App_Data\\" + "xmlForFastReport.xml";
                //m_xmlForfastReport.Save(filePath);


                // report init
                Report report = new Report();

                // bind data
                System.Data.DataSet dataSet = new System.Data.DataSet();
                dataSet.ReadXml(new StringReader(new XElement(m_xmlForfastReport).ToString()));
                //dataSet.ReadXml(filePath);
                report.Report.RegisterData(dataSet);

                // load report template and prepare the final report
                report.Load(m_fastReportTemplateLocation + m_fastReportTemplateName);
                report.Prepare(); // tuto to hadze chybu

                FastReport.Export.Pdf.PDFExport export = new FastReport.Export.Pdf.PDFExport();

                // save generated report to disk
                if (!Directory.Exists(m_fileNameBuilder.GetDestinationPath())) // ak priecinok zatial neexistuje -> vytvor ho
                    Directory.CreateDirectory(m_fileNameBuilder.GetDestinationPath());

                m_destinationFilePath = m_fileNameBuilder.GetDestinationPath() + @DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + m_fileNameBuilder.GetPartnerName() + "_" + m_fileNameBuilder.GetCardOrderId() + "_" + m_fileNameBuilder.GetEnvironment() + ".pdf";

                export.Export(report, m_destinationFilePath);
                export = null; // ??? necessary ???
                report = null; // ??? necessary ???
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public string GetDestinationFilePath() { return m_destinationFilePath; }
    }
}
