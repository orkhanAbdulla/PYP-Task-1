using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploadReadDataSave.Persistence
{
    public static class Configuration
    {
        public static string ConnectionString
        {
            get
            {
                ConfigurationManager configuration = new();
                configuration.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/E-CommerceApi.Api"));
                configuration.AddJsonFile("appsettings.json");
                return configuration.GetConnectionString("PostgreSQL");
            }
        }
    }
}
