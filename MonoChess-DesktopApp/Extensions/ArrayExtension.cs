using System;
using Microsoft.Xna.Framework;

namespace MonoChess_DesktopApp.Extensions
{
    internal static class ArrayExtension
    {
        public static void ForEach<T>(this T[,] source, Action<T> callback) =>
            source.ForEach((x, y) => callback(source[x, y]));

        public static void ForEach<T>(this T[,] source, Action<T, Point> callback) =>
            source.ForEach((x, y) => callback(source[x, y], new Point(x, y)));

        public static void ForEach<T>(this T[,] source, Action<int, int> action)
        {
            for (var x = 0; x < source.GetLength(0); x++)
            for (var y = 0; y < source.GetLength(1); y++)
                action(x, y);
        }
    }
}