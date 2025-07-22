using System.ComponentModel.Design;
using System.Xml.Linq;

namespace Parallelograph.Controllers
{
    internal class MusicXmlParser
    {
        // private List<XmlElement> measures = new();

        public MusicXmlParser(string musicXmlFilePath)
        {
            XDocument document = XDocument.Load(musicXmlFilePath);


            var parts = document.Root.Elements("part").Elements("measure");
            foreach (var part in parts)
            {
                var voice = part.Elements("voice").ToList();
                Console.WriteLine(voice);
                foreach (var v in voice)
                {
                    Console.WriteLine(v);
                }
            }
        }
    }
}