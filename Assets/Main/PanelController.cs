using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PanelController : MonoBehaviour
{
    /// <summary>�p�l�����i�[���Ă��郊�X�g</summary>
    [SerializeField]
    private List<PanelObject> panelList = new List<PanelObject>();

    /// <summary>�ʒu���X�g</summary>
    private List<Vector3> positionList = null;

    /// <summary>�p�l�����ύX���ꂽ�Ƃ��̃R�[���o�b�N�֐�</summary>
    private Action<PanelObject> onChangePanel;

    public void Initialize(Action<PanelObject> onChangePanel)
    {
        this.onChangePanel = onChangePanel;

        // �ʒu��o�^
        if (positionList == null)
        {
            positionList = new List<Vector3>();
            panelList.ForEach(x => positionList.Add(x.gameObject.transform.localPosition));
        }

        // �p�l����������
        for (int i = 0; i < panelList.Count; i++)
        {
            panelList[i].Initialize(i, OnClickPanel);
        }

        // �Ō�̃p�l���͔�\��
        panelList.Last().SetVisible(false);
    }

    /// <summary>
    /// �Ώۂ̃p�l�����אڂ���p�l���ɔ�\���̃p�l�������邩
    /// </summary>
    /// <param name="panelObject"></param>
    /// <returns></returns>
    public bool IsLinkHidePanelObject(PanelObject panelObject)
    {
        // ��\���p�l��
        var hidePanel = GetHidePanelObject();

        int index = panelList.FindIndex(x => x == panelObject);
        int col = index % GameDefine.MaxCol;
        int row = index / GameDefine.MaxCol;
        
        // ��\���p�l�������E�㉺�ɂ��邩��r
        // ��
        if (GetPanelObject(col - 1, row) == hidePanel) return true;
        // �E
        if (GetPanelObject(col + 1, row) == hidePanel) return true;
        // ��
        if (GetPanelObject(col, row - 1) == hidePanel) return true;
        // ��
        if (GetPanelObject(col, row + 1) == hidePanel) return true;

        return false;
    }

    /// <summary>
    /// ��\���̃p�l�����擾
    /// </summary>
    /// <returns></returns>
    public PanelObject GetHidePanelObject()
    {
        return panelList.Find(x => x.IsVisible == false);
    }

    /// <summary>
    /// �C���f�b�N�X����p�l���擾
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public PanelObject GetPanelObject(int index)
    {
        if (index < 0 || panelList.Count <= index)
        {
            return null;
        }
        return panelList[index];
    }

    /// <summary>
    /// ��ƍs����PanelObject�擾
    /// </summary>
    /// <param name="col"></param>
    /// <param name="row"></param>
    /// <returns></returns>
    public PanelObject GetPanelObject(int col, int row)
    {
        if ((col < 0 || GameDefine.MaxCol <= col)
            || (row < 0 || GameDefine.MaxRow <= row))
        {
            return null;
        }

        int index = row * GameDefine.MaxCol + col;
        return panelList[index];
    }

    /// <summary>
    /// �p�l���̌���
    /// </summary>
    /// <param name="panel1"></param>
    /// <param name="panel2"></param>
    public void ChangePanelPosition(PanelObject panel1, PanelObject panel2)
    {
        int idx1 = panelList.FindIndex(x => x == panel1);
        int idx2 = panelList.FindIndex(x => x == panel2);

        // �ʒu�̌���
        panel1.gameObject.transform.localPosition = positionList[idx2];
        panel2.gameObject.transform.localPosition = positionList[idx1];
        panel1.realIndex = idx2;
        panel2.realIndex = idx1;

        // �z���������ւ�
        var work = panelList[idx1];
        panelList[idx1] = panelList[idx2];
        panelList[idx2] = work;
    }

    /// <summary>
    /// ����������
    /// </summary>
    /// <returns></returns>
    public bool IsComplete()
    {
        var samePanel = panelList.Where(x => x.startIndex == x.realIndex).ToList();
        return (samePanel.Count == panelList.Count);
    }

    /// <summary>
    /// �R���v���[�g���o
    /// </summary>
    public void CompleteEffect()
    {
        // �t���[�������ׂď���
        panelList.ForEach(x => x.SetVisibleFrame(false));
        // �p�l���͑S�\��
        panelList.ForEach(x => x.SetVisible(true));
        // �{�^���𖳌��ɂ���
        panelList.ForEach(x => x.SetEnabledButton(false));
    }

    /// <summary>
    /// �p�l���������ꂽ�Ƃ�
    /// </summary>
    /// <param name="panelObject"></param>
    private void OnClickPanel(PanelObject panelObject)
    {
        // �������p�l���̏㉺���E�ɋ�̃p�l���̈ʒu��������p�l������
        var hidePanel = GetHidePanelObject();
        if (IsLinkHidePanelObject(panelObject))
        {
            // ����ւ�
            ChangePanelPosition(panelObject, hidePanel);

            onChangePanel.Invoke(panelObject);
        }
    }

    /// <summary>
    /// �V���b�t������
    /// </summary>
    public void Shuffle(int shuffleCount)
    {
        while (true)
        {
            // ��\���p�l��
            var hidePanel = GetHidePanelObject();
            int index = panelList.FindIndex(x => x == hidePanel);
            int col = index % GameDefine.MaxCol;
            int row = index / GameDefine.MaxCol;

            PanelObject panel = null;
            switch (UnityEngine.Random.Range(1, 5))
            {
                case 1: panel = GetPanelObject(col - 1, row); break;
                case 2: panel = GetPanelObject(col + 1, row); break;
                case 3: panel = GetPanelObject(col, row - 1); break;
                case 4: panel = GetPanelObject(col, row + 1); break;
            }

            // ���݂�����������ăV���b�t���񐔂��ւ炷
            if (panel != null)
            {
                ChangePanelPosition(panel, hidePanel);
                --shuffleCount;
                if (shuffleCount <= 0)
                {
                    break;
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            string info = "";
            for (int i = 0; i < panelList.Count; i++)
            {
                var panel = panelList[i];//GetPanelObject(i);
                info += $"panel {i} realIndex:{panel.realIndex} startIndex:{panel.startIndex}\n";
            }
            Debug.Log(info);
        }
    }
}
