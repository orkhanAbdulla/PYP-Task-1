using ExcelUploadReadDataSave.Application.DTOs.ReportDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploadReadDataSave.Application.Services
{
    public interface IMailService
    {
        Task<string> SendFileAtchemenEmail(ReportAtchementlDto reportEmailDto);
 
    }
}
