using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// メインシーンのトップスクリプト（ここから開始する）
/// </summary>
public class MainController : MonoBehaviour
{
    [SerializeField]
    FrameController frameController;

    [SerializeField]
    PanelController panelController;

    private void Awake()
    {
        frameController.Initialize(OnClickPanel);
        panelController.Initialize();
    }

    private void OnClickPanel(int index)
    {
        var panelObj = panelController.GetPanelObject(index);
        var hidePanel = panelController.GetHidePanelObjectFromLink();
        var hideLinkPanels = panelController.GetLinkHidePanelObjectList();
        for (int i = 0; i < hideLinkPanels.Count; i++)
        {
            if (panelObj == null || panelObj != hideLinkPanels[i])
            {
                continue;
            }

            // 入れ替え
            panelController.ChangePanelPosition(panelObj, hidePanel);

            Debug.Log($"**>> panelObj:{panelObj.name} hidePanels[i]:{hideLinkPanels[i].name}");
            return;
        }
    }
}
