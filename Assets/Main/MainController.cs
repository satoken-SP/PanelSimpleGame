using System.Collections;
using UnityEngine;

/// <summary>
/// メインシーンのトップスクリプト（ここから開始する）
/// </summary>
public class MainController : MonoBehaviour
{
    /// <summary>最大ステージ数</summary>
    const int StageCount = 3;

    [SerializeField]
    PanelController panelController;

    [SerializeField]
    TMPro.TextMeshProUGUI stageName;

    private void Awake()
    {
        // 初期化
        panelController.Initialize(OnChangePanel);

        // シャッフル
        panelController.Shuffle(30);

        stageName.SetText($"すてーじ{GameController.instance.nowStage}");
    }

    /// <summary>
    /// パネルの交換が起こったとき
    /// </summary>
    /// <param name="panelObject"></param>
    private void OnChangePanel(PanelObject panelObject)
    {
        if (panelController.IsComplete())
        {
            Debug.Log("***>>>    COMPLETE  !!!!");

            panelController.CompleteEffect();

            if (GameController.instance.nowStage < StageCount)
            {
                StartCoroutine(NextStage());
            }
        }
    }

    private IEnumerator NextStage()
    {
        yield return new WaitForSeconds(4f);

        GameController.instance.nowStage += 1;
        GameController.instance.sceneChanger.ChangeScene(SceneType.MainScene);
    }
}
