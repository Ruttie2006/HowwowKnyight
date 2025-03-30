using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace HowwowKnyight;

public sealed class GlobalSettings {
    public float StutterChance { get; set; } = 0.15f;
    public float FaceChance { get; set; } = 0.2f;

    public readonly List<string> Faces = [
        " uwu",
        " owo",
        " UwU",
        " OwO",
        " >w<",
        " ^w^",
        " QwQ",
        " UwU",
        " @w@",
        " >.<",
        " ÕwÕ",
        "~", "~", "~", "~", "~"
    ];

    public readonly List<ReplacementRule> Replacements = [
        new("R", "W"),
        new("L", "W"),
        new("l", "w"),
        new("OU", "UW"),
        new("Ou", "Uw"),
        new("ou", "uw"),
        new("TH", "D"),
        new("Th", "D"),
        new("th", "d"),

        new(@"N([AEIOU])", @"NY$1", regex: true),
        new(@"N([aeiou])", @"Ny$1", regex: true),
        new(@"n([aeiou])", @"ny$1", regex: true),
        new(@"T[Hh]([UI][^sS])", @"F$1", regex: true),
        new(@"th([ui][^sS])", @"f$1", regex: true),

        new(@"(?<!<b)r(?!>)", @"w", regex: true),
        new(@"T[Hh]\b", @"F", regex: true),
        new(@"th\b", @"f", regex: true),
        new(@"OVE\b", @"UV", regex: true),
        new(@"Ove\b", @"Uv", regex: true),
        new(@"ove\b", @"uv", regex: true),
    ];
}

public class ReplacementRule(string match, string replacement, bool regex = false) {
    [JsonIgnore]
    private Regex? _matchRegex = null;
    [JsonIgnore]
    public Regex MatchRegex => UseRegex
        ? _matchRegex ??= new(Matcher)
        : throw new InvalidOperationException("This is a non-regex match");

    public readonly string Matcher = match;
    public readonly bool UseRegex = regex;
    public readonly string Replacement = replacement;
}