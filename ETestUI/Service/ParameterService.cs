using ETestUI.Common.Models;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETestUI.Service
{
    public class ParameterService : IParameterService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public Param MyParam { get; set; }
        public bool Load(string path)
        {
            try
            {
                string jsonString = File.ReadAllText(path);
                MyParam = JsonConvert.DeserializeObject<Param>(jsonString);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return false;
            }
        }

        public bool Save(string path)
        {
            return true;
        }
    }
}
