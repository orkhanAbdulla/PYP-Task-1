﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploadReadDataSave.Application.DTOs.ReportDto
{
    public class ReportUploadFileDto
    {
        public IFormFile File { get; set; }
    }
}
