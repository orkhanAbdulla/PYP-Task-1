using ExcelUploadReadDataSave.Application.DTOs.ReportDto;
using ExcelUploadReadDataSave.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploadReadDataSave.Application.Services
{
    public interface IFileManager
    {
        string Save(ReportUploadFileDto file);
        void DeleteFile(string path);
        bool IsValidType(ReportUploadFileDto file, string type);
        bool IsValidSize(ReportUploadFileDto file, int size);
        List<Report> ExcelDataReader(string Path);
        string TemplateHtml(List<ReportGetDto> ReportFilterData);
        string ExcelCreator(ReportResultDtos ReportFilterData);
    }
}
