using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploadReadDataSave.Application.DTOs.ReportDto
{
    public class ReportGetDto
    {
        public string Type { get; set; }
        public double UnitsSold { get; set; }
        public long GrossSales { get; set; }
        public double Discounts { get; set; }
        public double Profit { get; set; }

    }
}
