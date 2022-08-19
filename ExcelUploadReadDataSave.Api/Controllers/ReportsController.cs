using ExcelUploadReadDataSave.Application.DTOs.ReportDto;
using ExcelUploadReadDataSave.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExcelUploadReadDataSave.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }
        [HttpPost("")]
        public async Task<IActionResult> UploadData([FromForm]ReportUploadFileDto reportUploadFileDto)
        {
           await _reportService.UploadDataFile(reportUploadFileDto);
           return Ok();
        }
    }
}
