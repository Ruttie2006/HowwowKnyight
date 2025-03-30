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
    }

    private void OnUpdateTitle(On.MenuStyles.orig_UpdateTitle orig, MenuStyles self) {
        if (self.CurrentStyle > 0 && self.title != null) {
            orig(self);
        }
    }

    private void OnSceneChange(Scene oldScene, Scene newScene) {
        if (newScene.buildIndex != MainMenuBuildIndex)
            return;

        var titleGo = newScene.FindGameObject(TitleObjectName);
        var titleStyle = titleGo.GetComponent<MenuStyleTitle>();
        titleStyle.TitleSprites = [
            ..titleStyle.TitleSprites,
            new() {
                PlatformWhitelist = [.. Enum.GetValues(typeof(RuntimePlatform)).OfType<RuntimePlatform>()],
                Default = CreateSprite(
                    titleStyle.DefaultTitleSprite.Default,
                    // Debug easter egg integration is currently largely untested
                    newScene.FindGameObject("DebugEasterEgg") != null
                        ? LoadDebugTexture()
                        : TitleTexture)
            }
        ];
        titleStyle.SetTitle(titleStyle.TitleSprites.Length - 1);
    }

    public void Dispose() {
        USceneMgr.activeSceneChanged -= OnSceneChange;
        On.MenuStyles.UpdateTitle -= OnUpdateTitle;

        USceneMgr.GetActiveScene()
            .FindGameObject(TitleObjectName)?
            .GetComponent<MenuStyleTitle>()
            .SetTitle(-1);
    }
}