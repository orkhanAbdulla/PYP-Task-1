using AutoMapper;
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
using static ExcelUploadReadDataSave.Application.Utilis.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ExcelUploadReadDataSave.Persistence.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IFileManager _fileManager;
        private readonly IMapper _mapper;


        public ReportService(IReportRepository reportRepository, IFileManager fileManager, IMapper mapper)
        {
            _reportRepository = reportRepository;
            _fileManager = fileManager;
            _mapper = mapper;
        }

        public async Task UploadDataFile(ReportUploadFileDto FileDto)
        {
            if (!_fileManager.IsValidType(FileDto, ".xlxs"))
                throw new System.IO.FileFormatException("File extention must be .xlsx or .xls");
            if (!_fileManager.IsValidSize(FileDto, 5120))
                throw new System.IO.FileFormatException("File must be less than 5 kb");

            List<Report> reports = _fileManager.ExcelDataReader(_fileManager.Save(FileDto));
            await _reportRepository.AddRangeAsync(reports);
            await _reportRepository.SaveAsync();
        }
        public async Task<ReportResultDtos> GetReport(SendReportDto sendReportDto, int type)
        {
            int result = DateTime.Compare(sendReportDto.StartDate, sendReportDto.EndDate);
            if (result > 0 || result == 0)
                throw new DateIntervalException("Start Date must be more End date");
            if (!Enum.IsDefined((ReportType)type))
                throw new NotReportTypeException("Please enter correct report type!");
            ReportType reportType = (ReportType)type;

            ReportResultDtos reportResultDto = new ReportResultDtos();


            IQueryable<Report> SegmentDb = _reportRepository.GetWhere(x => x.Data >= sendReportDto.StartDate && x.Data <= sendReportDto.EndDate);


            switch (reportType)
            {
                case ReportType.SegmentSales:

                    reportResultDto.reportGetDtos = SegmentDb.GroupBy(x => x.Segment)
                          .Select(g => new ReportGetDto
                          {
                              Type = g.Key,
                              UnitsSold = g.Sum(x => x.Discounts),
                              GrossSales = g.Sum(x => x.GrossSales),
                              Discounts = g.Sum(x => x.Discounts),
                              Profit = g.Sum(x => x.Profit),
                          }).ToList();
                    break;
                case ReportType.CountrySales:

                    reportResultDto.reportGetDtos = SegmentDb.GroupBy(x => x.Country)
                         .Select(g => new ReportGetDto
                         {
                             Type = g.Key,
                             UnitsSold = g.Sum(x => x.Discounts),
                             GrossSales = g.Sum(x => x.GrossSales),
                             Discounts = g.Sum(x => x.Discounts),
                             Profit = g.Sum(x => x.Profit),
                         }).ToList();
                    break;
                case ReportType.ProductSales:

                    reportResultDto.reportGetDtos = SegmentDb.GroupBy(x => x.Product)
                .Select(g => new ReportGetDto
                {
                    Type = g.Key,
                    UnitsSold = g.Sum(x => x.Discounts),
                    GrossSales = g.Sum(x => x.GrossSales),
                    Discounts = g.Sum(x => x.Discounts),
                    Profit = g.Sum(x => x.Profit),
                }).ToList();
                    break;
                case ReportType.ProductDiscount:
                    reportResultDto.reportProductDiscountDtos = SegmentDb.GroupBy(x => x.Product)
                .Select(g => new ReportProductDiscountDto
                {
                    Type = g.Key,
                    Percent = g.Sum(x => x.Discounts) / g.Sum(x => x.GrossSales) * 100
                }).ToList();
                    break;
            }
            return reportResultDto;
        }
    }
}
