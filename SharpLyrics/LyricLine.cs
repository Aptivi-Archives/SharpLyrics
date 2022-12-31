/*
 * MIT License
 *
 * Copyright (c) 2022-2023 Aptivi
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpLyrics
{
    /// <summary>
    /// A line from the lyric with its properties
    /// </summary>
    public class LyricLine
    {
        /// <summary>
        /// Lyrical line
        /// </summary>
        public string Line { get; }
        /// <summary>
        /// Starting time of the lyric line
        /// </summary>
        public TimeSpan LineSpan { get; }
        /// <summary>
        /// Group of words from the lyric line
        /// </summary>
        public List<LyricLineWord> LineWords { get; }

        protected internal LyricLine(string line, TimeSpan lineSpan)
        {
            Line = line;
            LineSpan = lineSpan;
            LineWords = LyricReader.GetLyricWords(line);

            if (line.Contains("<") && line.Contains(">"))
                Line = string.Join(" ", LineWords.Select((llw) => llw.Word).ToArray());
        }
    }
}
