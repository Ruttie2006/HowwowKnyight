using Modding.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HowwowKnyight.Services;

// Unused
[Obsolete("In incomplete state", true)]
public sealed class GrimmManager: IDisposable {
    public const int GGGrimmBuildIndex = 443;
    private const string OwOGrimmResourceName = "OwOGrimm.png";

    private readonly Texture2D OwOGrimmTexture;

    public GrimmManager() {
        using var owoGrimmStream = typeof(HowwowKnyightMod).Assembly.GetManifestResourceStream(OwOGrimmResourceName);
        var buf = new byte[owoGrimmStream.Length];
        _ = owoGrimmStream.Read(buf, 0, buf.Length);
        OwOGrimmTexture = new Texture2D(1, 1);
        OwOGrimmTexture.LoadImage(buf);
    }

    public void SetHooks() {
        USceneMgr.activeSceneChanged += OnSceneChange;
    }

    private void OnSceneChange(Scene oldScene, Scene newScene) {
        if (newScene.buildIndex == GGGrimmBuildIndex) {
            var grimmScene = newScene.FindGameObject("Grimm Scene");
            if (grimmScene == null)
                return;
            //grimmScene.GetComponent<Transform>().StartCoroutine("");
        }
    }

    public void Dispose() {
        throw new NotImplementedException();
    }
}