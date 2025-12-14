namespace Jumbee.Console;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ConsoleGUI.Common;
using ConsoleGUI.Data;
using ConsoleGUI.Controls;
using ConsoleGUI.Space;

public static class ControlExtensions
{
    public static T[][] Transpose<T>(this T[][] source)
    {
        if (source == null || source.Length == 0)
        {
            return Array.Empty<T[]>();
        }

        // Determine the number of rows (source.Length) and columns (source[0].Length)
        int rowCount = source.Length;
        for (int i = 1; i < rowCount; i++)
        {
            if (source[i].Length != source[0].Length)
            {
                throw new ArgumentException("All inner arrays must have the same length to transpose.");
            }
        }   
        // This assumes all inner arrays have the same length for a successful transpose
        int columnCount = source[0].Length;

        // Create the new jagged array with dimensions swapped
        T[][] result = new T[columnCount][];

        for (int i = 0; i < columnCount; i++)
        {
            // Initialize each inner array of the result with the new row count
            result[i] = new T[rowCount];
            for (int j = 0; j < rowCount; j++)
            {
                // Swap the indices (i, j) to (j, i)
                result[i][j] = source[j][i];
            }
        }

        return result;
    }
        
    public static Margin WithMargin(this Control control, int left, int top, int right, int bottom) => new Margin
    {
        Offset = new Offset(left, top, right, bottom),
        Content = control
    };

    public static Margin WithMargin(this Control control, int offset) => new Margin
    {
        Offset = new Offset(offset, offset, offset, offset),
        Content = control
    };

    public static Border WithBorder(this Control control, BorderStyle style) => new Border
    {
        BorderStyle = style,
        Content = control
    };

    public static VerticalScrollPanel WithVerticalScrollBar(this Control control) => new VerticalScrollPanel
    {
        Content = control
    };  
}
