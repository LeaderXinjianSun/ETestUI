using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETestUI.Common.Models
{
    public class Param
    {
        public string COM;
        public int SelectedIndex;
        public List<Project> Projects;
        public Param()
        {
            Projects = new List<Project>();
        }
    }
    public class Project
    {
        public int Id;
        public string Name;
        public DateTime Create;
        public DateTime Modify;
        public List<Segment> Segments;
        public List<TestPoint> TestPoints;
        public Project()
        {
            Segments = new List<Segment>();
            TestPoints = new List<TestPoint>();
        }
    }
    public class Segment
    {
        public int Id;
        public string Name;
        public SegmentType segmentType;
        public OpenTable openTable;
        public ShortTable shortTable;
        public LEDVoltageTable lEDVoltageTable;
        public LEDLightTable lEDLightTable;
        public Segment()
        {
            openTable = new OpenTable();
            shortTable = new ShortTable();
            lEDVoltageTable = new LEDVoltageTable();
            lEDLightTable = new LEDLightTable();
        }
    }
    public enum SegmentType
    {
        开路, 
        短路, 
        LED压降, 
        LED亮灭
    }
    public class TestPoint
    {
        public int Index;
        public string Name;
        public string Alias;
    }
    public class TestPointTableBase
    {
        public string Name;
        public List<int> Points;
        public bool IsUsed;
        public TestPointTableBase()
        {
            Points = new List<int>();
        }
    }
    public class OpenTable : TestPointTableBase
    { }
    public class ShortTable : TestPointTableBase
    { }
    public class LEDVoltageTable : TestPointTableBase
    { }
    public class LEDLightTable : TestPointTableBase
    { }
}