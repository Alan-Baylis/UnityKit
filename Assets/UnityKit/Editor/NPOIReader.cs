using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;

namespace UnityKit.Editor {
    public class NPOIReader : ITableReader {
        public Table Read(string excelFile) {
            if (!File.Exists(excelFile)) {
                throw new Exception("Excel file is not found");
            }

            string[,] data = null;
            using (var fs = new FileStream(excelFile, FileMode.Open, FileAccess.Read)) {
                IWorkbook book = excelFile.EndsWith(".xlsx") ? new XSSFWorkbook(fs) as IWorkbook : new HSSFWorkbook(fs) as IWorkbook;
                var sheet = book.GetSheetAt(0);
                int nrows = sheet.LastRowNum + 1;
                int ncols = sheet.GetRow(0).LastCellNum;
                data = new string[nrows, ncols];

                for (int rowIndex = 0; rowIndex < nrows; rowIndex++) {
                    var rowItems = sheet.GetRow(rowIndex);
                    var count = rowItems.LastCellNum;
                    for (int colIndex = 0; colIndex < ncols; colIndex++) {
                        if (colIndex >= count) {
                            break;
                        }
                        data[rowIndex, colIndex] = CellValue2Str(rowItems.GetCell(colIndex));
                    }
                }
                book.Close();
            }
            return new Table(data) { name = excelFile };
        }

        static string CellValue2Str(ICell cell) {
            if (null == cell) return string.Empty;
            if (cell.CellType == CellType.Blank) return string.Empty;
            if (cell.CellType == CellType.Boolean) return System.Convert.ToString(cell.BooleanCellValue);
            if (cell.CellType == CellType.Numeric) return System.Convert.ToInt32(cell.NumericCellValue).ToString();
            if (cell.CellType == CellType.String) return cell.StringCellValue;
            return string.Empty;
        }
    }
}
