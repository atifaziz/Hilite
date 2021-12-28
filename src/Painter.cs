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

    using System;
    using System.Xml;
    using System.Text.RegularExpressions;
    using System.Diagnostics;

    #endregion

    internal sealed class Painter : IPainter
    {
        private IStrokePainter _strokePainter;

        public Painter() { }

        public Painter(IStrokePainter strokePainter)
        {
            _strokePainter = strokePainter;
        }

        public IStrokePainter StrokePainter
        {
            get { return _strokePainter; }
            set { _strokePainter = value; }
        }

        public void Paint(string text, Canvas canvas)
        {
            if (text == null || canvas == null)
                return;

            IStrokePainter strokePainter = StrokePainter;

            if (strokePainter == null)
            {
                canvas.Write(text);
                return;
            }

            Stroke[] strokes = strokePainter.Paint(text);

            if (strokes == null)
            {
                canvas.Write(text);
                return;
            }

            int pos = 0;

            foreach (Stroke stroke in strokes)
            {
                canvas.Write(text.Substring(pos, stroke.Index - pos));
                canvas.WriteWithStyle(stroke.StyleName, stroke.Text);

                pos = stroke.Index + stroke.Length;
            }

            canvas.Write(text.Substring(pos));
        }

        internal static IStrokePainter FromXmlNode(XmlNode node)
        {
            Debug.Assert(node != null);
            Debug.Assert(node.NodeType == XmlNodeType.Element);

            CompositePainter painter;

            XmlAttribute typeAttribute = node.Attributes["type"];
            if (typeAttribute == null || string.IsNullOrEmpty(typeAttribute.Value))
                painter = new CompositePainter();
            else
                painter = (CompositePainter) Activator.CreateInstance(Type.GetType(typeAttribute.Value));

            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.NodeType != XmlNodeType.Element)
                    continue;

                IStrokePainter childPainter = null;
                string styleName = childNode.Attributes["style"].Value;

                switch (childNode.LocalName)
                {
                    case "regex":
                        {
                            childPainter = new RegexPainter(
                                childNode.Attributes["pattern"].Value,
                                childNode.Attributes["options"].Value,
                                styleName);
                            break;
                        }
                    case "keywords":
                        {
                            string keywords = Regex.Replace(childNode.InnerText, "\\s{1,}", " ").Trim();
                            childPainter = new RegexPainter(
                                GetKeywords(keywords),
                                XmlConvert.ToBoolean(Mask.EmptyString(((XmlElement) childNode).GetAttribute("ignoreCase"), "0")) ? "gmi" : "gm",
                                styleName);
                            break;
                        }
                }

                if (painter != null)
                    painter.ChildPainters.Add(childPainter);
            }

            return painter;
        }

        private static string GetKeywords(string s)
        {
            return "\\b" + Regex.Replace(s, " ", "\\b|\\b") + "\\b";
        }
    }
}
