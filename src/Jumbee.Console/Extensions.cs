namespace Jumbee.Console;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class CollectionExtensions
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

}
