/**
 *
 * This file is a slightly modified version of MonoGame.Extended BitmapFontReader.
 *
 * https://github.com/craftworkgames/MonoGame.Extended/blob/7e91f6437b464c60fa72d692a0265164eb95e768/Source/MonoGame.Extended/BitmapFonts/BitmapFontReader.cs
 *
 * The purpose of this file is to fix BitmapFontReader using relative asset names
 * for textures instead of the raw string. The raw string is required in this
 * case because the texture "path" is actually a CVar name.
 * 
 **/

using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.TextureAtlases;

namespace GameJam.Content.BitmapFonts
{
    public class BitmapFontReader : ContentTypeReader<BitmapFont>
    {
        protected override BitmapFont Read(ContentReader input, BitmapFont existingInstance)
        {
            var textureAssetCount = input.ReadInt32();
            var assets = new List<string>();

            for (var i = 0; i < textureAssetCount; i++)
            {
                var assetName = input.ReadString();
                assets.Add(assetName);
            }

            var textures = assets
                .Select(textureName => input.ContentManager.Load<Texture2D>(textureName))
                .ToArray();

            var lineHeight = input.ReadInt32();
            var regionCount = input.ReadInt32();
            var regions = new BitmapFontRegion[regionCount];

            for (var r = 0; r < regionCount; r++)
            {
                var character = input.ReadInt32();
                var textureIndex = input.ReadInt32();
                var x = input.ReadInt32();
                var y = input.ReadInt32();
                var width = input.ReadInt32();
                var height = input.ReadInt32();
                var xOffset = input.ReadInt32();
                var yOffset = input.ReadInt32();
                var xAdvance = input.ReadInt32();
                var textureRegion = new TextureRegion2D(textures[textureIndex], x, y, width, height);
                regions[r] = new BitmapFontRegion(textureRegion, character, xOffset, yOffset, xAdvance);
            }

            var characterMap = regions.ToDictionary(r => r.Character);
            var kerningsCount = input.ReadInt32();

            for (var k = 0; k < kerningsCount; k++)
            {
                var first = input.ReadInt32();
                var second = input.ReadInt32();
                var amount = input.ReadInt32();

                // Find region
                if (!characterMap.TryGetValue(first, out var region))
                    continue;

                region.Kernings[second] = amount;
            }

            return new BitmapFont(input.AssetName, regions, lineHeight);
        }
    }
}
