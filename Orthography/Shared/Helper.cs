﻿using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orthography.Shared
{
	public static class Helper
	{
        public static int ArrayToBitmask(bool[] array)
        {
            var bitmask = 0;
            for (var i = array.Length - 1; i >= 0; --i)
            {
                bitmask |= array[i] ? 1 : 0;
                if (i > 0) bitmask <<= 1;
            }
            return bitmask;
        }

        public static bool[] BitmaskToArray(int bitmask, int arraySize)
        {
            var res = new bool[arraySize];
            for (var i = 0; i < arraySize; ++i)
            {
                res[i] = bitmask % 2 > 0;
                bitmask >>= 1;
            }
            return res;
        }

        public static void DeleteCookie(IJSRuntime jsRuntime, string name)
        {
            WriteCookie(jsRuntime, name, string.Empty, -365);
        }

        public static void WriteCookie(IJSRuntime jsRuntime, string name, string value, int days)
        {
            jsRuntime.InvokeAsync<string>("WriteCookie", name, value, days);
        }

        public static async Task<IEnumerable<KeyValuePair<string, string>>> ReadCookie(IJSRuntime jsRuntime, string name)
        {
            var response = jsRuntime.InvokeAsync<string>("ReadCookie", name).Result;
            if (!string.IsNullOrWhiteSpace(response))
            {
                var cookies = response
                    .Split(";")
                    .Select(p => p.Split("="))
                    .Where(p => p.Length > 1)
                    .Select(p => new KeyValuePair<string, string>(p[0], p[1]));
                return cookies;
            }
            return null;
        }
    }
}
