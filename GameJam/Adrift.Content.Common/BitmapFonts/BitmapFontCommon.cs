/**
MonoGame.Extended License:

The MIT License (MIT)

Copyright (c) 2015 Dylan Wilson

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
**/

using System.Xml.Serialization;

namespace Adrift.Content.Common.BitmapFonts
{
    // ---- AngelCode BmFont XML serializer ----------------------
    // ---- By DeadlyDan @ deadlydan@gmail.com -------------------
    // ---- There's no license restrictions, use as you will. ----
    // ---- Credits to http://www.angelcode.com/ -----------------		
    public class BitmapFontCommon
    {
        [XmlAttribute("lineHeight")]
        public int LineHeight { get; set; }

        [XmlAttribute("base")]
        public int Base { get; set; }

        [XmlAttribute("scaleW")]
        public int ScaleW { get; set; }

        [XmlAttribute("scaleH")]
        public int ScaleH { get; set; }

        [XmlAttribute("pages")]
        public int Pages { get; set; }

        [XmlAttribute("packed")]
        public int Packed { get; set; }

        [XmlAttribute("alphaChnl")]
        public int AlphaChannel { get; set; }

        [XmlAttribute("redChnl")]
        public int RedChannel { get; set; }

        [XmlAttribute("greenChnl")]
        public int GreenChannel { get; set; }

        [XmlAttribute("blueChnl")]
        public int BlueChannel { get; set; }
    }
}