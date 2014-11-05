using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace FishTank
{
    [XmlRoot(ElementName = "game")]


    public class Games : Player 
    {
        [XmlAttribute]
        public string Level { get; set; }

        public List<Player> players { get; set; }
    }

    
    public class Player 
    {
        [XmlElement]
        public string PlayerName { get; set; }
        [XmlElement]
        public string HighestScore { get; set; }

        public List<Fish> fishes { get; set; }
    }

    
    public class Fish 
    {
        [XmlAttribute]
        public string Name { get; set; }
      //  [XmlElement]
        public string Colour { get; set; }
       // [XmlElement]
        public bool Caught { get; set; }

    }

    public static class Global
    {
        public static string player { get; set; }
        public static bool close { get; set; }
        public static string score { get; set; }

    }
}
