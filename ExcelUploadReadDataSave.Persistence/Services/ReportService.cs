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

namespace ExcelUploadReadDataSave.Persistence.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IFileManager _fileManager;
        private readonly IMapper _mapper;

        public ReportService(IReportRepository reportRepository, IFileManager fileManager,IMapper mapper)
        {
            _reportRepository = reportRepository;
            _fileManager = fileManager;
            _mapper = mapper;
        }

        public async Task UploadDataFile(ReportUploadFileDto FileDto)
        {
            if (!_fileManager.IsValidType(FileDto,".xlxs"))
                   throw new FileFormatException("File extention must be .xlsx or .xls");
            if (!_fileManager.IsValidSize(FileDto, 5120))
                   throw new FileFormatException("File must be less than 5 kb");

            List<Report> reports = _fileManager.ExcelDataReader(_fileManager.Save(FileDto));
                await _reportRepository.AddRangeAsync(reports);
                await _reportRepository.SaveAsync();
        }
        public async Task GetReport(SendReportDto sendReportDto,int type)
        {
            int result = DateTime.Compare(sendReportDto.StartDate, sendReportDto.EndDate);
            if (result>0 ||result==0)
                throw new DateIntervalException("Start Date must be more End date");
            if (!Enum.IsDefined((ReportType)type))
               throw new NotReportTypeException("Please enter correct report type!");
            ReportType reportType = (ReportType)type;

            switch (reportType)
            {
                case ReportType.SegmentSales:

                    IQueryable<Report> SegmentDb = _reportRepository.GetWhere(x =>x.Data >= sendReportDto.StartDate && x.Data <= sendReportDto.EndDate);
                    List<SegmentGetDto> segmentGetDtos = _mapper.Map<List<SegmentGetDto>>(SegmentDb);

                    Dictionary<string, SegmentGetDto> segmentTotal = new();

                    foreach (SegmentGetDto segmentGetDto in segmentGetDtos)
                    {
                        if (!segmentTotal.ContainsKey(segmentGetDto.Segment))
                        {
                            segmentTotal.Add(segmentGetDto.Segment, segmentGetDto);
                            continue;
                        }
                        segmentTotal[segmentGetDto.Segment].UnitsSold += segmentGetDto.UnitsSold;
                        segmentTotal[segmentGetDto.Segment].GrossSales += segmentGetDto.GrossSales;
                        segmentTotal[segmentGetDto.Segment].Discounts += segmentGetDto.Discounts;
                        segmentTotal[segmentGetDto.Segment].Profit += segmentGetDto.Profit;
                    }
                    Console.WriteLine(segmentTotal);

                    break;
                case ReportType.CountrySales:
                    break;
                case ReportType.ProductSales:
                    break;
                case ReportType.ProductDiscount:
                    break;
            }
        
        }


    }
}
