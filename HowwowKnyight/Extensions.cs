using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace HowwowKnyight;

public static class Utils {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder ReplaceRange(this StringBuilder sb, in int startIndex, in int maxLength, in string replacement) {
        var i = 0;
        while (i < Math.Min(maxLength, replacement.Length)) {
            sb[startIndex + i] = replacement[i];
            i++;
        }

        if (i < replacement.Length) {
            sb.Insert(startIndex + i, replacement.Substring(i));
        }
        else if (i < maxLength) {
            sb.Remove(startIndex + i, maxLength - i);
        }
        return sb;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool SeperatorWithin(this string input, int end) {
        var len = Math.Min(input.Length, end);
        for (var i = 0; i < len; i++) {
            if (IsSeperator(input[i])) {
                return true;
            }
        }
        return false;
    }

    public static int FirstIndexOf(this string val, Func<char, bool> filter) {
        var len = val.Length;
        for (int i = 0; i < len; i++) {
            if (filter(val[i]))
                return i;
        }
        return -1;
    }

    public static int FirstPhoneticWithLimit(this string val, int limit) {
        var len = Math.Min(val.Length, limit);
        for (var i = 0; i < len; i++) {
            // surely this can be done better
            if (val[i]
                is 'a' or 'e' or 'i' or 'o' or 'u' or 'y' or 'n' or 'g'
                or 'A' or 'E' or 'I' or 'O' or 'U' or 'Y' or 'N' or 'G') {
                return i;
            }
        }
        return -1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSeperator(char value) =>
        value is '-' or '.' or ' ' or ':';

    public static Texture2D LoadTextureFromResources(string name) {
        using var stream = typeof(Utils).Assembly.GetManifestResourceStream(name);
        var buf = new byte[stream.Length];
        _ = stream.Read(buf, 0, buf.Length);
        var tex = new Texture2D(1, 1);
        tex.LoadImage(buf);
        return tex;
    }
}