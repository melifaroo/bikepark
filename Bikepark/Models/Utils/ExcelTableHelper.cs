using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using X14 = DocumentFormat.OpenXml.Office2010.Excel;
using X15 = DocumentFormat.OpenXml.Office2013.Excel;

namespace Bikepark.Models
{
    public class ExcelTableHelper
    {

        public static (string, string) UpdateContractForRecord(Record record, string ContractFormFile, string OutPutFileDirectory, string fileName)
        {
            var datetime = DateTime.Now.ToString().Replace("/", "_").Replace(":", "_");

            string fileFullName = Path.Combine(OutPutFileDirectory, fileName + ".xlsx");
            string fileNameWithExt = fileName + ".xlsx";

            if (File.Exists(fileFullName))
            {
                fileFullName = Path.Combine(OutPutFileDirectory, fileName + "_" + datetime + ".xlsx");
                fileNameWithExt = fileName + "_" + datetime + ".xlsx";
            }

            File.Copy(ContractFormFile, fileFullName);

            using (var workbook = new XLWorkbook(fileFullName))
            {
                var worksheet = workbook.Worksheets.FirstOrDefault();
                if (record.Customer != null)
                {
                    if (workbook.NamedRanges.Contains("CustomerFullName") )
                        workbook.Cell("CustomerFullName").Value = record.Customer.CustomerFullName;
                    if (workbook.NamedRanges.Contains("CustomerPhoneNumber") )
                        workbook.Cell("CustomerPhoneNumber").Value = record.Customer.CustomerPhoneNumber;
                    if (workbook.NamedRanges.Contains("CustomerDocumentType") )
                        workbook.Cell("CustomerDocumentType").Value = record.Customer.CustomerDocumentType;
                    if (workbook.NamedRanges.Contains("CustomerDocumentSeries") )
                        workbook.Cell("CustomerDocumentSeries").Value = record.Customer.CustomerDocumentSeries;
                    if (workbook.NamedRanges.Contains("CustomerDocumentNumber") )
                        workbook.Cell("CustomerDocumentNumber").Value = record.Customer.CustomerDocumentNumber;
                }
                if (record.End != null)
                {
                    if (workbook.NamedRanges.Contains("EndHours"))
                        workbook.Cell("EndHours").Value = record.End.GetValueOrDefault().Hour;
                    if (workbook.NamedRanges.Contains("EndMinutes"))
                        workbook.Cell("EndMinutes").Value = record.End.GetValueOrDefault().Minute;
                    if (workbook.NamedRanges.Contains("EndDay"))
                        workbook.Cell("EndDay").Value = record.End.GetValueOrDefault().Day;
                    if (workbook.NamedRanges.Contains("EndMonth"))
                        workbook.Cell("EndMonth").Value = record.End.GetValueOrDefault().Month;
                    if (workbook.NamedRanges.Contains("EndYear"))
                        workbook.Cell("EndYear").Value = record.End.GetValueOrDefault().Year;
                }
                if (workbook.NamedRanges.Contains("Price"))
                    workbook.Cell("Price").Value = record.Price;
                if (record.Start != null)
                {
                    if (workbook.NamedRanges.Contains("DateDay"))
                        workbook.Cell("DateDay").Value = record.Start.GetValueOrDefault().Day;
                    if (workbook.NamedRanges.Contains("DateMonth"))
                        workbook.Cell("DateMonth").Value = record.Start.GetValueOrDefault().Month;
                    if (workbook.NamedRanges.Contains("DateYear"))
                        workbook.Cell("DateYear").Value = record.Start.GetValueOrDefault().Year;
                }
                if (record.ItemRecords != null) {
                    if (workbook.NamedRanges.Contains("RentalItems"))
                    {
                        IXLCell cell = workbook.Cell("RentalItems");
                        int i = 0;
                        foreach (var icat in record.ItemRecords.DistinctBy(irec => irec.Item.ItemType.ItemCategory).Select(irec => irec.Item.ItemType.ItemCategory) )
                        {
                            i++;
                            if (icat==null || !icat.Accessories)
                            {
                                var count = record.ItemRecords.Count(irec => irec.Item.ItemType.ItemCategoryID == icat?.ItemCategoryID);
                                var numbers = string.Join(", ", record.ItemRecords.Where(irec => irec.Item.ItemType.ItemCategoryID == icat?.ItemCategoryID).Select(irec => irec.Item.ItemNumber).ToList());
                                cell.Value = "\t" + i + ". " + icat?.ItemCategoryName + " в количестве " + count + " шт., номера: " + numbers;
                            }
                            else 
                            {
                                cell.Value = "\t" + i +". " + icat?.ItemCategoryName + ": ";
                                foreach (var itype in record.ItemRecords.Where(irec => irec.Item.ItemType.ItemCategoryID == icat?.ItemCategoryID).DistinctBy(irec => irec.Item.ItemType).Select(irec => irec.Item.ItemType)) { 
                                    var count = record.ItemRecords.Count(irec => irec.Item.ItemTypeID == itype?.ItemTypeID);
                                    cell.Value = cell.Value + itype?.ItemTypeName +" (" + count + "шт.) ";
                                }
                            }
                            cell = cell.WorksheetRow().InsertRowsBelow(1).Cells().First();
                        }
                    }
                }


                workbook.Save();
            }

            return (fileFullName, fileNameWithExt);
        }

