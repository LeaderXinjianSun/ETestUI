using ETestUI.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETestUI.Service
{
    public interface IParameterService
    {
        public Param MyParam { get; set; }
        public bool Load(string path);
        public bool Save(string path);
        public bool Add(string name);
    }
}
