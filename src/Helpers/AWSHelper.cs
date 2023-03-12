using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DiscordBot.Helpers
{
    class AWSHelper
    {
        public static string GetRDSConnectionString(IConfiguration config)
        {
            string dbname = config["connection_string:aws:RDS_DB_NAME"];

            if (string.IsNullOrEmpty(dbname)) return null;

            string username = config["connection_string:aws:RDS_USERNAME"];
            string password = config["connection_string:aws:RDS_PASSWORD"];
            string hostname = config["connection_string:aws:RDS_HOSTNAME"];
            string port = config["connection_string:aws:RDS_PORT"];

            return "Data Source=" + hostname + ";Initial Catalog=" + dbname + ";User ID=" + username + ";Password=" + password + ";";
        }
    }
}
