using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETestUI.Common.Models
{
    public class Param
    {
        public string? COM;
        public List<Project>? Projects;
    }
    public class Project
    {
        public int Id;
        public string? Name;        
    }
}