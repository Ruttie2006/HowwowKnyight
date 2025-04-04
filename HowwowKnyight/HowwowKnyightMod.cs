using HowwowKnyight.Services;
using Modding;
using UnityEngine;

namespace HowwowKnyight;

public sealed class HowwowKnyight():
    Mod(Constants.Name),
    IGlobalSettings<GlobalSettings>,
    IMenuMod {

    public GlobalSettings Settings { get; private set; } = null!;
    public MenuTitleManager TitleManager { get; private set; } = null!;
    public LanguageGetManager LanguageGetManager { get; private set; } = null!;
    public GrimmManager GrimmManager { get; private set; } = null!;

    public override string GetVersion() {
        return Constants.Version;
    }

    public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects) {
        Settings ??= new();

        TitleManager ??= new();
        if (Settings.MainMenuEnabled)
            TitleManager.SetHooks();

        LanguageGetManager ??= new(Settings, this);
        if (Settings.OwOTextEnabled)
            LanguageGetManager.SetHooks();

        GrimmManager ??= new();
        if (Settings.GrimmEnabled)
            GrimmManager.SetHooks();
    }

    public void Unload() {
        TitleManager.Dispose();
        LanguageGetManager.Dispose();
        GrimmManager.Dispose();
    }

    public void OnLoadGlobal(GlobalSettings s) =>
        Settings = s;
    public GlobalSettings OnSaveGlobal() =>
        Settings;

    public bool ToggleButtonInsideMenu => true;
    public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? toggleButtonEntry) => [
        new() {
            Name = "OwO Text",
            Description = "Whether or not to replace text into OwO language",
            Values = ["Disabled", "Enabled"],
            Loader = () => {
                return Settings.OwOTextEnabled ? 1 : 0;
            },
            Saver = (val) => {
                if (val == 1) {
                    Settings.OwOTextEnabled = true;
                    LanguageGetManager.SetHooks();
                }
                else {
                    Settings.OwOTextEnabled = false;
                    LanguageGetManager.Dispose();
                }
            }
        },
        new() {
            Name = "Grimm",
            Description = "Whether or not to replace the grimm sprites",
            Values = ["Disabled", "Enabled"],
            Loader = () => {
                return Settings.GrimmEnabled ? 1 : 0;
            },
            Saver = (val) => {
                if (val == 1) {
                    Settings.GrimmEnabled = true;
                    GrimmManager.SetHooks();
                }
                else {
                    Settings.GrimmEnabled = false;
                    GrimmManager.Dispose();
                }
            }
        },
        new() {
            Name = "Main Menu",
            Description = "Whether or not to replace the main menu title text",
            Values = ["Disabled", "Enabled"],
            Loader = () => {
                return Settings.MainMenuEnabled ? 1 : 0;
            },
            Saver = (val) => {
                if (val == 1) {
                    Settings.MainMenuEnabled = true;
                    TitleManager.SetHooks();
                }
                else {
                    Settings.MainMenuEnabled = false;
                    TitleManager.Dispose();
                }
            }
        },
    ];
}