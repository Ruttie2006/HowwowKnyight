using System.Text;
using System.Text.RegularExpressions;
using Modding;

namespace HowwowKnyight.Services;

public sealed class LanguageGetManager(GlobalSettings settings, ILogger logger): IDisposable {
    private readonly GlobalSettings Settings = settings;
    private readonly StringBuilder _sb = new();
    private readonly ILogger Logger = logger;

    public void SetHooks() {
        ModHooks.LanguageGetHook += OnLanguageGet;
    }

    public string OnLanguageGet(string key, string sheetTitle, string orig) {
        try {
            if (orig.All(x => char.IsDigit(x)))
                return orig;

            if (ModHooks.GetMod("Firemoth", onlyEnabled: true) is null) {
                // manual overrides
                switch (orig) {
                    case "Troupe Master":
                        return "OwO Mastew";
                    case "Uumuu":
                        return "Uuwuu";
                }
            }

            return OwOIfy(orig);
        }
        catch (Exception ex) {
            Logger.LogError($"Exception in owo-ify: {ex}");
#if DEBUG
            return "OWO NOOOOOOOOOOOOOOOOOOOOOOOOO";
#else
            return orig;
#endif
        }
    }

    public string OwOIfy(string owig) {
        var sb = _sb;
        sb.EnsureCapacity(owig.Length);

        try {
            if (owig.StartsWith("Oh", StringComparison.Ordinal)) {
                sb.Append("Uh");
                sb.Append(owig, 2, owig.Length - 2);
            }
            else if (owig.Length > 3
                && URandom.value < Settings.StutterChance
                && owig.SeperatorWithin(6)
                && owig.FirstPhoneticWithLimit(5) is > 0 and { } phoneticMatch) {

                sb.Append(owig, 0, phoneticMatch + 1);
                sb.Append('-');
                var firstWord = owig.FirstIndexOf(x => char.IsLetterOrDigit(x) || x == '_');
                if (firstWord != -1) {
                    sb.Append(owig, firstWord, owig.Length - firstWord);
                }
            }
            else {
                sb.Append(owig);
            }

            var faceAdded = false;
            sb.Replace("what is that", "whats this");
            if (owig.IndexOf("What is that", StringComparison.Ordinal) != -1) {
                sb.Replace("What is that", "OWO whats this");
                faceAdded = true;
            }
            sb.Replace("Little", "Widdow"); // idk why these are here, should they be moved to settings?
            sb.Replace("little", "widdow");
            if (owig.IndexOf("!", StringComparison.Ordinal) != -1) {
                sb.Replace("!", "! >w<");
                faceAdded = true;
            }

            if (owig[owig.Length - 1] == '?' || (!faceAdded && URandom.value < Settings.FaceChance)) {
                while (Extensions.IsSeperator(sb[sb.Length - 1])) {
                    sb.Length -= 1;
                }
                var faceIndex = URandom.Range(0, Settings.Faces.Count - 1);
                sb.Append(Settings.Faces[faceIndex]);
            }

            foreach (var rule in Settings.Replacements) {
                if (rule.UseRegex) {
                    var regexStr = sb.ToString(); // regex is an allocation mess, and requires a string, so we have to create one

                    var offset = 0; // .Matches will go through them in order, which means we can simply use an offset
                    var matches = rule.MatchRegex.Matches(regexStr);
                    if (matches.Count > 0) {
                        foreach (Match match in matches) {
                            var startLen = sb.Length;
                            sb.ReplaceRange(match.Index + offset, match.Length, rule.Replacement);
                            for (int i = 1; i < match.Groups.Count; i++) {
                                sb.Replace($"${i}", match.Groups[i].Value, match.Index + offset, rule.Replacement.Length);
                            }
                            offset += sb.Length - startLen;
                        }
                    }
                }
                else {
                    sb.Replace(rule.Matcher, rule.Replacement);
                }
            }

            return sb.ToString();
        }
        finally {
            sb.Clear();
        }
    }

    public void Dispose() {
        ModHooks.LanguageGetHook -= OnLanguageGet;
        _sb.Capacity = _sb.Length;
    }
}