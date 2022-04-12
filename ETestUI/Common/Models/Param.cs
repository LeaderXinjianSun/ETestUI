using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETestUI.Common.Models
{
    public class CardConfig
    {
        public List<int> Cards;
    }
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
        public OpenProperty openProperty;
        public ShortProperty shortProperty;
        public RgProperty rgProperty;
        public ResProperty resProperty;
        public TvsProperty tvsProperty;
        public List<ShortGroupItem> ShortGroupList;
        public List<OpenItem> OpenList;
        public List<ShortItem> ShortList;
        public List<RgItem> RgList;
        public List<ResItem> ResList;
        public List<TvsItem> TvsList;
        public Segment()
        {
            openProperty = new OpenProperty();
            shortProperty = new ShortProperty();
            rgProperty = new RgProperty();
            resProperty = new ResProperty();
            tvsProperty = new TvsProperty();
            ShortGroupList = new List<ShortGroupItem>();
            OpenList = new List<OpenItem>();
            ShortList = new List<ShortItem>();
            RgList = new List<RgItem>();
            ResList = new List<ResItem>();
            TvsList = new List<TvsItem>();
        }
    }
    public enum SegmentType
    {
        开路, 
        短路, 
        光敏, 
        电阻,
        稳压管
    }
    public class TestPoint
    {
        public int Index;
        public string Name;
        public string Alias;
    }
    public class OpenProperty
    {
        public double PropUpLimit;
        public double PropDownLimit;
    }
    public class ShortProperty
    {
        public double PropUpLimit;
        public double PropDownLimit;
    }
    //光敏电阻测试
    public class RgProperty
    {
        public double PropUpLimit;
        public double PropDownLimit;
    }
    //电阻测试
    public class ResProperty
    {
        public double PropUpLimit;
        public double PropDownLimit;
    }
    //双向稳压管测试
    public class TvsProperty
    {
        public double PropUpLimit;
        public double PropDownLimit;
    }
    public class ShortGroupItem
    {
        public int Id;
        public string Content;
        public string Remark;
    }
    public class OpenItem
    {
        public int Id;
        public string Content;
        public bool Select;
    }
    public class ShortItem
    {
        public int Id;
        public string Content;
        public bool Select;
    }
    public class RgItem
    {
        public int Id;
        public string Content;
        public bool Select;
    }
    public class ResItem
    {
        public int Id;
        public string Content;
        public bool Select;
    }
    public class TvsItem
    {
        public int Id;
        public string Content;
        public bool Select;
    }
}