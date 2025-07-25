using Parallelograph.Util.Debug;
using Parallelograph.Util.Exceptions;

using System.Xml;
using System.Xml.Linq;

namespace Parallelograph.Controllers
{
    //Store pairs based upon voices: 
    //Example: Start at 0 and set a "last" variable, then iterate through from 1 in the soprano line. Simple for loop. Keep track of
    //octaves in one structure, fifths in another. Write the encoding of pairs 
    internal class MusicXmlParser
    {
        private Dictionary<string, int> PitchClasses = new() //TODO double sharps/flats, etc.
        {
            { "Bsharp", 0},
            { "C", 0 },
            { "Csharp", 1 },
            { "Dflat", 1 },
            { "D", 2 },
            { "Dsharp", 3 },
            { "Eflat", 3 },
            { "E", 4 },
            { "Fflat", 4 },
            { "Esharp", 5 },
            { "F", 5 },
            { "Fsharp", 6 },
            { "Gflat", 6 },
            { "G", 7 },
            { "Gsharp", 8 },
            { "Aflat", 8 },
            { "A", 9 },
            { "Asharp", 10 },
            { "Bflat", 10 },
            { "B", 11 },
            { "Cflat", 11 },
        };
        private Dictionary<int, List<int>> NoteMap = new();
        private List<List<int>> FourParts = new();
        public MusicXmlParser(string musicXmlFilePath)
        {
            XDocument document;
            try
            {
                document = XDocument.Load(musicXmlFilePath);
            }
            catch
            {
                throw new InvalidMusicXmlDataException("Failed to get root element from XML. Make sure you don't have a malformed/corrupted file.");
            }
            try
            {
                DBG.WriteLine("Parsing measures.");
#pragma warning disable CS8602 //We are handling the case where critical elements are null via an exception.
                foreach (var measure in document.Root.Elements("part").Elements("measure"))
                {
                    var notes = measure.Elements("note");
                    var _notes = notes.Count();                                      
                    foreach (var note in notes)
                    {

                        int octave = Int32.Parse(note.Element("pitch").Element("octave").Value);
                        int voice = Int32.Parse(note.Element("voice").Value);
                        string element = $"{note.Element("pitch")?.Element("step")?.Value}{note.Element("accidental")?.Value.Replace("natural", "")}";
                        if (!PitchClasses.ContainsKey(element))
                        {
                            throw new InvalidMusicXmlDataException("Invalid note name found.");
                        }
                        int pitch = PitchClasses[element];

                        if (!NoteMap.ContainsKey(voice))
                        {
                            NoteMap[voice] = new List<int>();
                        }

                        NoteMap[voice].Add((octave * 12) + pitch); //We likely can get away without octave displacement in the map for now, but I want to keep this intact in case we need it later.
                      
                    }
                }
#pragma warning restore CS8602
                DBG.WriteLine("Parsing done.");

            }
            catch
            {
                DBG.WriteLine("In catch block of MusicXmlParser constructor.");
                throw new InvalidMusicXmlDataException("Error parsing MusicXML. Please make sure you have a compatible file.");
            }
        }

        public List<List<int>> CoalesceAndGetFourParts()
        {
            foreach (KeyValuePair<int, List<int>> pair in NoteMap.OrderBy(x => x.Key))
            {
                FourParts.Add(pair.Value);
            }
            return FourParts;
        }
    }
}
