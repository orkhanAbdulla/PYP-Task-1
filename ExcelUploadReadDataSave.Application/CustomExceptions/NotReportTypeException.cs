using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploadReadDataSave.Application.CustomExceptions
{
    public class NotReportTypeException:Exception
    {
        public NotReportTypeException(string msg):base(msg)
        {

        }
    }
}
