using System.Collections.Generic;
using System.Linq;

namespace FontExtension
{
    public class FontDescription
    {
        public FontDescription(string path, params char[] characters)
        {
            Path = path;
            Characters = characters.ToList().AsReadOnly();
        }

        public string Path { get; }
        public IReadOnlyList<char> Characters { get; }
    }
}
