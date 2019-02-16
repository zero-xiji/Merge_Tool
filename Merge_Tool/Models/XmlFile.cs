using Merge_Tool.Resource;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Merge_Tool.Models
{
    [XmlRoot("StaticInspection")]
    public class XmlFile
    {
        #region 属性
        [XmlAttribute("version")]
        public string version;

        [XmlAttribute("date")]
        public string date;
        #endregion

        #region 子元素
        [XmlElement("RegisteredType")]
        public string RegisteredType;
        #endregion

        #region 子序列
        [XmlArrayItem("Inspection")]
        public XmlInspection[] Inspection { get; set; }
        #endregion
        
    }


    [XmlRoot("Inspection")]
    public class XmlInspection
    {
        #region 属性
        [XmlAttribute("code")]
        public string Code;

        [XmlAttribute("switch")]
        public string Switch;
        #endregion

        #region 子节点
        [XmlElement("Name")]
        public string Name;
        #endregion

        #region 子序列
        [XmlArrayItem("Step")]
        public XmlStep[] Step { get; set; }
        #endregion
    }

    [XmlRoot("Step")]
    public class XmlStep
    {
        #region 属性
        [XmlAttribute("start")]
        public string start;

        [XmlAttribute("measure")]
        public string measure;
        #endregion

        #region 子元素
        [XmlElement("Name")]
        public string Name;

        [XmlElement("Standard")]
        public XmlStanderd Standard;

        [XmlElement("Message"), XmlAttribute("id")]
        public string Message_id;
        #endregion
    }

    [XmlRoot("Standard")]
    public class XmlStanderd
    {
        [XmlAttribute("min")]
        public string min;

        [XmlAttribute("max")]
        public string max;

        [XmlAttribute("deltaFlag")]
        public string DeltaFlag;

        [XmlAttribute("deltaValue")]
        public string DeltaValue;

    }
}
