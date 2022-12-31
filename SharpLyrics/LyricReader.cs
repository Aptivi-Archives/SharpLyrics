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
using System.IO;

namespace SharpLyrics
{
    /// <summary>
    /// Lyrics reader module
    /// </summary>
    public static class LyricReader
    {
        /// <summary>
        /// Gets the lyrics and their properties from the .LRC or .TXT lyric file
        /// </summary>
        /// <param name="path">Path to the LRC or TXT file containing lyrics</param>
        public static Lyric GetLyrics(string path)
        {
            // Check to see if the lyrics path exists
            if (!File.Exists(path))
                throw new FileNotFoundException("Lyric doesn't exist", path);

            // Get the lines and parse them
            var lyricFileLines = File.ReadAllLines(path);
            var lyricLines = new List<LyricLine>();
            foreach (var line in lyricFileLines)
            {
                // Lyric line is usually [00:00.00]Some lyric lines here
                string finalLine = line.Trim();

                // Check the line
                if (string.IsNullOrWhiteSpace(finalLine))
                    // Don't process empty line
                    continue;
                if (!finalLine.StartsWith("["))
                    // Don't process non-lyric info start line
                    continue;
                if (finalLine.Length == finalLine.IndexOf("]") + 1)
                    // Don't process lyric info without lyric line
                    // TODO: Some of them will later be implemented to store song info
                    continue;

                // We need to trim it after splitting the two elements
                string period = "00:" + finalLine.Substring(finalLine.IndexOf("[") + 1, finalLine.IndexOf("]") - 1);
                string text = finalLine.Substring(finalLine.IndexOf("]") + 1).Trim();

                // Parse the period and install the values to the LyricLine
                var periodTs = TimeSpan.Parse(period);
                var lyricLine = new LyricLine(text, periodTs);

                // Add the line
                lyricLines.Add(lyricLine);
            }

            // Return the lyric
            return new Lyric(lyricLines);
        }

        #region Internal functions
        internal static List<LyricLineWord> GetLyricWords(string line)
        {
            var lyricWords = new List<LyricLineWord>();
            var words = line.Split(' ');

            // If the line is in the <Time> format for each word, take them and install them
            for (int i = 0; i < words.Length; i++)
            {
                string timeOrWord = words[i];
                string nextWord = "";
                bool isTime = timeOrWord.Contains("<") && timeOrWord.Contains(">");
                var wordTime = new TimeSpan();

                // If the current word is a time, populate the word variable
                if (isTime)
                {
                    // Strip the time indicators and get the next word
                    timeOrWord = timeOrWord.Replace("<", "").Replace(">", "");
                    nextWord = words[i + 1];
                    i++;

                    // Parse the time
                    wordTime = TimeSpan.Parse($"00:{timeOrWord}");
                }

                // Get the final word and install the values and then add to the dictionary
                string finalWord = isTime ? nextWord : timeOrWord;
                var lyricLineWord = new LyricLineWord(finalWord, wordTime);
                lyricWords.Add(lyricLineWord);
            }

            // Return the final list
            return lyricWords;
        }
        #endregion
    }
}