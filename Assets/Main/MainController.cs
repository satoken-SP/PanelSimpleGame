using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���C���V�[���̃g�b�v�X�N���v�g�i��������J�n����j
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

            // ����ւ�
            panelController.ChangePanelPosition(panelObj, hidePanel);

            Debug.Log($"**>> panelObj:{panelObj.name} hidePanels[i]:{hideLinkPanels[i].name}");
            return;
        }
    }
}
