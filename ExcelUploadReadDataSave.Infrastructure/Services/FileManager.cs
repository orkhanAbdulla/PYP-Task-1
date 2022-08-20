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

namespace ExcelUploadReadDataSave.Infrastructure.Services
{
    public class FileManager:IFileManager
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

        public string EmailHtml(ReportAtchementlDto reportEmailDto)
        {
            return @"<html><head>
		<meta charset='utf-8' />
		<title></title>
		<style>
		@mixin media() {
		@media (min-width: 768px) {
			@content;
		}
	   }
 
	body, html {
	font-family: 'Vollkorn', serif;
	font-weight: 400;
	line-height: 1.3;
	font-size: 16px;
	}
 
	.siteTitle {
	display: block;
	font-weight: 900;
	font-size: 30px;
	margin: 20px 0;
	
	@include media {
		font-size: 60px;
	}
	}
 
	header,
	main,
	footer {
	max-width: 960px;
	margin: 0 auto;
	}
 
	.card {
	height: 400px;
	position: relative;
	padding: 20px;
	box-sizing: border-box;
	display: flex;
	align-items: flex-end;
	text-decoration: none;
	border: 4px solid #b0215e;
	margin-bottom: 20px;
	background-image: url('https://baylorlariat.com/wp-content/uploads/2018/02/Iron-Man-Movie_Poster_2008.jpg');
		background-size: cover;
	
	@include media {
		height: 500px;
	}
	}
 
	.inner {
	height: 50%;
	display: flex;
	flex-direction: column;
	justify-content: center;
	align-items: center; 
	background: white;
	box-sizing: border-box;
	padding: 40px;
	
	@include media {
		width: 50%;
		height: 100%;
	}
	}
 
	.title {
	font-size: 24px;
	color: black;  
	text-align: center;
	font-weight: 700;
	color: #181818;
	text-shadow: 0px 2px 2px #a6f8d5;
	position: relative;
	margin: 0 0 20px 0;
	
	@include media {
		font-size: 30px;
	}
	}
	</style>
</head>
<body>
<div  class='card'>
  <div class='inner'>
	<h2 class='title'>{{name}}</h2>
	<time class='subtitle'>Supper<time>
  </div>
</div>
</body>
</html>".Replace("{{name}}", reportEmailDto.toEmail);
        }
    }
}