        public static (string, string) CreateExcelFile<T>(IEnumerable<T> data, string OutPutFileDirectory, string fileName)
        {
            var datetime = DateTime.Now.ToString().Replace("/", "_").Replace(":", "_");

            string fileFullName = Path.Combine(OutPutFileDirectory, fileName + ".xlsx");
            string fileNameWithExt = fileName + ".xlsx";

            if (File.Exists(fileFullName))
            {
                fileFullName = Path.Combine(OutPutFileDirectory, fileName + "_" + datetime + ".xlsx");
                fileNameWithExt = fileName + "_" + datetime + ".xlsx";
            }

            using (SpreadsheetDocument package = SpreadsheetDocument.Create(fileFullName, SpreadsheetDocumentType.Workbook))
            {
                CreatePartsForExcel<T>(package, data);
            }

            return (fileFullName, fileNameWithExt);
        }

        private static void CreatePartsForExcel<T>(SpreadsheetDocument document, IEnumerable<T> data)
        {
            SheetData partSheetData = GenerateSheetdataForDetails<T>(data);

            WorkbookPart workbookPart1 = document.AddWorkbookPart();
            GenerateWorkbookPartContent(workbookPart1);

            WorkbookStylesPart workbookStylesPart1 = workbookPart1.AddNewPart<WorkbookStylesPart>("rId3");
            GenerateWorkbookStylesPartContent(workbookStylesPart1);

            WorksheetPart worksheetPart1 = workbookPart1.AddNewPart<WorksheetPart>("rId1");
            GenerateWorksheetPartContent(worksheetPart1, partSheetData);
        }

        private static void GenerateWorkbookPartContent(WorkbookPart workbookPart1)
        {
            Workbook workbook1 = new Workbook();
            Sheets sheets1 = new Sheets();
            Sheet sheet1 = new Sheet() { Name = "Sheet1", SheetId = (UInt32Value)1U, Id = "rId1" };
            sheets1.Append(sheet1);
            workbook1.Append(sheets1);
            workbookPart1.Workbook = workbook1;
        }

        private static SheetData GenerateSheetdataForDetails<T>(IEnumerable<T> data)
        {
            SheetData sheetData1 = new SheetData();
            sheetData1.Append(CreateHeaderRowForExcel<T>());

            foreach (T model in data)
            {
                Row partsRows = GenerateRowForChildPartDetail<T>(model);
                sheetData1.Append(partsRows);
            }
            return sheetData1;
        }

        private static Row CreateHeaderRowForExcel<T>()
        {
            Row tRow = new Row();
            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                tRow.Append(CreateCell( prop.GetAttribute<DisplayAttribute>(false).Name, 2U));
            }
            return tRow;
        }

