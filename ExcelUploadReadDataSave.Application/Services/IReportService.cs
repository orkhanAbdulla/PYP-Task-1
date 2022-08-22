using ExcelUploadReadDataSave.Application.DTOs.ReportDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ExcelUploadReadDataSave.Application.Utilis.Enums;

namespace ExcelUploadReadDataSave.Application.Services
{
    public interface IReportService
    {
      Task UploadDataFile(ReportUploadFileDto reportUploadFileDto);
      Task<ReportResultDtos> GetReport(SendReportDto sendReportDto);
    }
}
