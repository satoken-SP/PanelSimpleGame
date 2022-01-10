using System.Collections;
using UnityEngine;

/// <summary>
/// ���C���V�[���̃g�b�v�X�N���v�g�i��������J�n����j
/// </summary>
public class MainController : MonoBehaviour
{
    /// <summary>�ő�X�e�[�W��</summary>
    const int StageCount = 3;

    [SerializeField]
    PanelController panelController;

    [SerializeField]
    TMPro.TextMeshProUGUI stageName;

    private void Awake()
    {
        // ������
        panelController.Initialize(OnChangePanel);

        // �V���b�t��
        panelController.Shuffle(30);

        stageName.SetText($"���ā[��{GameController.instance.nowStage}");
    }

    /// <summary>
    /// �p�l���̌������N�������Ƃ�
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
