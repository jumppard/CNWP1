using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Serilog;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Windows;

using OfficeOpenXml; // EEPlus
using OfficeOpenXml.Drawing; // EEPlus
using OfficeOpenXml.Style;

/// <summary>
/// This application mirrors REWARDO's TRANSACTION table to CARDNET's ANSXXX006 table. It only works with organization_id's transactions.
/// </summary>
namespace DeliveryListGenerationApp
{
    /// <summary>
    /// Class for logging operations. AppEngine<->FileManager == Composition
    /// </summary>
    public class MyLogger
    {
        private ILogger m_logger; // LOGGER

        public MyLogger()
        {
            m_logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.LiterateConsole()
            .WriteTo.RollingFile("logs\\DeliveryListGenerationApp-{Date}.txt")
            .CreateLogger();
        }

        public void setLogger(string logSentence, string typeOfMessage)
        {
            if (typeOfMessage == "information")
            {
                m_logger.Information(logSentence);
            }
            else if (typeOfMessage == "error")
            {
                m_logger.Error(logSentence);
            }
            else if (typeOfMessage == "debug")
            {
                m_logger.Debug(logSentence);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class FileManager
    {
        private string m_fileName { get; set; }
        private XElement m_xml { get; set; }
        private string m_pomXML { get; set; }

        public FileManager(string fileName)
        {
            m_fileName = fileName;
        }

        public XElement retrieveXML()
        {
            m_xml = XElement.Load(m_fileName);
            return m_xml;
        }

        public string getXMLString()
        {
            string xml = System.IO.File.ReadAllText(m_fileName);
            return xml;
        }
    }

    /// <summary>
    /// Class for maintaning exporting of cards to .xlsx, .pdf etc...
    /// </summary>
    public class ExportManager : MyLogger
    {
        // ***** NEW *****
        private ExcelPackage xlApp { get; set; }
        private ExcelWorksheet m_workSheet { get; set; }
        private FileInfo m_myFile { get; set; }

        private string m_sambaPath { get; set; }
        private string m_environment { get; set; }
        private string m_destinatinPath { get; set; }

        public ExportManager(string sambaPath, string environment, Tuple<string, string> dlTuple)
        {
            // initialize a worksheet
            if (dlTuple.Item2.Contains(";"))
            {
                int position = dlTuple.Item2.IndexOf(";");
                var dlTemplatename = dlTuple.Item2.Substring(0, position);
                m_myFile = new FileInfo(@AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\" + dlTemplatename /*"Předávací protokol_V4 MAUR (1).xlsx"*/);
            }
            else
            {
                m_myFile = new FileInfo(@AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\" + dlTuple.Item2 /*"Předávací protokol_V4 MAUR (1).xlsx"*/);
            }

            xlApp = new ExcelPackage(m_myFile);
            m_workSheet = xlApp.Workbook.Worksheets["Gusto Karta"];
            m_sambaPath = sambaPath;
            m_environment = environment;
        }

        public ExportManager()
        {
        }

        public void insertIntoWorkSheet(int rowKey, string columnKey, string value)
        {
            m_workSheet.Cells[columnKey+rowKey.ToString()].Value = value;
        }

        public void SaveWorksheet(string partnerName, long orderId, uint? packageId = null, string department = "")
        {
            var cardOrderId = orderId;
            var pathString = @m_sambaPath + @m_environment + @"\PARTNERS\" + partnerName + @"\" + "CARD_ORDERS" + @"\" + cardOrderId + @"\DELIVERY_LISTS\" + packageId;

            if (!Directory.Exists(pathString))
            {
                Directory.CreateDirectory(pathString);
            }
            // v tomto kroku je vytvorena zlozka do ktorej mozem zapisovat

            var envPom = m_environment.Substring(5).ToLower();

            // ***** NEW VERSION *****
            xlApp.SaveAs(new FileInfo(@pathString + @"\" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + partnerName + "_" + cardOrderId + "_" + packageId + "_" + department + "_" + envPom + ".xlsx"));

            m_destinatinPath = pathString + @"\" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + partnerName + "_" + cardOrderId + "_" + packageId + "_" + department + "_" + envPom + ".xlsx";
        }

        public void SetTemplate(string newTemplateName)
        {
            m_myFile = null;
            m_myFile = new FileInfo(@AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\" + newTemplateName);

            InitExcelObject();
            InitExcelWorksheetObject("SUMMARY");
        }

        public void InitExcelObject() { xlApp = new ExcelPackage(m_myFile); }
        public void InitExcelWorksheetObject(string workSheetName) { m_workSheet = xlApp.Workbook.Worksheets[workSheetName]; }

        public string getDestinationPath() { return m_destinatinPath; }
        public ExcelPackage GetExcel() { return xlApp; }
        public ExcelWorksheet GetExcelWorkSheet() { return m_workSheet; }
        public void ReleaseExcelWorksheet() { m_workSheet = null; /*m_workSheet.Dispose();*/ }
        public void ReleaseExcel() { xlApp = null; /*xlApp.Dispose();*/ }

        public void AddExcelRow(int rowIndex)
        {
            m_workSheet.InsertRow(rowIndex,1);
        }

        public void RemoveExcelRow(int rowIndex)
        {
            m_workSheet.DeleteRow(rowIndex, 1);
        }

        public void SetCellBorder(string borderStyle, int x, string y)
        {
            if (borderStyle == "thin")
            {
                m_workSheet.Cells[y+x.ToString()].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                m_workSheet.Cells[y+x.ToString()].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                m_workSheet.Cells[y+x.ToString()].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                m_workSheet.Cells[y+x.ToString()].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            }
        }

        public void SetFontStyle(string fontStyle, int fontSize, string alignment, int x, string y)
        {
            if (fontStyle == "bold")
                m_workSheet.Cells[y + x.ToString()].Style.Font.Bold = true;

            m_workSheet.Cells[y + x.ToString()].Style.Font.Size = fontSize;

            if (alignment == "center")
            {
                m_workSheet.Cells[y + x.ToString()].Style.HorizontalAlignment   = ExcelHorizontalAlignment.Center;
                m_workSheet.Cells[y + x.ToString()].Style.VerticalAlignment     = ExcelVerticalAlignment.Center;
            }
            else if (alignment == "left")
            {
                m_workSheet.Cells[y + x.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                m_workSheet.Cells[y + x.ToString()].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }
        }
    }

    /// <summary>
    /// Container of Card information
    /// </summary>
    public class Card
    {
        public uint packageId { get; set; }
        public uint orderInPackage { get; set; }
        public string employeePersonalNumber { get; set; }
        public string employeeName { get; set; }
        public string branch { get; set; }
    }

    /// <summary>
    /// Class specifies package in REQXXX002::content (XML) column according to package_id.
    /// </summary>
    public class Package
    {
        public uint m_ID { get; set; }
        public List<Card> m_cards { get; set; } // cards in package // dictionary key = {order_in_package, employee_id, employee_name, employee_surname, branch}
        public string m_address { get; set; }

        public Package(uint id)
        {
            m_ID = id;
            m_cards = new List<Card>();
            m_address = "";
        }

        public void add(Card newCard)
        {
            m_cards.Add(newCard);
        }
    }

    /// <summary>
    /// Main class
    /// </summary>
    public class AppEngine
    {
        private MyLogger m_myLogger { get; set; }
        private XElement m_inputXMLFile { get; set; }
        private Package m_package { get; set; } // initial package
        private ExportManager m_exportManager { get; set; }
        private int m_countOfPersonalizedCards { get; set; }
        private XmlReader m_xr { get; set; }

        private uint myPackageId { get; set; }
        private string m_partnerName { get; set; }
        private long m_orderId { get; set; }
        private Tuple<string, string> m_deliveryListTuple { get; set; }

        public AppEngine()
        {

        }

        public AppEngine(XElement p_inputXMLFile, uint p_packageId, string partnerName, long orderId, string sambaPath, string environment, /*string deliveryListType*/Tuple<string, string> dlTuple)
        {
            m_package = new Package(p_packageId);
            m_inputXMLFile = p_inputXMLFile;
            m_countOfPersonalizedCards = 0;
            m_partnerName = partnerName;
            m_orderId = orderId;
            m_deliveryListTuple = dlTuple;

            try
            {
                m_myLogger = new MyLogger();
                m_myLogger.setLogger("APPLICATION STARTED", "information");
                run(p_packageId, sambaPath, environment); // main functionality
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            m_myLogger.setLogger("APPLICATION ENDED", "information");

        } // end of AppEngine class

        public void retrievePackageData(uint p_packageId)
        {
            retrievePackage(p_packageId);
            retrievePackageCards(p_packageId);
        }

        private void retrievePackage(uint p_packageId)
        {
            // retrieve <package_distribution>s
            IEnumerable<XElement> pom = from members in m_inputXMLFile.Elements("package_distribution") select members;

            // retrieve specific <package_distribution>
            var elements = (from item in pom where Convert.ToUInt32(item.Attribute("package_id").Value) == p_packageId select item).FirstOrDefault();

            var address_person = "null";
            var address_phone = "null";

            if (elements != null)
            {
                //retrieve <package_address>
                var address_recipient = elements.Element("package_address").Element("address_recipient").Value.ToString();          /*(from item in elements select item.Element("package_address").Element("address_recipient").Value.ToString()).FirstOrDefault();*/
                if (elements.Element("package_address").Element("address_person") != null)
                {
                    address_person = elements.Element("package_address").Element("address_person").Value.ToString();            /*(from item in elements select item.Element("package_address").Element("address_person").Value.ToString()).FirstOrDefault(); // NOT REQUIRED*/
                }
                if (elements.Element("package_address").Element("address_phone") != null)
                {
                    address_phone = elements.Element("package_address").Element("address_phone").Value.ToString();              /*(from item in elements select item.Element("package_address").Element("address_person").Value.ToString()).FirstOrDefault(); // NOT REQUIRED*/
                }
                var address_country = elements.Element("package_address").Element("address_country").Value.ToString();              /*(from item in elements select item.Element("package_address").Element("address_country").Value.ToString()).FirstOrDefault();*/
                var address_city_zip = elements.Element("package_address").Element("address_city_zip").Value.ToString();      /*(from item in elements select item.Element("package_address").Element("address_city_zip").Value.ToString()).FirstOrDefault();*/
                var address_street = elements.Element("package_address").Element("address_street").Value.ToString();        /*(from item in elements select item.Element("package_address").Element("address_street").Value.ToString()).FirstOrDefault();*/
                var address_street_number = elements.Element("package_address").Element("address_street_number").Value.ToString();  /*(from item in elements select item.Element("package_address").Element("address_street_number").Value.ToString()).FirstOrDefault();*/


                if (address_person != "null" && address_phone != "null")
                {
                    m_package.m_address = address_recipient + ", " + address_person + ", " + address_phone + ", " + address_country + ", " + address_city_zip + ", " + address_street + ", " + address_street_number;
                }
                else if (address_person != "null" && address_phone == "null")
                {
                    m_package.m_address = address_recipient + ", " + address_person /*+ ", " + address_phone*/ + ", " + address_country + ", " + address_city_zip + ", " + address_street + ", " + address_street_number;
                }
                else if (address_person == "null" && address_phone != "null")
                {
                    m_package.m_address = address_recipient /*+ ", " + address_person*/ + ", " + address_phone + ", " + address_country + ", " + address_city_zip + ", " + address_street + ", " + address_street_number;
                }
                else if (address_person == "null" && address_phone == "null")
                {
                    m_package.m_address = address_recipient/* + ", " + address_person + ", " + address_phone*/ + ", " + address_country + ", " + address_city_zip + ", " + address_street + ", " + address_street_number;
                }
            }
        }

        /// <summary>
        /// retirieves all cards info from m_package
        /// </summary>
        /// <param name="p_packageId"></param>
        private void retrievePackageCards(uint p_packageId)
        {
            // retrieve <card>s
            IEnumerable<XElement> pom = from members in m_inputXMLFile.Elements("card") select members;

            var elements = (from item in pom where Convert.ToUInt32(item.Element("package_id").Value) == p_packageId select item);

            foreach (var card in elements)
            {
                addIntoPackage_Control(card);
            }
        }

        /// <summary>
        /// This method controlls if <param>card</param>'s child element "package_id" equals m_package.m_ID. If it equals => add <param>card</param> into m_package's cards list
        /// </summary>
        /// <param name="card"></param>
        private void addIntoPackage_Control(XElement card)
        {
            if (Convert.ToUInt32(card.Element("package_id").Value) == m_package.m_ID)
            {
                if (card.Elements("delivery_list").Any() && card.Element("delivery_list").HasElements) // if it is not an anonymous card && if it is not <delivery_list/>
                {
                    try
                    {
                        string pomEmployeePersonalNumber = "";
                        if (card.Element("delivery_list").Elements("dl_column2").Any())
                            pomEmployeePersonalNumber = card.Element("delivery_list").Element("dl_column2").Value.ToString();

                        Card pomCard = new Card()
                        {
                            packageId = Convert.ToUInt32(card.Element("package_id").Value),
                            orderInPackage = Convert.ToUInt32(card.Element("order_in_package").Value),
                            employeePersonalNumber = pomEmployeePersonalNumber, // minOccurs = 0
                            employeeName = card.Element("delivery_list").Element("dl_column3") != null ? card.Element("delivery_list").Element("dl_column3").Value.ToString() : "", // minOccurs = 0
                            branch = card.Element("delivery_list").Element("dl_column4") != null ? card.Element("delivery_list").Element("dl_column4").Value.ToString() : "", // minOccurs = 0
                        };

                        m_package.add(pomCard);
                    }
                    catch (Exception e)
                    {
                        m_myLogger.setLogger(e.Message, "error");
                    }
                }
                else // it is an anonymous card
                {
                    incrementPersonalizedCard();
                }
            }
        }

        private void incrementPersonalizedCard()
        {
            m_countOfPersonalizedCards += 1;
        }

        /// <summary>
        /// Method for exporting cards of specific package to OUTPUT. F.e., from application to file.xlsx
        /// </summary>
        public void exportPackageCards(string sambaPath, string environment)
        {

            m_exportManager = new ExportManager(sambaPath, environment, m_deliveryListTuple);

            if (m_deliveryListTuple.Item1 == "aaa_DN2" || m_deliveryListTuple.Item1 == "bbb_DN2")
            {
                // TODO 
                GenerateHruskaDeliveryList(m_package.m_ID);
                
                // TODO
                GenerateHruskaSummaryList();
            }
            else
            {
                GenerateClassicDeliveryList(m_package.m_ID);
            }
        }

        private void GenerateHruskaSummaryList()
        {
            if (m_deliveryListTuple.Item2.Contains(";"))
            {
                var position = m_deliveryListTuple.Item2.IndexOf(";");
                var newTemplateName = m_deliveryListTuple.Item2.Substring(position + 1);

                m_exportManager.SetTemplate(newTemplateName);

                var orderNumbers = new Dictionary<string, string>();
                var pomPackages = m_inputXMLFile.Elements("package_distribution").ToList();
            
                // Dictionary, key = dep id; value = list of card_id(s)
                var pomDict = new Dictionary<string, Dictionary<string,string>>();

                foreach (var package in pomPackages)
                {
                    var pomOrerNumber   = package.Element("delivery_list").Element("dl_row1").Value;
                    var pomCode         = package.Element("delivery_list").Element("dl_row4").Value;

                    orderNumbers.Add(pomOrerNumber,pomCode);

                    var pomList = m_inputXMLFile.Elements("card").Where(x => x.Element("package_id").Value == package.Attribute("package_id").Value);
                    foreach (var card in pomList.ToList())
                        if (pomDict.ContainsKey(card.Element("delivery_list").Element("dl_column4").Value)) // IF pomDict contains the given key THEN append card_id to the list
                        {
                            var pomcardCount = Convert.ToInt32(pomDict[card.Element("delivery_list").Element("dl_column4").Value]["cardCount"])+1; // increment
                            pomDict[card.Element("delivery_list").Element("dl_column4").Value]["cardCount"] = pomcardCount.ToString();
                        }
                        else // add key + value
                        {
                            var pom = new Dictionary<string, string>(); // key = column name; value = column value
                            pom.Add("cardCount","1");

                            var pomMesto = m_inputXMLFile.Elements("department").Where(d => d.Attribute("department_id").Value == card.Element("delivery_list").Element("dl_column4").Value).First();
                            pom.Add("mesto",pomMesto.Element("dep_row2").Value); // mesto
                            pom.Add("ulice", pomMesto.Element("dep_row3").Value); // ulice
                            pom.Add("nazev", pomMesto.Element("dep_row1").Value); // nazev
                            pomDict.Add(card.Element("delivery_list").Element("dl_column4").Value, pom);
                        }
                }

                GenerateNewHruskaSummaryList(orderNumbers,pomDict);
            }
        }

        private void GenerateNewHruskaSummaryList(Dictionary<string, string> orderNumbers, Dictionary<string, Dictionary<string, string>> pomDict)
        {
            List<string> columns = new List<string>() { "A", "B", "C", "D", "E", "F", "G" };


            // ZOZNAM ZASIELOK
            int i = 6;
            foreach (var orderNum in orderNumbers.OrderBy(o=>o.Key))
            {
                m_exportManager.insertIntoWorkSheet(i, columns[1], orderNum.Key);
                m_exportManager.SetCellBorder("thin", i, columns[1]);
                m_exportManager.SetFontStyle("bold", 11, "center", i, columns[1]);

                m_exportManager.insertIntoWorkSheet(i, columns[2], orderNum.Value);
                m_exportManager.SetCellBorder("thin", i, columns[2]);
                m_exportManager.SetFontStyle("bold", 11, "center", i, columns[2]);

                m_exportManager.AddExcelRow(i+1);
                i += 1;
            }
            m_exportManager.RemoveExcelRow(i);


            // ZOZNAM ODDELENI
            int i2 = i+2;
            int countOfAllCards = 0;
            foreach (var department in pomDict.OrderBy(o=>o.Key))
            {
                m_exportManager.insertIntoWorkSheet(i2, columns[0], department.Key);
                m_exportManager.SetCellBorder("thin", i2, columns[0]);

                m_exportManager.insertIntoWorkSheet(i2, columns[1], department.Value["nazev"]);
                m_exportManager.SetCellBorder("thin", i2, columns[1]);

                m_exportManager.insertIntoWorkSheet(i2, columns[2], department.Value["mesto"]);
                m_exportManager.SetCellBorder("thin", i2, columns[2]);

                m_exportManager.insertIntoWorkSheet(i2, columns[3], department.Value["ulice"]);
                m_exportManager.SetCellBorder("thin", i2, columns[3]);

                m_exportManager.insertIntoWorkSheet(i2, columns[4], department.Value["cardCount"]);
                m_exportManager.SetCellBorder("thin", i2, columns[4]);
                m_exportManager.SetCellBorder("thin", i2, columns[5]);

                countOfAllCards += Convert.ToInt32(department.Value["cardCount"]);

                m_exportManager.AddExcelRow(i2+1);

                i2 += 1;
            }
            m_exportManager.RemoveExcelRow(i2);

            m_exportManager.insertIntoWorkSheet(i2, columns[2], pomDict.Keys.Count.ToString()); // CELKEM STREDISEK
            m_exportManager.insertIntoWorkSheet(i2, columns[4], countOfAllCards.ToString()); // CELKEM KARET

            m_exportManager.SaveWorksheet(m_partnerName, m_orderId);

            m_exportManager.ReleaseExcelWorksheet(); // null, dispose
            m_exportManager.ReleaseExcel(); // null, dispose

        }

        /// <summary>
        /// Retrieves all necessary data for following generation of delivery list for Hruska card product.
        /// </summary>
        /// <param name="p_packageId"></param>
        private void GenerateHruskaDeliveryList(uint p_packageId)
        {
            // header
            IEnumerable<XElement> pom = from members in m_inputXMLFile.Elements("package_distribution") select members;
            var elements = (from item in pom where Convert.ToUInt32(item.Attribute("package_id").Value) == p_packageId select item).FirstOrDefault();

            // Dictionary, key = dep id; value = list of card_id(s)
            var pomDict = new Dictionary<string, List<int>>();
            var pomList = m_inputXMLFile.Elements("card").Where(x => x.Element("package_id").Value == p_packageId.ToString());
            foreach (var card in pomList.ToList())
                if (pomDict.ContainsKey(card.Element("delivery_list").Element("dl_column4").Value)) // IF pomDict contains the given key THEN append card_id to the list
                    pomDict[card.Element("delivery_list").Element("dl_column4").Value].Add(Convert.ToInt32(card.Element("card_id").Value));
                else // add key + value
                {
                    var pomList1 = new List<int>();
                    var pomCardId = Convert.ToInt32(card.Element("card_id").Value);
                    pomList1.Add(pomCardId);
                    pomDict.Add(card.Element("delivery_list").Element("dl_column4").Value, pomList1);
                }

            // foreach pomDict => GenerateDeliveryList
            int x1 = 0;
            foreach (var pomDictItem in pomDict)
            {   
                GenerateNewHruskaDeliveryList(pomDictItem, p_packageId);
                x1 += 1;
            }
        }

        /// <summary>
        /// Generates delivery list for Hruska card product
        /// </summary>
        /// <param name="departmentDictionary"></param>
        /// <param name="p_packageId"></param>
        private void GenerateNewHruskaDeliveryList(KeyValuePair<string, List<int>> departmentDictionary, uint p_packageId) // string =  deparment name; List<int> = cards from department
        {

            if (m_exportManager.GetExcel() == null)
                m_exportManager.InitExcelObject();

            if (m_exportManager.GetExcelWorkSheet() == null)
                m_exportManager.InitExcelWorksheetObject("GUSTO KARTA");


            List<string> columns = new List<string>() { "A", "B", "C", "D", "E", "F", "G" };

            // DELIVERY LIST HEADER
            var pomCisloObj = m_inputXMLFile.Elements("package_distribution").Where(e => e.Attribute("package_id").Value == p_packageId.ToString()).Select(e => e.Element("delivery_list").Element("dl_row3").Value).First(); // cisloObj
            var pomDep = m_inputXMLFile.Elements("department").Where(e => e.Attribute("department_id").Value == departmentDictionary.Key).Select(e => e).First();
            m_exportManager.insertIntoWorkSheet(5, columns[1], departmentDictionary.Value.Count().ToString()); // pocet karet
            m_exportManager.insertIntoWorkSheet(6, columns[1], pomCisloObj); // cislo obj
            m_exportManager.insertIntoWorkSheet(6, columns[3], departmentDictionary.Key); // stredisko
            m_exportManager.insertIntoWorkSheet(7, columns[1], pomDep.Element("dep_row1").Value); // nazev
            m_exportManager.insertIntoWorkSheet(8, columns[1], pomDep.Element("dep_row2").Value); // mesto

            // DELIVERY LIST BODY
            int i = 12;
            int j = 0;

            var sortedList = from element in m_inputXMLFile.Elements("card").ToList()
                             where element.Element("package_id").Value == p_packageId.ToString() && element.Element("delivery_list").Element("dl_column4").Value == departmentDictionary.Key
                             orderby element.Element("order_in_package").Value
                             select element;

            foreach (var item in sortedList)
            {
                m_exportManager.insertIntoWorkSheet(i, columns[j], item.Element("order_in_package").Value.ToString());
                m_exportManager.SetCellBorder("thin", i, columns[j]);
                m_exportManager.SetFontStyle("", 11, "left", i, columns[j]);
                j += 1;
                m_exportManager.insertIntoWorkSheet(i, columns[j], item.Element("delivery_list").Element("dl_column2")/*employeePersonalNumber*/.Value.ToString());
                m_exportManager.SetCellBorder("thin", i, columns[j]);
                m_exportManager.SetFontStyle("", 11, "left", i, columns[j]);
                j += 1;
                m_exportManager.insertIntoWorkSheet(i, columns[j], item.Element("delivery_list").Element("dl_column3")/*employeeName*/.Value.ToString());
                m_exportManager.SetCellBorder("thin", i, columns[j]);
                m_exportManager.SetFontStyle("", 11, "left", i, columns[j]);
                j += 1;
                m_exportManager.insertIntoWorkSheet(i, columns[j], item.Element("delivery_list").Element("dl_column4")/*branch*/.Value.ToString());
                m_exportManager.SetCellBorder("thin", i, columns[j]);
                m_exportManager.SetFontStyle("", 11, "left", i, columns[j]);
                j += 1;
                m_exportManager.SetCellBorder("thin", i, columns[j]);
                m_exportManager.SetFontStyle("", 11, "left", i, columns[j]);

                j = 0;
                i += 1;
            }

            m_exportManager.SaveWorksheet(m_partnerName, m_orderId, m_package.m_ID, departmentDictionary.Key);


            m_exportManager.ReleaseExcelWorksheet(); // null, dispose
            m_exportManager.ReleaseExcel(); // null, dispose
        }

        /// <summary>
        /// Generates classic delivery list
        /// </summary>
        private void GenerateClassicDeliveryList(uint p_packageId)
        {
            List<string> columns = new List<string>() { "A","B","C","D","E","F","G" };

            int i = 14; // column = 14
            int j = 0; // B

            // sort cards by orderInPackage
            m_package.m_cards = m_package.m_cards.OrderBy(o => o.orderInPackage).ToList();

            m_exportManager.insertIntoWorkSheet(9, columns[2], m_countOfPersonalizedCards.ToString());
            m_exportManager.insertIntoWorkSheet(10, columns[2], (m_package.m_cards.Count + m_countOfPersonalizedCards).ToString());

            foreach (var item in m_package.m_cards)
            {
                m_exportManager.insertIntoWorkSheet(i, columns[j], item.orderInPackage.ToString());
                m_exportManager.SetCellBorder("thin",i,columns[j]);
                m_exportManager.SetFontStyle("",11,"left",i,columns[j]);
                j += 1;
                m_exportManager.insertIntoWorkSheet(i, columns[j], item.employeePersonalNumber.ToString());
                m_exportManager.SetCellBorder("thin", i, columns[j]);
                m_exportManager.SetFontStyle("", 11, "left", i, columns[j]);
                j += 1;
                m_exportManager.insertIntoWorkSheet(i, columns[j], item.employeeName.ToString());
                m_exportManager.SetCellBorder("thin", i, columns[j]);
                m_exportManager.SetFontStyle("", 11, "left", i, columns[j]);
                j += 1;
                m_exportManager.insertIntoWorkSheet(i, columns[j], item.branch.ToString());
                m_exportManager.SetCellBorder("thin", i, columns[j]);
                m_exportManager.SetFontStyle("", 11, "left", i, columns[j]);
                j += 1;
                m_exportManager.SetCellBorder("thin", i, columns[j]);
                m_exportManager.SetFontStyle("", 11, "left", i, columns[j]);

                j = 0;
                i += 1;
            }

            // insert header
            IEnumerable<XElement> pom = from members in m_inputXMLFile.Elements("package_distribution") select members;
            var elements = (from item in pom where Convert.ToUInt32(item.Attribute("package_id").Value) == p_packageId select item).FirstOrDefault();

            string fooPom = "";
            if (elements.Element("delivery_list").Element("dl_row2") != null)
            {
                fooPom = elements.Element("delivery_list").Element("dl_row2").Value.ToString();
            }

            m_exportManager.insertIntoWorkSheet(5, columns[2], m_package.m_address);
            m_exportManager.insertIntoWorkSheet(8, columns[4], elements.Element("delivery_list").Element("dl_row1").Value.ToString());
            m_exportManager.insertIntoWorkSheet(6, columns[2], fooPom);
            m_exportManager.insertIntoWorkSheet(9, columns[4], elements.Element("delivery_list").Element("dl_row4").Value.ToString());
            m_exportManager.insertIntoWorkSheet(7, columns[2], elements.Element("delivery_list").Element("dl_row3").Value.ToString());
            m_exportManager.insertIntoWorkSheet(8, columns[2], elements.Element("delivery_list").Element("dl_row5").Value.ToString());

            m_exportManager.SaveWorksheet(m_partnerName, m_orderId, m_package.m_ID);
        }

        public uint getMyPackageId(uint myNewPackageId)
        {
            return myNewPackageId;
        }

        /// <summary>
        /// Main RUN() method. In this method will be whole application logic.
        /// </summary>
        public void run(uint p_packageId, string sambaPath, string environment)
        {
            retrievePackageData(p_packageId);
            exportPackageCards(sambaPath, environment);
        }

        public static void Main(string[] args)
        {

            //FileManager fm1 = new FileManager("xmlExample.xml");

            //Console.Write("package_id = ");
            //uint myPackageId = Convert.ToUInt32(Console.ReadLine());

            //AppEngine appE = new AppEngine(fm1.retrieveXML(),myPackageId/*,fm1.getXMLString()*/);

            // --------------------
            //var pomXElement = XElement.Parse(args[0]);
            //AppEngine appE = new AppEngine(pomXElement, Convert.ToUInt32(args[1])/*,fm1.getXMLString()*/);
            // --------------------

            //Console.ReadKey();
        }

        public ExportManager getExportManager() { return m_exportManager; }
    }
}
