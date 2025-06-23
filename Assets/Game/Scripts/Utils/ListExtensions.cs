using System.Collections.Generic;
using System;

public static class ListExtensions
{
    private static Random rng = new Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1); // случайный индекс от 0 до n
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
