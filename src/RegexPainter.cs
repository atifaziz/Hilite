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

    using System.Text.RegularExpressions;

    #endregion

    internal sealed class RegexPainter : IStrokePainter
    {
        private Regex _expression;
        private string _styleName;

        public RegexPainter(string pattern, string flags, string styleName)
        {
            _styleName = styleName;

            RegexOptions options = RegexOptions.ECMAScript;

            if (flags.IndexOf('m') >= 0)
                options |= RegexOptions.Multiline;

            if (flags.IndexOf('i') >= 0)
                options |= RegexOptions.IgnoreCase;

            _expression = new Regex(pattern, options);
        }

        public string StyleName
        {
            get { return _styleName; }
        }

        public Regex Expression
        {
            get { return _expression; }
        }

        public Stroke[] Paint(string text)
        {
            MatchCollection matches = _expression.Matches(text);
            Stroke[] strokes = new Stroke[matches.Count];

            int i = 0;
            foreach (Match match in matches)
                strokes[i++] = new Stroke(match.Index, match.Length, text, _styleName);

            return strokes;
        }
    }
}
