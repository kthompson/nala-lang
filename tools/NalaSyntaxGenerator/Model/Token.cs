using System.Xml.Serialization;

namespace NalaSyntaxGenerator.Model
{
    [XmlRoot]
    public class Token
    {
        [XmlAttribute]
        public string Name;


        [XmlAttribute]
        public string Content;
    }
}