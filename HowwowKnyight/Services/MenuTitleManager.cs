using Modding;
using Modding.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HowwowKnyight.Services;

public sealed class MenuTitleManager: IDisposable {
    public const string TitleObjectName = "LogoTitle";
    public const int MainMenuBuildIndex = 1;

    private const string OwOTitleResourceName = "OwOTitle.png";
    private const string OwODebugTitleResourceName = "OwODebugTitle.png";

    public readonly Texture2D TitleTexture;
    public Texture2D? DebugModTitleTexture { get; private set; } = null;
    private int? TitleIndex = null;

    public MenuTitleManager() {
        TitleTexture = Utils.LoadTextureFromResources(OwOTitleResourceName);
    }

    private static Sprite CreateSprite(Sprite orig, Texture2D tex) =>
        Sprite.Create(
            tex,
            new(0, 0, tex.width, tex.height),
            new(0.5f, 0.5f),
            orig.pixelsPerUnit,
            0,
            SpriteMeshType.FullRect
        );

    private Texture2D LoadDebugTexture() {
        return DebugModTitleTexture = Utils.LoadTextureFromResources(OwODebugTitleResourceName);
    }

    internal void SetHooks() {
        USceneMgr.activeSceneChanged += OnSceneChange;
        On.MenuStyles.UpdateTitle += OnUpdateTitle;
        ModHooks.FinishedLoadingModsHook += OnModLoadFinished;
    }

    private void OnUpdateTitle(On.MenuStyles.orig_UpdateTitle orig, MenuStyles self) {
        if (self.title != null && self.styles[self.CurrentStyle].titleIndex > 0) {
            orig(self);
        }
    }

    private void OnSceneChange(Scene oldScene, Scene newScene) {
        TitleIndex = null;
        RegisterStyles(newScene);
    }

    private void OnModLoadFinished() {
        RegisterStyles(USceneMgr.GetActiveScene());
    }

    private void RegisterStyles(Scene scene) {
        if (scene.buildIndex != MainMenuBuildIndex)
            return;

        var titleGo = scene.FindGameObject(TitleObjectName);
        var titleStyle = titleGo.GetComponent<MenuStyleTitle>();
        if (TitleIndex == null) {
            titleStyle.TitleSprites = [
                ..titleStyle.TitleSprites,
                new() {
                    PlatformWhitelist = [.. Enum.GetValues(typeof(RuntimePlatform)).OfType<RuntimePlatform>()],
                    Default = CreateSprite(
                        titleStyle.DefaultTitleSprite.Default,
                        // Debug easter egg integration is currently largely untested
                        scene.FindGameObject("DebugEasterEgg") != null
                            ? LoadDebugTexture()
                            : TitleTexture)
                }
            ];
            TitleIndex = titleStyle.TitleSprites.Length - 1;
        }
        titleStyle.SetTitle(TitleIndex.GetValueOrDefault());
    }

    public void Dispose() {
        USceneMgr.activeSceneChanged -= OnSceneChange;
        On.MenuStyles.UpdateTitle -= OnUpdateTitle;
        ModHooks.FinishedLoadingModsHook -= OnModLoadFinished;

        USceneMgr.GetActiveScene()
            .FindGameObject(TitleObjectName)?
            .GetComponent<MenuStyleTitle>()
            .SetTitle(-1);
    }
}