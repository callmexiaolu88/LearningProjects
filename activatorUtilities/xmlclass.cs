using System.Collections.Generic;
using System.Xml.Serialization;

namespace activatorUtilities
{
    [XmlRoot("grid")]
    public class xmlclass
    {

        [XmlElement("ext")]
        public List<xmlclassattr> ListInputs = new List<xmlclassattr>();
    }

    public class xmlclassattr
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("peerId")]
        public string Id { get; set; }

        [XmlAttribute("resourceId")]
        public string ResourceId { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("url")]
        public string Url { get; set; }

        [XmlAttribute("switchField")]
        public bool SwitchField { get; set; }

        [XmlAttribute("extObj")]
        public string ExtraString { get; set; } = string.Empty;
    }
}