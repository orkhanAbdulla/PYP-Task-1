using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploadReadDataSave.Application.CustomExceptions
{
    public class DateIntervalException:Exception
    {
        public DateIntervalException(string msg):base(msg)
        {

        }
    }
}
