using System;

namespace GameJam.Common
{
    /// <summary>
    /// Mersenne Twister random number generator implementation.
    /// </summary>
    /// <remarks>
    /// Author:
    /// Adam Veldhousen
    /// 
    /// Reference:
    /// https://gist.github.com/adamveld12/6c0350d1cfd2da449dc6#file-mersenneprimerandom-cs-L39
    /// 
    /// License:
    /// 
    /// The MIT License (MIT)
    /// 
    /// Copyright (c) 2014 Adam Veldhousen
    /// 
    /// Permission is hereby granted, free of charge, to any person obtaining a copy of this
    /// software and associated documentation files (the "Software"), to deal in the Software
    /// without restriction, including without limitation the rights to use, copy, modify, merge,
    /// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
    /// to whom the Software is furnished to do so, subject to the following conditions:
    /// 
    /// The above copyright notice and this permission notice shall be included in all copies
    /// or substantial portions of the Software.
    /// 
    /// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
    /// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
    /// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
    /// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
    /// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    /// SOFTWARE.
    /// </remarks>
    public class MTRandom
    {
        readonly uint[] _matrix = new uint[624];
        int _index = 0;

        public MTRandom() : this((uint)(0xFFFFFFFF & DateTime.Now.Ticks))
        {
        }

        public MTRandom(uint seed)
        {
            _matrix[0] = seed;
            for (int i = 1; i < _matrix.Length; i++)
            {
                _matrix[i] = (1812433253 * (_matrix[i - 1] ^ ((_matrix[i - 1]) >> 30) + 1));
            }
        }

        void Generate()
        {
            for (int i = 0; i < _matrix.Length; i++)
            {
                uint y = (_matrix[i] >> 31) + ((_matrix[(i + 1) & 623]) << 1);
                _matrix[i] = _matrix[(i + 397) & 623] ^ (y >> 1);
                if (y % 2 != 0)
                {
                    _matrix[i] = (_matrix[i] ^ 2567483615);
                }
            }
        }

        public int Next()
        {
            if (_index == 0)
            {
                Generate();
            }

            uint y = _matrix[_index];
            y = y ^ (y >> 11);
            y = (y ^ (y >> 7) & 2636928640);
            y = (y ^ (y << 15) & (4022730752));
            y = (y ^ (y >> 18));

            _index = (_index + 1) % 623;
            return (int)(y % int.MaxValue);
        }

        public int Next(int max)
        {
            if (max == 0)
            {
                return 0;
            }

            int randomValue = Next();
            return randomValue % (max + 1);
        }
        /// <summary>
        /// This function returns a random value between min and max.
        /// This function is inclusive on both extremes, meaning that min
        /// and max are both possible return values.
        /// </summary>
        /// <returns> A random value from range [min, max] </returns>
        public int Next(int min, int max)
        {
            if (min > max)
            {
                throw new ArgumentException("Min cannot be greater than max", nameof(min));
            }

            return Next(max - min) + min;
        }

        public float NextSingle()
        {
            int n = Next();
            return (float)n / int.MaxValue;
        }

        public float NextSingle(float min, float max)
        {
            float n = NextSingle();
            return (max - min) * n + min;
        }
    }
}
