using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploadReadDataSave.Application.DTOs.ReportDto
{
    public class ReportAtchementlDto
    {
        public string Atchement{ get; set; }
        public string[] emailAddresses { get; set; }
    }
}
