using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("Level")]
public class LevelContainer {

    [XmlAttribute("numLayers")]
    public int numLayers;

    [XmlAttribute("rows")]
    public int rows;

    [XmlAttribute("cols")]
    public int cols;

    [XmlArray("Layers"), XmlArrayItem("layer")]
    public TileMapContainer[] layers;

    public void Save(string path) {
        XmlSerializer serializer = new XmlSerializer(typeof(LevelContainer));
        using (StreamWriter stream = new StreamWriter(path, false, System.Text.Encoding.GetEncoding("UTF-8")))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static LevelContainer Load(string path) {
        XmlSerializer serializer = new XmlSerializer(typeof(LevelContainer));
        using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
        {
            return serializer.Deserialize(stream) as LevelContainer;
        }
       
    }
}
