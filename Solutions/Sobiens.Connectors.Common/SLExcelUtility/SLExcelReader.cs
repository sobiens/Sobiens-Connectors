using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;

namespace Sobiens.Connectors.Common.SLExcelUtility
{
    public class SLExcelReader
    {
        private string GetColumnName(string cellReference)
        {
            var regex = new Regex("[A-Za-z]+");
            var match = regex.Match(cellReference);

            return match.Value;
        }

        private int ConvertColumnNameToNumber(string columnName)
        {
            var alpha = new Regex("^[A-Z]+$");
            if (!alpha.IsMatch(columnName)) throw new ArgumentException();

            char[] colLetters = columnName.ToCharArray();
            Array.Reverse(colLetters);

            var convertedValue = 0;
            for (int i = 0; i < colLetters.Length; i++)
            {
                char letter = colLetters[i];
                int current = i == 0 ? letter - 65 : letter - 64; // ASCII 'A' = 65
                convertedValue += current * (int)Math.Pow(26, i);
            }

            return convertedValue;
        }

        private IEnumerator<Cell> GetExcelCellEnumerator(Row row)
        {
            int currentCount = 0;
            foreach (Cell cell in row.Descendants<Cell>())
            {
                string columnName = GetColumnName(cell.CellReference);

                int currentColumnIndex = ConvertColumnNameToNumber(columnName);

                for (; currentCount < currentColumnIndex; currentCount++)
                {
                    var emptycell = new Cell() { DataType = null, CellValue = new CellValue(string.Empty) };
                    yield return emptycell;
                }

                yield return cell;
                currentCount++;
            }
        }

        private string ReadExcelCell(Cell cell, WorkbookPart workbookPart)
        {
            var cellValue = cell.CellValue;
            var text = (cellValue == null) ? cell.InnerText : cellValue.Text;
            if ((cell.DataType != null) && (cell.DataType == CellValues.SharedString))
            {
                text = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(
                        Convert.ToInt32(cell.CellValue.Text)).InnerText;
            }

            return (text ?? string.Empty).Trim();
        }

        public SLExcelData ReadExcel(string filePath)
        {
            var data = new SLExcelData();
            /*
            // Check if the file is excel
            if (file.ContentLength <= 0)
            {
                data.Status.Message = "You uploaded an empty file";
                return data;
            }

            if (file.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                data.Status.Message = "Please upload a valid excel file of version 2007 and above";
                return data;
            }
            */

            // Open the excel document
            WorkbookPart workbookPart; List<Row> rows;
            try
            {
                //var document = SpreadsheetDocument.Open(file.InputStream, false);
                var document = SpreadsheetDocument.Open(filePath, false);
                workbookPart = document.WorkbookPart;

                var sheets = workbookPart.Workbook.Descendants<Sheet>();
                var sheet = sheets.First();
                data.SheetName = sheet.Name;

                var workSheet = ((WorksheetPart)workbookPart.GetPartById(sheet.Id)).Worksheet;
                var columns = workSheet.Descendants<Columns>().FirstOrDefault();
                data.ColumnConfigurations = columns;

                var sheetData = workSheet.Elements<SheetData>().First();
                rows = sheetData.Elements<Row>().ToList();
            }
            catch (Exception e)
            {
                data.Status.Message = "Unable to open the file";
                return data;
            }

            // Read the header
            if (rows.Count > 0)
            {
                var row = rows[0];
                var cellEnumerator = GetExcelCellEnumerator(row);
                while (cellEnumerator.MoveNext())
                {
                    var cell = cellEnumerator.Current;
                    var text = ReadExcelCell(cell, workbookPart).Trim();
                    data.Headers.Add(text);
                }
            }

            // Read the sheet data
            if (rows.Count > 1)
            {
                for (var i = 1; i < rows.Count; i++)
                {
                    var dataRow = new List<string>();
                    data.DataRows.Add(dataRow);
                    var row = rows[i];
                    var cellEnumerator = GetExcelCellEnumerator(row);
                    while (cellEnumerator.MoveNext())
                    {
                        var cell = cellEnumerator.Current;
                        var text = ReadExcelCell(cell, workbookPart).Trim();
                        dataRow.Add(text);
                    }
                }
            }

            return data;
        }

        public SLExcelData ReadExcel(string filePath, bool hasHeader)
        {
            return ReadExcel(filePath, string.Empty, hasHeader);
        }

        public SLExcelData ReadExcel(string filePath, string sheetTitle, bool hasHeader)
        {
            var data = new SLExcelData();

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (SpreadsheetDocument doc = SpreadsheetDocument.Open(fs, false))
                {
                    // Open the excel document
                    List<Row> rows;

                    WorkbookPart workbookPart = doc.WorkbookPart;
                    var sheets = workbookPart.Workbook.Descendants<Sheet>();
                    var sheet = sheets.First();

                    if (string.IsNullOrEmpty(sheetTitle) == false)
                    {
                        var query1 = from _sheet in workbookPart.Workbook.Descendants<Sheet>()
                                     where _sheet.Name.Value.ToString().Equals(sheetTitle, StringComparison.InvariantCultureIgnoreCase)
                                     select _sheet;
                        sheet = query1.FirstOrDefault();
                    }

                    data.SheetName = sheet.Name;

                    var workSheet = ((WorksheetPart)workbookPart.GetPartById(sheet.Id)).Worksheet;
                    var columns = workSheet.Descendants<Columns>().FirstOrDefault();
                    data.ColumnConfigurations = columns;


                    var sheetData = workSheet.Elements<SheetData>().First();
                    rows = sheetData.Elements<Row>().ToList();
                    int dataStartIndex = 0;

                    // Read the header
                    if (rows.Count > 0 && hasHeader == true)
                    {
                        var row = rows[0];
                        var cellEnumerator = GetExcelCellEnumerator(row);
                        while (cellEnumerator.MoveNext())
                        {
                            var cell = cellEnumerator.Current;
                            var text = ReadExcelCell(cell, workbookPart).Trim();
                            data.Headers.Add(text);
                        }
                        dataStartIndex = 1;
                    }

                    // Read the sheet data
                    if (rows.Count > dataStartIndex)
                    {
                        for (var i = dataStartIndex; i < rows.Count; i++)
                        {
                            var dataRow = new List<string>();
                            data.DataRows.Add(dataRow);
                            var row = rows[i];
                            var cellEnumerator = GetExcelCellEnumerator(row);
                            while (cellEnumerator.MoveNext())
                            {
                                var cell = cellEnumerator.Current;
                                var text = ReadExcelCell(cell, workbookPart).Trim();
                                dataRow.Add(text);
                            }
                        }
                    }

                    return data;
                }
            }
        }

    }
}