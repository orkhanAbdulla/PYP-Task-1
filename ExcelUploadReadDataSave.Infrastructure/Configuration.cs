using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploadReadDataSave.Infrastructure
{
    public static class Configuration
    {
        public static string SendGridEmailSetting(string key){
            ConfigurationManager configuration = new();
            configuration.SetBasePath(Directory.GetCurrentDirectory());
            configuration.AddJsonFile("appsettings.json");
            
            return configuration.GetSection(key).Value;
        }
        
    }
}
