using System.Runtime.CompilerServices;
using System.Text;

namespace HowwowKnyight;

public static class Extensions {
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSeperator(char value) =>
        value is '-' or '.' or ' ' or ':';

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
                is 'a'
                or 'e'
                or 'i'
                or 'o'
                or 'u'
                or 'y'
                or 'n'
                or 'g'
                or 'A'
                or 'E'
                or 'I'
                or 'O'
                or 'U'
                or 'Y'
                or 'N'
                or 'G') {
                return i;
            }
        }
        return -1;
    }
}