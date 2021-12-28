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
    using System.Diagnostics;
    using System.IO;

    #endregion

    internal abstract class Canvas
    {
        private TextWriter _writer;
        private IStyle _currentStyle;
        private LazyCollection<Dictionary<string, IStyle>> _styleByName = new LazyCollection<Dictionary<string, IStyle>>();
        private LazyCollection<Stack<IStyle>> _styleStack = new LazyCollection<Stack<IStyle>>();

        protected Canvas(TextWriter writer)
        {
            Debug.Assert(writer != null);

            _writer = writer;
        }

        protected virtual TextWriter BaseWriter
        {
            get { return _writer; }
        }

        public virtual IStyle CurrentStyle
        {
            get { return _currentStyle ?? DefaultStyle; }
        }

        public virtual IDictionary<string, IStyle> Styles
        {
            get { return _styleByName.Collection; }
        }

        public void BeginStyle(string styleName)
        {
            _styleStack.Collection.Push(CurrentStyle);

            IStyle newStyle;
            if (!Styles.TryGetValue(styleName, out newStyle))
                newStyle = DefaultStyle;

            EnterStyle(newStyle);
            _currentStyle = newStyle;
        }

        public void EndStyle()
        {
            IStyle oldStyle = _styleStack.Collection.Pop();
            EnterStyle(oldStyle);
            _currentStyle = oldStyle;
        }

        public virtual void Write(string text)
        {
            _writer.Write(text);
        }

        public virtual void WriteWithStyle(string styleName, string text)
        {
            BeginStyle(styleName);
            Write(text);
            EndStyle();
        }

        protected abstract IStyle DefaultStyle { get; }
        protected abstract void EnterStyle(IStyle style);
    }
}
