using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("TileMap")]
public class TileMapContainer {
    [XmlAttribute("rows")]
    public int rows;

    [XmlAttribute("cols")]
    public int cols;

    [XmlArray("Cells"), XmlArrayItem("Cell")]
    public Cell[] cells;

}
