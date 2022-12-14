using ExcelUploadReadDataSave.Application.DTOs.ReportDto;
using ExcelUploadReadDataSave.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ExcelUploadReadDataSave.Application.CustomExceptions;
using static ExcelUploadReadDataSave.Application.Utilis.Enums;

namespace ExcelUploadReadDataSave.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IMailService _mailService;
        private readonly IFileManager _fileManager;

        public ReportsController(IReportService reportService, IMailService mailService, IFileManager fileManager)
        {
            _reportService = reportService;
            _mailService = mailService;
            _fileManager = fileManager;
         
        }
        [HttpPost("")]
        public async Task<IActionResult> UploadData([FromForm]ReportUploadFileDto reportUploadFileDto)
        {
           await _reportService.UploadDataFile(reportUploadFileDto);
           return Ok();
        }
        [HttpGet("")]
        public async Task<IActionResult> SendReport([FromQuery] SendReportDto sendReportDto)
        {
            ReportResultDtos reportResultDto=await _reportService.GetReport(sendReportDto);

            string path=_fileManager.ExcelCreator(reportResultDto);

            ReportAtchementlDto reportAtchementlDto = new() { Atchement= path, emailAddresses= sendReportDto.emailAddresses};
            await _mailService.SendFileAtchemenEmail(reportAtchementlDto);

            _fileManager.DeleteFile(path);

            return Ok();
        }
    

    }
}
