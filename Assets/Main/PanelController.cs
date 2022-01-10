using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PanelController : MonoBehaviour
{
    /// <summary>パネルを格納しているリスト</summary>
    [SerializeField]
    private List<PanelObject> panelList = new List<PanelObject>();

    /// <summary>位置リスト</summary>
    private List<Vector3> positionList = null;

    /// <summary>パネルが変更されたときのコールバック関数</summary>
    private Action<PanelObject> onChangePanel;

    public void Initialize(Action<PanelObject> onChangePanel)
    {
        this.onChangePanel = onChangePanel;

        // 位置を登録
        if (positionList == null)
        {
            positionList = new List<Vector3>();
            panelList.ForEach(x => positionList.Add(x.gameObject.transform.localPosition));
        }

        // パネルを初期化
        for (int i = 0; i < panelList.Count; i++)
        {
            panelList[i].Initialize(i, OnClickPanel);
        }

        // 最後のパネルは非表示
        panelList.Last().SetVisible(false);
    }

    /// <summary>
    /// 対象のパネルが隣接するパネルに非表示のパネルがあるか
    /// </summary>
    /// <param name="panelObject"></param>
    /// <returns></returns>
    public bool IsLinkHidePanelObject(PanelObject panelObject)
    {
        // 非表示パネル
        var hidePanel = GetHidePanelObject();

        int index = panelList.FindIndex(x => x == panelObject);
        int col = index % GameDefine.MaxCol;
        int row = index / GameDefine.MaxCol;
        
        // 非表示パネルが左右上下にあるか比較
        // 左
        if (GetPanelObject(col - 1, row) == hidePanel) return true;
        // 右
        if (GetPanelObject(col + 1, row) == hidePanel) return true;
        // 上
        if (GetPanelObject(col, row - 1) == hidePanel) return true;
        // 下
        if (GetPanelObject(col, row + 1) == hidePanel) return true;

        return false;
    }

    /// <summary>
    /// 非表示のパネルを取得
    /// </summary>
    /// <returns></returns>
    public PanelObject GetHidePanelObject()
    {
        return panelList.Find(x => x.IsVisible == false);
    }

    /// <summary>
    /// インデックスからパネル取得
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
    /// 列と行からPanelObject取得
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
    /// パネルの交換
    /// </summary>
    /// <param name="panel1"></param>
    /// <param name="panel2"></param>
    public void ChangePanelPosition(PanelObject panel1, PanelObject panel2)
    {
        int idx1 = panelList.FindIndex(x => x == panel1);
        int idx2 = panelList.FindIndex(x => x == panel2);

        // 位置の交換
        panel1.gameObject.transform.localPosition = positionList[idx2];
        panel2.gameObject.transform.localPosition = positionList[idx1];
        panel1.realIndex = idx2;
        panel2.realIndex = idx1;

        // 配列内も入れ替え
        var work = panelList[idx1];
        panelList[idx1] = panelList[idx2];
        panelList[idx2] = work;
    }

    /// <summary>
    /// 完成したか
    /// </summary>
    /// <returns></returns>
    public bool IsComplete()
    {
        var samePanel = panelList.Where(x => x.startIndex == x.realIndex).ToList();
        return (samePanel.Count == panelList.Count);
    }

    /// <summary>
    /// コンプリート演出
    /// </summary>
    public void CompleteEffect()
    {
        // フレームをすべて消す
        panelList.ForEach(x => x.SetVisibleFrame(false));
        // パネルは全表示
        panelList.ForEach(x => x.SetVisible(true));
        // ボタンを無効にする
        panelList.ForEach(x => x.SetEnabledButton(false));
    }

    /// <summary>
    /// パネルを押されたとき
    /// </summary>
    /// <param name="panelObject"></param>
    private void OnClickPanel(PanelObject panelObject)
    {
        // 押したパネルの上下左右に空のパネルの位置だったらパネル交換
        var hidePanel = GetHidePanelObject();
        if (IsLinkHidePanelObject(panelObject))
        {
            // 入れ替え
            ChangePanelPosition(panelObject, hidePanel);

            onChangePanel.Invoke(panelObject);
        }
    }

    /// <summary>
    /// シャッフルする
    /// </summary>
    public void Shuffle(int shuffleCount)
    {
        while (true)
        {
            // 非表示パネル
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

            // 存在したら交換してシャッフル回数をへらす
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
