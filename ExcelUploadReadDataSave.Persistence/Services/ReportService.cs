using ExcelDataReader;
using ExcelUploadReadDataSave.Application.CustomExceptions;
using ExcelUploadReadDataSave.Application.DTOs.ReportDto;
using ExcelUploadReadDataSave.Application.Repositories;
using ExcelUploadReadDataSave.Application.Services;
using ExcelUploadReadDataSave.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploadReadDataSave.Persistence.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IFileManager _fileManager;

        public ReportService(IReportRepository reportRepository, IFileManager fileManager)
        {
            _reportRepository = reportRepository;
            _fileManager = fileManager;
        }

        public async Task UploadDataFile(ReportUploadFileDto FileDto)
        {
            if (_fileManager.IsValidType(FileDto,".xlxs"))
                   throw new FileFormatException("File extention must be .xlsx or .xls");
            if (!_fileManager.IsValidSize(FileDto, 5120))
                   throw new FileFormatException("File must be less than 5 kb");

            List<Report> reports = _fileManager.ExcelDataReader(_fileManager.Save(FileDto));

                await _reportRepository.AddRangeAsync(reports);
                await _reportRepository.SaveAsync();
        }
    }
}
