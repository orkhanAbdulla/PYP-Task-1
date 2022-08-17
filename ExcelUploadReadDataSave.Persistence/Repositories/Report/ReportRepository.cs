using ExcelUploadReadDataSave.Application.Repositories;
using ExcelUploadReadDataSave.Domain.Entities;
using ExcelUploadReadDataSave.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploadReadDataSave.Persistence.Repositories
{
    public class ReportRepository:Repository<Report>,IReportRepository
    {
        public ReportRepository(AppDbContext context):base(context)
        {

        }
    }
}
