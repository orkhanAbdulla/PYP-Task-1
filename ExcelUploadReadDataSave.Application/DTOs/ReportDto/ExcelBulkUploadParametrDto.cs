using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploadReadDataSave.Application.DTOs.ReportDto
{
    public class ExcelBulkUploadParametrDto
    {
        public string Segment { get; set; }
        public string Country { get; set; }
        public string Product { get; set; }
        public string DiscountBand { get; set; }
        public double UnitsSold { get; set; }
        public int ManufacturingPrice { get; set; }
        public int SalePrice { get; set; }
        public long GrossSales { get; set; }
        public int Discounts { get; set; }
        public double Sales { get; set; }
        public int COGS { get; set; }
        public double Profit { get; set; }
        public DateTime Data { get; set; }
    }
}
