using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Runtime.InteropServices;

namespace POUtilityTool.ViewModels.Helpers
{
    public class ExcelHelper
    {
        // Memory Tips: 1 dot per varible and Marshal.ReleaseComObject() each varible
        private Excel.Application excelApp;
        private Excel.Workbooks workbooksAPP;
        public Excel._Workbook excelWorkbook;

        public ExcelHelper(string filepath)
        {
            try
            {
                excelApp = new Excel.Application();
                workbooksAPP = (Excel.Workbooks)excelApp.Workbooks;
                excelWorkbook = (Excel._Workbook)(workbooksAPP.Open(filepath));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddFeaturesToSheet(string sheetName, List<Feature> features)
        {
            int row_num = 2; //Skip header which already exist in the file

            Excel.Worksheet sheet = (Excel.Worksheet)excelWorkbook.Sheets[sheetName];

            foreach (var feature in features)
            {
                sheet.Cells[row_num, 1] = feature.Id;
                sheet.Cells[row_num, 4] = feature.name;
                sheet.Cells[row_num, 6] = feature.userStoryCount;
                sheet.Cells[row_num, 7] = feature.userStoryClosedPercentage;
                sheet.Cells[row_num, 8] = feature.technicalStoryCount;
                sheet.Cells[row_num, 9] = feature.technicalStoryClosedPercentage;
                sheet.Cells[row_num, 10] = feature.spikeCount;
                sheet.Cells[row_num, 11] = feature.spikeClosedPercentage;
                sheet.Cells[row_num, 12] = feature.activeWorkItems;
                row_num++;
            }


        }

        public void AddItemstoSheet(string sheetName, List<Work_Item> workItemList)
        {
            int row_num = 2; //Skip header which already exist in the file

            Excel.Worksheet sheet = (Excel.Worksheet)excelWorkbook.Sheets[sheetName];

            foreach (var workitem in workItemList)
            {
                sheet.Cells[row_num, 1] = workitem.Id;
                sheet.Cells[row_num, 2] = workitem.POStatus;
                sheet.Cells[row_num, 3] = workitem.Title;
                sheet.Cells[row_num, 4] = workitem.StoryPoints;
                row_num++;
            }

            Marshal.ReleaseComObject(sheet); //help with releasing Excel from memory
            sheet = null; //help with releasing Excel from memory
        }

        public void CloseExcel()
        {
            //To clean Excel from memory
            //Garbage Collector
            GC.Collect();
            GC.WaitForPendingFinalizers();

            if (excelWorkbook != null)
            {
                excelWorkbook.Save();
                excelWorkbook.Close();
                Marshal.ReleaseComObject(excelWorkbook);
                excelWorkbook = null;
                Marshal.ReleaseComObject(workbooksAPP);
                workbooksAPP = null;

            }

            if (excelApp != null)
            {
                excelApp.Quit();
                Marshal.ReleaseComObject(excelApp);
                excelApp = null;
            }
        }
    }
}
