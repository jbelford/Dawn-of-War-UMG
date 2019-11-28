using System;

namespace DowUmg.Extensions
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Search for the index that satisfies the compare method. Requires array to be sorted.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="compare"></param>
        /// <returns>Index of value or -1 if not found</returns>
        public static int BinarySearch<T>(this T[] arr, Func<T, int> compare)
        {
            int low = 0;
            int high = arr.Length;

            while (low < high)
            {
                int mid = (high + low) / 2;

                int result = compare.Invoke(arr[mid]);
                if (result == 0)
                {
                    return mid;
                }

                if (result > 0)
                {
                    low = mid + 1;
                }
                else
                {
                    high = mid;
                }
            }

            return -1;
        }
    }
}
