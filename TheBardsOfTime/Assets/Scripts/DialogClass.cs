using System.Xml.Serialization;

namespace DialogClass
{
    [XmlRoot("dialogue")]
    public class Dialogue
    {
        [XmlElement("scene")]
        public Scene[] Scenes { get; set; }
    }

    public class Scene
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlArray("lines")]
        [XmlArrayItem("line")]
        public Line[] Lines { get; set; }
    }

    public class Line
    {
        [XmlAttribute("speakerID")]
        public int SpeakerID { get; set; }

        [XmlAttribute("delay")]
        public float Delay { get; set; }

        [XmlElement("text")]
        public string Text { get; set; }
    }
}
