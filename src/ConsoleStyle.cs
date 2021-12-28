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
    using System.Diagnostics;

    #endregion

    [ DebuggerDisplay("Name: {Name}") ]
    internal sealed class ConsoleStyle : INamedStyle
    {
        private readonly string _name;
        private ConsoleColor? _foreColor;
        private ConsoleColor? _backColor;

        public ConsoleStyle() :
            this(null) { }

        public ConsoleStyle(string name) :
            this(name, null, null) { }

        public ConsoleStyle(ConsoleColor foreColor)  :
            this(null, foreColor, null) { }

        public ConsoleStyle(ConsoleColor? foreColor, ConsoleColor? backColor) :
            this(null, foreColor, backColor) { }

        public ConsoleStyle(string name, ConsoleColor? foreColor, ConsoleColor? backColor)
        {
            _name = name ?? string.Empty;
            _foreColor = foreColor;
            _backColor = backColor;
        }

        public string Name
        {
            get { return _name; }
        }

        public ConsoleColor? ForeColor
        {
            get { return _foreColor; }
            set { _foreColor = value; }
        }

        public ConsoleColor? BackColor
        {
            get { return _backColor; }
            set { _backColor = value; }
        }
    }
}
