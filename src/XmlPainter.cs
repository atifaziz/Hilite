#region License

//
// The zlib/libpng License
// Copyright (c) 2006 Atif Aziz, Skybow AG.
//
// This software is provided 'as-is', without any express or implied 
// warranty. In no event will the authors be held liable for any damages
// arising from the use of this software.
//
// Permission is granted to anyone to use this software for any purpose, 
// including commercial applications, and to alter it and redistribute it
// freely, subject to the following restrictions:
//
// 1. The origin of this software must not be misrepresented; you must not 
//    claim that you wrote the original software. If you use this software in
//    a product, an acknowledgment in the product documentation would be 
//    appreciated but is not required.
//
// 2. Altered source versions must be plainly marked as such, and must not be
//    misrepresented as being the original software.
//
// 3. This notice may not be removed or altered from any source distribution.
//

#endregion

namespace Hilite
{
    #region Imports

    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    #endregion
    
    internal sealed class XmlPainter : CompositePainter
    {
        protected override Stroke[] CollectStrokes(string text)
        {
            List<Stroke> strokes = new List<Stroke>();

            RegexPainter painter;

            //
            // Match CDATA in the following format <![ ... [ ... ]]>
            // <\!\[[\w\s]*?\[(.|\s)*?\]\]>
            //

            painter = new RegexPainter(@"<\!\[[\w\s]*?\[(.|\s)*?\]\]>", "gm", "xml-cdata");
            strokes.AddRange(painter.Paint(text));

            //
            // Match comments
            // <!--\s*.*\s*?-->
            //

            painter = new RegexPainter(@"<!--\s*.*\s*?-->", "gm", "xml-comment");
            strokes.AddRange(painter.Paint(text));

            //
            // Match attributes and their values
            // (\w+)\s*=\s*(".*?"|\'.*?\'|\w+)*
            //
            
            foreach (Match match in Regex.Matches(text, @"([\w-\.]+)\s*=\s*("".*?""|'.*?'|\w+)*", RegexOptions.ECMAScript | RegexOptions.Multiline))
            {
                strokes.Add(new Stroke(match.Groups[1].Index, match.Groups[1].Length, text, "xml-attribute"));

                //
                // If XML is invalid and attribute has no property value, ignore it.
                //

		        if (match.Groups[2].Value.Length > 0)
                    strokes.Add(new Stroke(match.Groups[2].Index, match.Groups[2].Length, text, "xml-attribute-value"));
	        }

	        //
            // Match opening and closing tag brackets
        	// </*\?*(?!\!)|/*\?*>
            //

            painter = new RegexPainter(@"</*\?*(?!\!)|/*\?*>", "gm", "xml-tag");
            strokes.AddRange(painter.Paint(text));

            //
            // Match tag names
	        // </*\?*\s*(\w+)
            //

            foreach (Match match in Regex.Matches(text, @"</*\?*\s*([\w-\.]+)", RegexOptions.ECMAScript | RegexOptions.Multiline))
                strokes.Add(new Stroke(match.Groups[1].Index, match.Groups[1].Length, text, "xml-tag-name"));

            return strokes.ToArray();
        }
    }
}
