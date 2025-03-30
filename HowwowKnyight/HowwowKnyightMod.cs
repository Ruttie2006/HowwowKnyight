using HowwowKnyight.Services;
using Modding;
using UnityEngine;

namespace HowwowKnyight;

public sealed class HowwowKnyightMod(): Mod(Constants.Name), IGlobalSettings<GlobalSettings>, ITogglableMod {
    public GlobalSettings Settings { get; private set; } = null!;
    public MenuTitleManager TitleManager { get; private set; } = null!;
    public LanguageGetManager LanguageGetManager { get; private set; } = null!;

    public override string GetVersion() {
        return Constants.Version;
    }

    public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects) {
        TitleManager ??= new();
        TitleManager.SetHooks();

        LanguageGetManager ??= new(Settings, this);
        LanguageGetManager.SetHooks();
    }

    public void Unload() {
        TitleManager.Dispose();
        LanguageGetManager.Dispose();
    }

    public void OnLoadGlobal(GlobalSettings s) =>
        Settings = s;
    public GlobalSettings OnSaveGlobal() =>
        Settings;
}