        private static Row GenerateRowForChildPartDetail<T>(T model)
        {
            Row tRow = new Row();
            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                tRow.Append(CreateCell( prop.GetValue(model)?.ToString() ));
            }
            return tRow;
        }

        private static Cell CreateCell(string? text)
        {
            Cell cell = new Cell();
            cell.StyleIndex = 1U;
            cell.DataType = ResolveCellDataTypeOnValue(text);
            cell.CellValue = new CellValue(text??"");
            return cell;
        }

        private static Cell CreateCell(string text, uint styleIndex)
        {
            Cell cell = new Cell();
            cell.StyleIndex = styleIndex;
            cell.DataType = ResolveCellDataTypeOnValue(text);
            cell.CellValue = new CellValue(text??"");
            return cell;
        }

        private static EnumValue<CellValues> ResolveCellDataTypeOnValue(string? text)
        {
            int intVal;
            double doubleVal;
            if (int.TryParse(text, out intVal) || double.TryParse(text, out doubleVal))
            {
                return CellValues.Number;
            }
            else
            {
                return CellValues.String;
            }
        }

        private static void GenerateWorksheetPartContent(WorksheetPart worksheetPart1, SheetData sheetData1)
        {
            Worksheet worksheet1 = new Worksheet() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "x14ac" } };
            worksheet1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            worksheet1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            worksheet1.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");
            SheetDimension sheetDimension1 = new SheetDimension() { Reference = "A1" };

            SheetViews sheetViews1 = new SheetViews();

            SheetView sheetView1 = new SheetView() { TabSelected = true, WorkbookViewId = (UInt32Value)0U };
            Selection selection1 = new Selection() { ActiveCell = "A1", SequenceOfReferences = new ListValue<StringValue>() { InnerText = "A1" } };

            sheetView1.Append(selection1);

            sheetViews1.Append(sheetView1);
            SheetFormatProperties sheetFormatProperties1 = new SheetFormatProperties() { DefaultRowHeight = 15D, DyDescent = 0.25D };

            PageMargins pageMargins1 = new PageMargins() { Left = 0.7D, Right = 0.7D, Top = 0.75D, Bottom = 0.75D, Header = 0.3D, Footer = 0.3D };
            worksheet1.Append(sheetDimension1);
            worksheet1.Append(sheetViews1);
            worksheet1.Append(sheetFormatProperties1);
            worksheet1.Append(sheetData1);
            worksheet1.Append(pageMargins1);
            worksheetPart1.Worksheet = worksheet1;
        }

        private static void GenerateWorkbookStylesPartContent(WorkbookStylesPart workbookStylesPart1)
        {
            Stylesheet stylesheet1 = new Stylesheet() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "x14ac" } };
            stylesheet1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            stylesheet1.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");

            Fonts fonts1 = new Fonts() { Count = (UInt32Value)2U, KnownFonts = true };

            Font font1 = new Font();
            FontSize fontSize1 = new FontSize() { Val = 11D };
            Color color1 = new Color() { Theme = (UInt32Value)1U };
            FontName fontName1 = new FontName() { Val = "Calibri" };
            FontFamilyNumbering fontFamilyNumbering1 = new FontFamilyNumbering() { Val = 2 };
            FontScheme fontScheme1 = new FontScheme() { Val = FontSchemeValues.Minor };

            font1.Append(fontSize1);
            font1.Append(color1);
            font1.Append(fontName1);
            font1.Append(fontFamilyNumbering1);
            font1.Append(fontScheme1);

            Font font2 = new Font();
            Bold bold1 = new Bold();
            FontSize fontSize2 = new FontSize() { Val = 11D };
            Color color2 = new Color() { Theme = (UInt32Value)1U };
            FontName fontName2 = new FontName() { Val = "Calibri" };
            FontFamilyNumbering fontFamilyNumbering2 = new FontFamilyNumbering() { Val = 2 };
            FontScheme fontScheme2 = new FontScheme() { Val = FontSchemeValues.Minor };

            font2.Append(bold1);
            font2.Append(fontSize2);
            font2.Append(color2);
            font2.Append(fontName2);
            font2.Append(fontFamilyNumbering2);
            font2.Append(fontScheme2);

            fonts1.Append(font1);
            fonts1.Append(font2);

            Fills fills1 = new Fills() { Count = (UInt32Value)2U };

            Fill fill1 = new Fill();
            PatternFill patternFill1 = new PatternFill() { PatternType = PatternValues.None };

            fill1.Append(patternFill1);

            Fill fill2 = new Fill();
            PatternFill patternFill2 = new PatternFill() { PatternType = PatternValues.Gray125 };

            fill2.Append(patternFill2);

            fills1.Append(fill1);
            fills1.Append(fill2);

            Borders borders1 = new Borders() { Count = (UInt32Value)2U };

            Border border1 = new Border();
            LeftBorder leftBorder1 = new LeftBorder();
            RightBorder rightBorder1 = new RightBorder();
            TopBorder topBorder1 = new TopBorder();
            BottomBorder bottomBorder1 = new BottomBorder();
            DiagonalBorder diagonalBorder1 = new DiagonalBorder();

            border1.Append(leftBorder1);
            border1.Append(rightBorder1);
            border1.Append(topBorder1);
            border1.Append(bottomBorder1);
            border1.Append(diagonalBorder1);

            Border border2 = new Border();

            LeftBorder leftBorder2 = new LeftBorder() { Style = BorderStyleValues.Thin };
            Color color3 = new Color() { Indexed = (UInt32Value)64U };

            leftBorder2.Append(color3);

            RightBorder rightBorder2 = new RightBorder() { Style = BorderStyleValues.Thin };
            Color color4 = new Color() { Indexed = (UInt32Value)64U };

            rightBorder2.Append(color4);

            TopBorder topBorder2 = new TopBorder() { Style = BorderStyleValues.Thin };
            Color color5 = new Color() { Indexed = (UInt32Value)64U };

            topBorder2.Append(color5);

            BottomBorder bottomBorder2 = new BottomBorder() { Style = BorderStyleValues.Thin };
            Color color6 = new Color() { Indexed = (UInt32Value)64U };

            bottomBorder2.Append(color6);
            DiagonalBorder diagonalBorder2 = new DiagonalBorder();

            border2.Append(leftBorder2);
            border2.Append(rightBorder2);
            border2.Append(topBorder2);
            border2.Append(bottomBorder2);
            border2.Append(diagonalBorder2);

            borders1.Append(border1);
            borders1.Append(border2);

            CellStyleFormats cellStyleFormats1 = new CellStyleFormats() { Count = (UInt32Value)1U };
            CellFormat cellFormat1 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U };

            cellStyleFormats1.Append(cellFormat1);

            CellFormats cellFormats1 = new CellFormats() { Count = (UInt32Value)3U };
            CellFormat cellFormat2 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U };
            CellFormat cellFormat3 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)1U, FormatId = (UInt32Value)0U, ApplyBorder = true };
            CellFormat cellFormat4 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)1U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)1U, FormatId = (UInt32Value)0U, ApplyFont = true, ApplyBorder = true };

            cellFormats1.Append(cellFormat2);
            cellFormats1.Append(cellFormat3);
            cellFormats1.Append(cellFormat4);

            CellStyles cellStyles1 = new CellStyles() { Count = (UInt32Value)1U };
            CellStyle cellStyle1 = new CellStyle() { Name = "Normal", FormatId = (UInt32Value)0U, BuiltinId = (UInt32Value)0U };

            cellStyles1.Append(cellStyle1);
            DifferentialFormats differentialFormats1 = new DifferentialFormats() { Count = (UInt32Value)0U };
            TableStyles tableStyles1 = new TableStyles() { Count = (UInt32Value)0U, DefaultTableStyle = "TableStyleMedium2", DefaultPivotStyle = "PivotStyleLight16" };

            StylesheetExtensionList stylesheetExtensionList1 = new StylesheetExtensionList();

            StylesheetExtension stylesheetExtension1 = new StylesheetExtension() { Uri = "{EB79DEF2-80B8-43e5-95BD-54CBDDF9020C}" };
            stylesheetExtension1.AddNamespaceDeclaration("x14", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
            X14.SlicerStyles slicerStyles1 = new X14.SlicerStyles() { DefaultSlicerStyle = "SlicerStyleLight1" };

            stylesheetExtension1.Append(slicerStyles1);

            StylesheetExtension stylesheetExtension2 = new StylesheetExtension() { Uri = "{9260A510-F301-46a8-8635-F512D64BE5F5}" };
            stylesheetExtension2.AddNamespaceDeclaration("x15", "http://schemas.microsoft.com/office/spreadsheetml/2010/11/main");
            X15.TimelineStyles timelineStyles1 = new X15.TimelineStyles() { DefaultTimelineStyle = "TimeSlicerStyleLight1" };

            stylesheetExtension2.Append(timelineStyles1);

            stylesheetExtensionList1.Append(stylesheetExtension1);
            stylesheetExtensionList1.Append(stylesheetExtension2);

            stylesheet1.Append(fonts1);
            stylesheet1.Append(fills1);
            stylesheet1.Append(borders1);
            stylesheet1.Append(cellStyleFormats1);
            stylesheet1.Append(cellFormats1);
            stylesheet1.Append(cellStyles1);
            stylesheet1.Append(differentialFormats1);
            stylesheet1.Append(tableStyles1);
            stylesheet1.Append(stylesheetExtensionList1);

            workbookStylesPart1.Stylesheet = stylesheet1;
        }



    }
}
