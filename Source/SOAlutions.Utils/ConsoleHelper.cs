// © 2013 SOAlutions, Inc. All rights reserved.
// Please direct all inquiries to http://www.soalutions.net

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace SOAlutions.Utils
{
    public class TextBlock
    {
        public int DisplayOrder { get; set; }
        public bool IsVariable { get; set; }
        public string Text { get; set; }
    }

    public static class ConsoleHelper
    {
        public static string GetArgumentValue(string[] args, string argTag)
        {
            string result = String.Empty;
            for (int i = 0; i < args.Count(); i++)
            {
                if (String.Compare(args[i], argTag, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (i + 1 < args.Count())
                    {
                        result = args[i + 1];
                    }
                    break;
                }
            }
            return result;
        }

        public static void WaitForAnyKey(bool showWaitingMessage = true)
        {
            WaitForKey(showWaitingMessage, null);
        }

        public static void WaitForKey(bool showWaitingMessage = true, Nullable<ConsoleKey> expectedKey = null)
        {
            if (showWaitingMessage)
            {
                WriteLine("Press {0} to exit...", expectedKey.HasValue ? $"<{expectedKey.Value.ToString()}>" : "any key");
            }

            ConsoleKeyInfo keyInfo;
            do
            { 
                keyInfo = Console.ReadKey(true);
            }
            while (expectedKey != null && keyInfo.Key != expectedKey);
        }

        public static void WriteHeaderAndTitle(string title)
        {
            System.Console.Title = title;

            WriteBlockLines(title, true, ConsoleColor.Yellow, ConsoleColor.DarkBlue);
        }

        public static void WriteTestSection(string sectionName)
        {
            WriteTextBlock(sectionName, true, ConsoleColor.DarkBlue, ConsoleColor.Cyan);
        }

        public static void WriteTestStep(string stepName)
        {
            WriteTextBlock(stepName, false, ConsoleColor.White, ConsoleColor.DarkCyan);
        }

        public static void WriteError(string message)
        {
            WriteTextBlock(message, true, ConsoleColor.White, ConsoleColor.Red);
        }

        public static void WriteInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void WriteInfo(string format, params object[] args)
        {
            List<TextBlock> textBlocks = GetTextBlocks(format, args);

            foreach (var textBlock in textBlocks.OrderBy(tb => tb.DisplayOrder))
            {
                if (textBlock.IsVariable)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                }
                Console.Write(textBlock.Text);
                Console.ResetColor();
            }
            Console.WriteLine();
        }

        public static void SetWindowSize(int width, int height)
        {
            Console.SetWindowSize(
                Math.Min(width, Console.LargestWindowWidth),
                Math.Min(height, Console.LargestWindowHeight));
            MakeWindowSizeStatic();
        }

        #region Private Methods

        private static bool DetermineSymbols(string currentSymbol, ref int currentArgIndex, ref string currentSymbolModifier)
        {
            string fullSymbol = currentSymbol.Trim('{', '}');
            if (fullSymbol.Contains(":"))
            {
                var symbolParts = fullSymbol.Split(':');
                currentArgIndex = Int32.Parse(symbolParts[0]);
                currentSymbolModifier = symbolParts[1];
                return true;
            }
            else
            {
                currentArgIndex = Int32.Parse(fullSymbol);
                currentSymbolModifier = null;
                return false;
            }
        }

        private static List<TextBlock> GetTextBlocks(string format, object[] args)
        {
            List<TextBlock> textBlocks = new List<TextBlock>();

            int currentDisplayOrder = 0;
            char currentChar;
            int nextIndex = 0;
            int testIndex;
            string currentSymbol;
            int currentArgIndex = 0;
            string currentSymbolModifier = null;
            string formattedText = null;
            bool isVariable = false;

            for (int position = 0; position < format.Length;)
            {
                currentChar = format[position];
                if (currentChar == '{')
                {
                    nextIndex = format.IndexOf('}', position) + 1; // inclusive
                    currentSymbol = format.Substring(position, nextIndex - position);

                    bool hasModifier = DetermineSymbols(currentSymbol, ref currentArgIndex, ref currentSymbolModifier);

                    if (hasModifier)
                    {
                        currentSymbol = "{0:" + currentSymbolModifier + "}";
                    }
                    else
                    {
                        currentSymbol = "{0}";
                    }

                    formattedText = String.Format(currentSymbol, args[currentArgIndex]);
                    isVariable = true;
                }
                else
                {
                    testIndex = format.IndexOf('{', position); // exclusive
                    if (testIndex >= 0)
                    {
                        formattedText = format.Substring(position, testIndex - position);
                        nextIndex = testIndex;
                    }
                    else
                    {
                        formattedText = format.Substring(position);
                        nextIndex += formattedText.Length;
                    }
                    isVariable = false;
                }

                var tb = new TextBlock
                {
                    DisplayOrder = currentDisplayOrder++,
                    Text = formattedText,
                    IsVariable = isVariable,
                };
                textBlocks.Add(tb);

                position = nextIndex;
            }

            return textBlocks;
        }

        private static void WriteTextBlock(string text, bool showPadding, ConsoleColor foreColor, ConsoleColor backColor)
        {
            int padding = 2;
            int width = Console.WindowWidth;
            int halfWidth = width / 2;
            int minWidth = Math.Max(text.Length + (padding * 2), halfWidth);

            WriteBlockLines(text, showPadding, foreColor, backColor, minWidth);
        }

        private static void WriteBlockLines(string text, bool showPadding, ConsoleColor foreColor, ConsoleColor backColor, int width = 0)
        {
            bool isFullWidth = false;
            if (width == 0 || width >= Console.WindowWidth)
            {
                width = Console.WindowWidth;
                isFullWidth = true;
            }
            int halfWidth = width / 2;

            if (!isFullWidth)
            {
                System.Console.WriteLine();
                if (showPadding)
                {
                    System.Console.WriteLine();
                }
            }
            System.Console.ForegroundColor = foreColor;
            System.Console.BackgroundColor = backColor;
            if (showPadding)
            {
                WriteLine(String.Empty.PadLeft(width, ' '), isFullWidth);
            }
            WriteLine(text.PadLeft((halfWidth + (text.Length / 2)), ' ').PadRight(width, ' '), isFullWidth);
            if (showPadding)
            {
                WriteLine(String.Empty.PadLeft(width, ' '), isFullWidth);
            }
            System.Console.ResetColor();
            System.Console.WriteLine();
        }

        private static void WriteLine(string format, params object[] args)
        {
            WriteLine(String.Format(format, args));
        }

        private static void WriteLine(string value, bool isFullWidth = false)
        {
            if (isFullWidth)
            {
                System.Console.Write(value);
            }
            else
            {
                System.Console.WriteLine(value);
            }
        }

        #endregion Private Methods

        #region External Methods

        private const int MF_BYCOMMAND = 0x00000000;
        private const int SC_CLOSE = 0xF060;
        private const int SC_MINIMIZE = 0xF020;
        private const int SC_MAXIMIZE = 0xF030;
        private const int SC_SIZE = 0xF000;

        [DllImport("user32.dll")]
        private static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        private static void MakeWindowSizeStatic()
        {
            IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);

            if (handle != IntPtr.Zero)
            {
                DeleteMenu(sysMenu, SC_MINIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
            }
        }

        #endregion External Methods
    }
}
