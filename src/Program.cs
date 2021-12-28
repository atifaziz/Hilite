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
    using System.IO;
    using System.Reflection;
    using System.Xml;

    #endregion

    internal static class Program
    {
        private static int Main(string[] args)
        {
            try
            {
                Process(StripCommandLineComment(args));
                return 0;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.GetBaseException().Message);
                Trace.WriteLine(e.ToString());
                return -1;
            }
        }

        private const string _configurationResourceName = "Configuration.xml";

        private static void Process(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Write(Console.In.ReadToEnd());
                return;
            }

            string first = args[0];
            string source = args.Length > 1 ? args[1] : string.Empty;

            if ("?".Equals(first))
            {
                ShowUsgae();
            }
            else
            {
                string path = first;

                if ("-".Equals(path))
                {
                    Process(Console.In.ReadToEnd(), source);
                }
                else
                {
                    if (source.Length == 0)
                        source = Path.GetExtension(path).ToLowerInvariant().TrimStart('.');

                    Process(File.ReadAllText(path), source);
                }
            }
        }

        private static void Process(string text, string source)
        {
            XmlDocument configDocument = GetXmlResource(_configurationResourceName);

            Painter painter = new Painter();
            XmlNode painterNode = configDocument.SelectSingleNode("/configuration/painters/painter[contains(concat(' ', concat(@for, ' ')), ' " + source + " ')]");

            if (painterNode != null)
                painter.StrokePainter = Painter.FromXmlNode(painterNode);

            Canvas canvas = new ConsoleCanvas();
            LoadStyles(canvas.Styles, configDocument.SelectNodes("/configuration/styles/style"));

            ConsoleColor oldForeColor = Console.ForegroundColor;
            ConsoleColor oldBackColor = Console.BackgroundColor;

            try
            {
                painter.Paint(text, canvas);
            }
            finally
            {
                Console.ForegroundColor = oldForeColor;
                Console.BackgroundColor = oldBackColor;
            }
        }

        private static void LoadStyles(IDictionary<string, IStyle> styleByName, XmlNodeList styleNodes)
        {
            foreach (XmlElement styleElement in styleNodes)
            {
                string styleName = styleElement.GetAttribute("id");
                ConsoleStyle style = new ConsoleStyle(styleName);
                style.ForeColor = TryParse<ConsoleColor>(styleElement.GetAttribute("foreColor"), true);
                style.BackColor = TryParse<ConsoleColor>(styleElement.GetAttribute("backColor"), true);
                styleByName[styleName] = style;
            }
        }

        private static XmlDocument GetXmlResource(string name)
        {
            Type type = typeof(Program);
            using (Stream stream = type.Assembly.GetManifestResourceStream(type, name))
            {
                XmlDocument document = new XmlDocument();
                document.Load(stream);
                return document;
            }
        }

        private static T? TryParse<T>(string value, bool ignoreCase) where T : struct
        {
            if (string.IsNullOrEmpty(value))
                return null;

            Type type = typeof(T);
            Debug.Assert(type.IsEnum);

            try
            {
                return (T) Enum.Parse(type, value, ignoreCase);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        private static void ShowUsgae()
        {
            WriteLogo();

            Console.WriteLine("Usage: hilite [ ( FILENAME | - | ? ) [ LANGUAGE ] ]");
            Console.WriteLine();

            Console.WriteLine("Supported languages and dialects are:");
            Console.WriteLine();

            //
            // Display supported grammars.
            //

            XmlDocument configDocument = GetXmlResource(_configurationResourceName);

            foreach (XmlElement painterElement in configDocument.SelectNodes("/configuration/painters/painter"))
            {
                Console.WriteLine("{0,-12}: {1}",
                    painterElement.GetAttribute("displayName"),
                    painterElement.GetAttribute("for"));
            }
        }

        private static void WriteLogo()
        {
            Assembly assembly = typeof(Program).Assembly;

            Console.WriteLine("{0}, v{1}",
                AttributeQuery<AssemblyTitleAttribute>.Get(assembly).Title,
                AttributeQuery<AssemblyFileVersionAttribute>.Get(assembly).Version);
            Console.WriteLine(AttributeQuery<AssemblyDescriptionAttribute>.Get(assembly).Description);
            Console.WriteLine("By Atif Aziz -- http://www.raboof.com/");
            Console.WriteLine(AttributeQuery<AssemblyCopyrightAttribute>.Get(assembly).Copyright);
            Console.WriteLine();
        }

        private static string[] StripCommandLineComment(string[] args)
        {
            Debug.Assert(args != null);

            for (int i = 0; i < args.Length; i++)
            {
                if ("--".Equals(args[i]))
                {
                    string[] actuals = new string[i];
                    Array.Copy(args, actuals, i);
                    return actuals;
                }
            }

            return args;
        }
    }
}
