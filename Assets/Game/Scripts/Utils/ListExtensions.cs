using System.Collections.Generic;
using System;

// Extension methods for List and other IList implementations
public static class ListExtensions
{
    // Static Random instance to generate random numbers
    private static Random rng = new Random();

    /// <summary>
    /// Shuffles the elements of the list in place using Fisher-Yates algorithm.
    /// </summary>
    /// <typeparam name="T">Type of list elements</typeparam>
    /// <param name="list">The list to shuffle</param>
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        // Loop backwards through the list
        while (n > 1)
        {
            n--;
            // Generate a random index between 0 and n (inclusive)
            int k = rng.Next(n + 1);
            // Swap the elements at indices k and n
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}