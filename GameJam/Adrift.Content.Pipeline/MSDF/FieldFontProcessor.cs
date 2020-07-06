using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using RoyT.TrueType.Helpers;

namespace FontExtension
{
    
    [ContentProcessor(DisplayName = "Field Font Processor")]
    public class FieldFontProcessor : ContentProcessor<FontDescription, FieldFont>
    {        
        [DisplayName("msdfgen path")]
        [Description("Path to the msdfgen binary used to generate the multi-spectrum signed distance field")]
        [DefaultValue("msdfgen.exe")]
        public virtual string ExternalPath { get; set; } = "msdfgen.exe";


        [DisplayName("resolution")]
        [Description("Resolution of the the texture to store a single glyph in")]
        [DefaultValue(32)]
        public virtual uint Resolution { get; set; } = 32;

        [DisplayName("range")]
        [Description("Distance field range, in pixels in the output texture")]
        [DefaultValue(4)]
        public virtual uint Range { get; set; } = 4;

        [DisplayName("pack textures")]
        [Description("Pack textures into the XNB file directly. Otherwise, textures are put within a folder next to the ini file.")]
        [DefaultValue(true)]
        public virtual bool PackTextures { get; set; } = true;

        [DisplayName("texture folder")]
        [Description("Folder name to place textures if `pack textures` is false. Otherwise, not used.")]
        [DefaultValue("")]
        public virtual string TextureFolder { get; set; } = "";

        [DisplayName("use cvars")]
        [Description("If true, CVar names are used (see `cvar prefix`). Otherwise, paths are used.")]
        [DefaultValue(true)]
        public virtual bool UseCVars { get; set; } = false;

        [DisplayName("cvar prefix")]
        [Description("If `use cvars` is true, textures are referenced using CVars, with a prefix to this value (for example, font_msdf_fontname_texture_)")]
        [DefaultValue("")]
        public virtual string CVarPrefix { get; set; } = "";

        [DisplayName("base")]
        [Description("Height of each line when rendering the font.")]
        [DefaultValue(0)]
        public virtual float Base { get; set; } = 0;

        public override FieldFont Process(FontDescription input, ContentProcessorContext context)
        {
            System.Diagnostics.Debugger.Break();

        string msdfgen = Path.Combine(Directory.GetCurrentDirectory(), ExternalPath);
            string objPath;
            string simplePath = "";
            if (PackTextures)
            {
                // Garbage
                objPath = Path.Combine(Directory.GetCurrentDirectory(), "obj");
            } else
            {
                // Store
                objPath = Path.Combine(Path.GetDirectoryName(input.Path), TextureFolder);

                Uri fullPath = new Uri(objPath, UriKind.Absolute);
                Uri rootPath = new Uri(Directory.GetCurrentDirectory() + "/", UriKind.Absolute);
                Uri relUri = rootPath.MakeRelativeUri(fullPath);
                simplePath = relUri.ToString();
            }

            Console.WriteLine(objPath);
            Console.WriteLine(simplePath);


            if (File.Exists(msdfgen))
            {
                var glyphs = new FieldGlyph[input.Characters.Count];

                // Generate a distance field for each character using msdfgen
                /*Parallel.For(
                    0,
                    input.Characters.Count,
                    i =>
                    {
                        var c = input.Characters[i];
                        glyphs[i] = CreateFieldGlyphForCharacter(c, input, msdfgen, objPath, simplePath);
                    });*/
                for(int i = 0; i < input.Characters.Count; i++)
                {
                    var c = input.Characters[i];
                    glyphs[i] = CreateFieldGlyphForCharacter(c, input, msdfgen, objPath, simplePath);
                }
                
                var kerning = ReadKerningInformation(input.Path, input.Characters);

                float baseSize = Base;

                return new FieldFont(input.Path, glyphs, kerning, Range, Resolution, baseSize);
            }

            throw new FileNotFoundException(
                "Could not find msdfgen. Check your content processor parameters",
                msdfgen);
        }

        private FieldGlyph CreateFieldGlyphForCharacter(char c, FontDescription input, string msdfgen, string objPath, string simplePath)
        {
            var metrics = CreateDistanceFieldForCharacter(input, msdfgen, objPath, c);
            var path = GetOuputPath(objPath, input, c);
            FieldGlyph glyph;
            if (PackTextures)
            {
                glyph = new FieldGlyph(c, File.ReadAllBytes(path), metrics);
            } else
            {
                string name = Path.GetFileNameWithoutExtension(input.Path);
                string contentPath = Path.Combine(simplePath, $"{name}-{(int)c}");
                if(UseCVars)
                {
                    contentPath = CVarPrefix + "character_" + (int)c;
                }
                glyph = new FieldGlyph(c, contentPath, metrics);
            }

            return glyph;
        }    

        private Metrics CreateDistanceFieldForCharacter(FontDescription font, string msdfgen, string objPath, char c)
        {
            Directory.CreateDirectory(objPath);
            var outputPath = GetOuputPath(objPath, font, c);
            var startInfo = new ProcessStartInfo(msdfgen)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                Arguments = $"-font \"{font.Path}\" {(int)c} -o \"{outputPath}\" -size {Resolution} {Resolution} -pxrange {Range} -autoframe -printmetrics"                
            };
            Console.WriteLine($" -font \"{font.Path}\" {(int)c} -o \"{outputPath}\" -size {Resolution} {Resolution} -pxrange {Range} -autoframe -printmetrics");
          
            var process = System.Diagnostics.Process.Start(startInfo);
            if (process == null)
            {
                throw new InvalidOperationException("Could not start msdfgen.exe");
            }

            var output = process.StandardOutput.ReadToEnd();
            return ParseOutput(output);            
        }

        private static Metrics ParseOutput(string output)
        {
            var advance = 0.0f;
            var scale = 0.0f;
            var translation = Vector2.Zero;

            foreach (var line in output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                ParseLine(line, "advance = ", FloatHelper.ParseInvariant, ref advance);
                ParseLine(line, "scale = ", FloatHelper.ParseInvariant, ref scale);
                ParseLine(line, "translate = ", ParseVector2, ref translation);
            }

            return new Metrics(advance, scale, translation);
        }

        private static string GetOuputPath(string objPath, FontDescription font, char c)
        {
            var name = Path.GetFileNameWithoutExtension(font.Path);
            return Path.Combine(objPath, $"{name}-{(int)c}.bmp");
        }

        private static Vector2 ParseVector2(string text)
        {
            var args = text.Split(',');
            return new Vector2(FloatHelper.ParseInvariant(args[0]), FloatHelper.ParseInvariant(args[1]));
        }
        
        private static void ParseLine<T>(string line, string match, Func<string ,T> resultParser, ref T result)
        {
            if (line.StartsWith(match, StringComparison.InvariantCultureIgnoreCase))
            {
                var value = line.Substring(match.Length).Trim();
                result = resultParser(value);                
            }            
        }

        private static List<KerningPair> ReadKerningInformation(string path, IReadOnlyList<char> characters)
        {
            var pairs = new List<KerningPair>();

            var font = RoyT.TrueType.TrueTypeFont.FromFile(path);

            foreach (var left in characters)
            {
                foreach (var right in characters)
                {
                    var kerning = KerningHelper.GetHorizontalKerning(left, right, font);                          
                    if (kerning > 0 || kerning < 0)
                    {
                        // Scale the kerning by the same factor MSDFGEN scales it
                        pairs.Add(new KerningPair(left, right, kerning / 64.0f));
                    }
                }
            }

            return pairs;
        }
    }   
}