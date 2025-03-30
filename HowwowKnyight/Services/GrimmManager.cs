using System.Collections;
using Modding.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HowwowKnyight.Services;

public sealed class GrimmManager: IDisposable {
    public const int GGGrimmBuildIndex = 443;
    private const string OwOGrimmResourceName = "OwOGrimm.png";

    private readonly Texture2D OwOGrimmTexture;

    public GrimmManager() {
        OwOGrimmTexture = Utils.LoadTextureFromResources(OwOGrimmResourceName);
    }

    public void SetHooks() {
        USceneMgr.activeSceneChanged += OnSceneChange;
    }

    private void OnSceneChange(Scene oldScene, Scene newScene) {
        if (newScene.buildIndex == GGGrimmBuildIndex) {
            var grimmScene = newScene.FindGameObject("Grimm Scene");
            if (grimmScene == null)
                return;
            var fsm = grimmScene.GetComponent<PlayMakerFSM>();

            IEnumerator ChangeSpriteRoutine(PlayMakerFSM fsm) {
                yield return null;
                while (fsm.ActiveStateName != "Battle Start")
                    yield return null;

                fsm.transform.Find("Grimm Boss")
                    .GetComponent<tk2dSprite>()
                    .GetCurrentSpriteDef().material.mainTexture = OwOGrimmTexture;
                yield break;
            }
            fsm.StartCoroutine(ChangeSpriteRoutine(fsm));
        }
    }

    public void Dispose() {
        USceneMgr.activeSceneChanged -= OnSceneChange;
    }
}