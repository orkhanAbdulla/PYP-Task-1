using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploadReadDataSave.Application.CustomExceptions
{
    public class FileFormatException:Exception
    {
        public FileFormatException(string msg):base(msg)
        {

        }
    }
}
