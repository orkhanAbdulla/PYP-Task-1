using ExcelDataReader;
using ExcelUploadReadDataSave.Application.DTOs.ReportDto;
using ExcelUploadReadDataSave.Application.Services;
using ExcelUploadReadDataSave.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Syncfusion.XlsIO;
using ClosedXML.Excel;

namespace ExcelUploadReadDataSave.Infrastructure.Services
{
    public class FileManager : IFileManager
    {
        public bool IsValidType(ReportUploadFileDto file, string type)
        {
            string xlxs = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string xls = "application/vnd.ms-excel";
            return file.File.ContentType == xlxs || file.File.ContentType == xls;
        }
        public bool IsValidSize(ReportUploadFileDto file, int kb)
        {
            return file.File.Length / 1024 <= kb;
        }
        public string Save(ReportUploadFileDto file)
        {
            string NewfileName = Guid.NewGuid().ToString() + file.File.FileName;
            string path = Directory.GetCurrentDirectory() + "/wwwroot/" + NewfileName;
            using (FileStream stream = new FileStream(path, FileMode.CreateNew))
            {
                file.File.CopyToAsync(stream);
            }
            return path;
        }

        public List<Report> ExcelDataReader(string Path)
        {
            List<ExcelBulkUploadParametrDto> Parametrs = new();
            FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            IExcelDataReader excelDataReader = ExcelReaderFactory.CreateReader(stream);
            DataSet dataSet = excelDataReader.AsDataSet(
                new ExcelDataSetConfiguration()
                {
                    UseColumnDataType = false,
                    ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                }
                );
            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                ExcelBulkUploadParametrDto rows = new();
                rows.Segment = dataSet.Tables[0].Rows[i].ItemArray[0] != null ? Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[0]) : "-1";
                rows.Country = dataSet.Tables[0].Rows[i].ItemArray[1] != null ? Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[1]) : "-1";
                rows.Product = dataSet.Tables[0].Rows[i].ItemArray[2] != null ? Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[2]) : "-1";
                rows.DiscountBand = dataSet.Tables[0].Rows[i].ItemArray[3] != null ? Convert.ToString(dataSet.Tables[0].Rows[i].ItemArray[3]) : "-1";
                rows.UnitsSold = dataSet.Tables[0].Rows[i].ItemArray[4] != null ? Convert.ToDouble(dataSet.Tables[0].Rows[i].ItemArray[4]) : 0;
                rows.ManufacturingPrice = dataSet.Tables[0].Rows[i].ItemArray[5] != null ? Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[5]) : 0;
                rows.SalePrice = dataSet.Tables[0].Rows[i].ItemArray[6] != null ? Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[6]) : 0;
                rows.GrossSales = dataSet.Tables[0].Rows[i].ItemArray[7] != null ? Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[7]) : 0;
                rows.Discounts = dataSet.Tables[0].Rows[i].ItemArray[8] != null ? Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[8]) : 0;
                rows.Sales = dataSet.Tables[0].Rows[i].ItemArray[9] != null ? Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[9]) : 0;
                rows.COGS = dataSet.Tables[0].Rows[i].ItemArray[10] != null ? Convert.ToInt32(dataSet.Tables[0].Rows[i].ItemArray[10]) : 0;
                rows.Profit = dataSet.Tables[0].Rows[i].ItemArray[11] != null ? Convert.ToDouble(dataSet.Tables[0].Rows[i].ItemArray[11]) : 0;
                rows.Data = dataSet.Tables[0].Rows[i].ItemArray[12] != null ? Convert.ToDateTime(dataSet.Tables[0].Rows[i].ItemArray[12]) : DateTime.MinValue;
                Parametrs.Add(rows);
            }
            stream.Close();
            DeleteFile(Path);

            List<Report> reports = new();

            if (Parametrs.Count > 0)
            {
                foreach (var rows in Parametrs)
                {
                    Report report = new Report()
                    {
                        Segment = rows.Segment,
                        Country = rows.Country,
                        Product = rows.Product,
                        DiscountBand = rows.DiscountBand,
                        UnitsSold = rows.UnitsSold,
                        ManufacturingPrice = rows.ManufacturingPrice,
                        SalePrice = rows.SalePrice,
                        GrossSales = rows.GrossSales,
                        Discounts = rows.Discounts,
                        Sales = rows.Sales,
                        COGS = rows.COGS,
                        Profit = rows.Profit,
                        Data = rows.Data,
                        CreateAt = DateTime.UtcNow
                    };
                    reports.Add(report);
                }
            }
            return reports;
        }

        public void DeleteFile(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

        }

        public string TemplateHtml(List<ReportGetDto> ReportFilterData)
        {
            var sb = new StringBuilder();
            sb.Append(@"
                   <html>
                        <head></head>
                        <body>
                            <div class='header'<h1>This is the generated Pdf report!!!</h1></div>
                            <table align='center'>
                                <tr>
                                    <th>Segment</th>
                                    <th>UnitsSold</th>
                                    <th>GrossSales</th>
                                    <th>Discounts</th>
                                    <th>Profit</th>
                                </tr>");
            foreach (var data in ReportFilterData)
            {
                sb.AppendFormat(@"
                                 <tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                    <td>{4}</th>
                                  </tr>", data.Type, data.UnitsSold, data.GrossSales, data.Discounts, data.Profit);
            }
            sb.Append(@"
                    </table>
                    </body>
                    </html>
                ");

            return sb.ToString();

        }

        public string ExcelCreator(ReportResultDtos ReportFilterData)
        {
            DataTable dt = new DataTable("Grid");
         
            if (ReportFilterData.reportProductDiscountDtos.Count>0)
            {
                dt.Columns.AddRange(new DataColumn[2] { new DataColumn("Type"),
                                     new DataColumn("UnitsSold")});
                foreach (var report in ReportFilterData.reportProductDiscountDtos)
                {
                    dt.Rows.Add(report.Type, report.Percent);
                }
            }
            else if(ReportFilterData.reportGetDtos.Count > 0)
            {
                dt.Columns.AddRange(new DataColumn[5] { new DataColumn("Product"),
                                     new DataColumn("UnitsSold"), new DataColumn("Discount"), new DataColumn("Discounts"), new DataColumn("Profit") });
                foreach (var report in ReportFilterData.reportGetDtos)
                {
                    dt.Rows.Add(report.Type, report.UnitsSold,report.GrossSales,report.Discounts,report.Profit);
                }
            }
            string fileName =Guid.NewGuid().ToString()+"Grid.xlsx";
            string path = Directory.GetCurrentDirectory() + "/wwwroot/" + fileName;
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);

                using (FileStream stream = new FileStream(path, FileMode.CreateNew))
                {
                    wb.SaveAs(stream);
                }
                //using (MemoryStream stream = new MemoryStream())
                //{
                //    wb.SaveAs(stream);
                //    stream.ToArray();
                //    File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                //}
            }
            return path;
        }
    }   
}
