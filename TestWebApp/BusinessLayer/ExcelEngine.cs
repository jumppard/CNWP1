using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OfficeOpenXml;

namespace TestWebApp.BusinessLayer
{
    public class ExcelEngine
    {
        /// <summary>
        /// Object from external library for manipulating excel files
        /// </summary>
        private ExcelPackage m_excelObject;

        /// <summary>
        /// _ctor
        /// </summary>
        public ExcelEngine()
        {
            Init();
        }

        /// <summary>
        /// Method for initialization of all variables
        /// </summary>
        private void Init()
        {
            m_excelObject = new ExcelPackage();
        }

        /// <summary>
        /// Method for creating excel file
        /// </summary>
        /// <param name="destinationPath">Destination path of generated excel file</param>
        /// <param name="fileName">File name for generated excel file</param>
        /// <param name="lastExcelFileName">File name of lastly created excel file</param>
        public void CreateExcelFile(string lastExcelFileName, string destinationPath, string fileName)
        {
            // 1. vytvor kopiu $lastExcelFileName v adresari $destinationPath s tym, ze mu pozmen meno na $fileName
            var pom1 = destinationPath  + lastExcelFileName;
            var pom2 = destinationPath  + fileName;
            System.IO.File.Copy(destinationPath + lastExcelFileName, destinationPath + fileName, true);
        }

        /// <summary>
        /// Method for editing specific excel cell.
        /// </summary>
        /// <param name="tabName">Name of excel file tab</param>
        /// <param name="column">Column id of excel file tab</param>
        /// <param name="row">Row id of excel file tab</param>
        /// <param name="newValue">New value of specific cell of specific excel file tab</param>
        public void EditCellValue(string tabName, int column, int row, string newValue) { }

        /// <summary>
        /// Method which returns the newest file name from specified folder
        /// </summary>
        /// <param name="folderName">Folder name</param>
        /// <returns>the newest file name from specified folder</returns>
        public string GetLastExcelFileName(string folderName)
        {
            throw new NotImplementedException();
        }
    }
}