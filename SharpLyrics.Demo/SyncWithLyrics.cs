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
using System.Diagnostics;
using System.Threading;

namespace SharpLyrics.Demo
{
    internal class SyncWithLyrics
    {
        static void Main(string[] args)
        {
            // Get the path
            string path = args.Length > 0 ? args[0] : "";

            // Check to see if the lyric file is provided
            if (string.IsNullOrEmpty(path))
                Console.WriteLine("Specify a path to the lyric file.");
            else
            {
                // Here, the lyric file is given. Process it...
                var lyric = LyricReader.GetLyrics(path);
                var lyricLines = lyric.Lines;
                var shownLines = new List<LyricLine>();

                // Start the elapsed time in 3...
                for (int i = 3; i > 0; i--)
                {
                    Console.WriteLine($"{i}...");
                    Thread.Sleep(1000);
                }

                // Go!
                Console.WriteLine("Go!\n");
                var sw = new Stopwatch();
                sw.Start();
                foreach (var ts in lyricLines)
                {
                    while (sw.Elapsed < ts.LineSpan)
                        Thread.Sleep(1);

                    if (sw.Elapsed > ts.LineSpan)
                    {
                        Console.WriteLine(ts.Line);
                        shownLines.Add(ts);
                        if (shownLines.Count == lyricLines.Count)
                            return;
                    }
                }
            }
        }
    }
}