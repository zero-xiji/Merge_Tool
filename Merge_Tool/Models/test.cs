using System.Xml.Serialization;

namespace Merge_Tool.Models
{
    [XmlRoot("CatCollections")]
    public class test
    {
        [XmlArray("Collection")]
        public CatCollection[] CatCollection { get; set; }
    }

    [XmlRoot("cats")]
    public class CatCollection
    {
        [XmlArray("items"), XmlArrayItem("acat")]
        public Cat[] Cats { get; set; }
    }

    [XmlRoot("cat")]
    public class Cat
    {
        //定义Color属性的序列化为cat节点的属性
        [XmlAttribute("color")]
        public string Color { get; set; }

        //要求不序列化Speed属性
        [XmlIgnore]
        public int Speed { get; set; }

        //设置Saying属性序列化为Xml子元素
        [XmlElement("saying")]
        public string Saying { get; set; }
    }

}
