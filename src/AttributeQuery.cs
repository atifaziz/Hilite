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
    using System.Reflection;

    #endregion

    internal static class AttributeQuery<TAttribute>
        where TAttribute : Attribute
    {
        public static TAttribute Get<TSource>(TSource source)
            where TSource : ICustomAttributeProvider
        {
            return Get(source, false);
        }

        public static TAttribute Get<TSource>(TSource source, bool inherit)
            where TSource : ICustomAttributeProvider
        {
            TAttribute attribute = Find(source, inherit);

            if (attribute == null)
                throw new ObjectNotFoundException(string.Format("The attribute {0} was not found.", typeof(TAttribute).FullName));

            return attribute;
        }

        public static TAttribute Find<TSource>(TSource source)
            where TSource : ICustomAttributeProvider
        {
            return Find(source, false);
        }

        public static TAttribute Find<TSource>(TSource source, bool inherit)
            where TSource : ICustomAttributeProvider
        {
            TAttribute[] attributes = FindAll(source, inherit);
            return attributes.Length > 0 ? attributes[0] : null;
        }

        public static TAttribute FindAll<TSource>(TSource source)
            where TSource : ICustomAttributeProvider
        {
            return Find(source, false);
        }

        public static TAttribute[] FindAll<TSource>(TSource source, bool inherit)
            where TSource : ICustomAttributeProvider
        {
            return (TAttribute[]) source.GetCustomAttributes(typeof(TAttribute), inherit);
        }
    }
}
