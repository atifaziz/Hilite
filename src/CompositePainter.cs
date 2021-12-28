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
    using System.Collections.Generic;
    using System.Diagnostics;

    #endregion

    internal class CompositePainter : IStrokePainter
    {
        private LazyCollection<List<IStrokePainter>> _childPainters = new LazyCollection<List<IStrokePainter>>();

        public virtual Stroke[] Paint(string text)
        {
            Stroke[] strokes = CollectStrokes(text);

            if (strokes == null)
                return null;

            Array.Sort(strokes, delegate(Stroke m1, Stroke m2)
            {
                //
                // Sort matches by index first.
                //

                if (m1.Index < m2.Index)
                    return -1;
                else if (m1.Index > m2.Index)
                    return 1;
                else
                {
                    //
                    // If index is the same, sort by length.
                    //

                    if (m1.Length < m2.Length)
                        return -1;
                    else if (m1.Length > m2.Length)
                        return 1;
                }
                return 0;
            });

            List<Stroke> rootStrokes = new List<Stroke>();

            //
            // The following loop checks to see if any of the matches are inside
            // of other matches. This process would get rid of highligting strings
            // inside comments, keywords inside strings and so on.
            //

            foreach (Stroke stroke in strokes)
            {
                if (!OverlapsAny(strokes, stroke))
                    rootStrokes.Add(stroke);
                else
                    Trace.WriteLine(string.Format("Dropping {0} for '{1}'", stroke.StyleName, stroke.Text));
            }

            return rootStrokes.ToArray();
        }

        protected virtual Stroke[] CollectStrokes(string text)
        {
            List<Stroke> strokes = new List<Stroke>();

            foreach (IStrokePainter brush in ChildPainters)
                strokes.AddRange(brush.Paint(text));

            return strokes.ToArray();
        }

        public virtual bool HasChildPainters
        {
            get { return _childPainters.HasCollection; }
        }

        public virtual List<IStrokePainter> ChildPainters
        {
            get { return _childPainters.Collection; }
        }

        private static bool OverlapsAny(ICollection<Stroke> strokes, Stroke testStroke)
        {
            if (testStroke.Length == 0)
                return false;

            foreach (Stroke stroke in strokes)
            {
                if ((testStroke.Index > stroke.Index) &&
                    (testStroke.Index < stroke.Index + stroke.Length))
                    return true;
            }

            return false;
        }
    }
}
