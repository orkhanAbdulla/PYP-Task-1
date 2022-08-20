using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploadReadDataSave.Application.DTOs.ReportDto
{
    public class ReportResultDtos
    {
        public List<ReportGetDto> reportGetDtos { get; set; }
        public List<ReportProductDiscountDto> reportProductDiscountDtos { get; set; }
        public ReportResultDtos()
        {
            reportGetDtos = new List<ReportGetDto>();
            reportProductDiscountDtos = new List<ReportProductDiscountDto>();
        }
    }
}
