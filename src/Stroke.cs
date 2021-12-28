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
    internal sealed class Stroke
    {
        private int _index;
        private int _length;
        private string _text;
        private string _styleName;

        public Stroke(int index, int length, string text, string styleName)
        {
            _index = index;
            _length = length;
            _text = text;
            _styleName = styleName;
        }

        public int Index
        {
            get { return _index; }
        }

        public int Length
        {
            get { return _length; }
        }

        public string Text
        {
            get { return _text.Substring(_index, _length); }
        }

        public string StyleName
        {
            get { return _styleName; }
        }
    }
}
