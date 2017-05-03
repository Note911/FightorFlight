using System.Xml;
using System.Xml.Serialization;

public class Cell {

    [XmlAttribute("x")]
    public int x;

    [XmlAttribute("y")]
    public int y;

    [XmlAttribute("Hazard")]
    public string hazard;

    [XmlAttribute("Doodad")]
    public string doodad;

    public int scaleX;
    public int scaleY;
    public int rotation;
    public int tileType;
    public int tileShape;
    public int spriteID;


}
