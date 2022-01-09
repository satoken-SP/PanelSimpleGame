using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PanelController : MonoBehaviour
{
    [SerializeField]
    private List<PanelObject> panelList = new List<PanelObject>();

    /// <summary>位置リスト</summary>
    private List<Vector3> positionList = null;

    public void Initialize()
    {
        // 位置を登録
        if (positionList == null)
        {
            positionList = new List<Vector3>();
            panelList.ForEach(x => positionList.Add(x.gameObject.transform.localPosition));
        }

        for (int i = 0; i < panelList.Count; i++)
        {
            panelList[i].Initialize(i);
        }

        // 隣接するパネル設定
        for (int i = 0; i < panelList.Count; i++)
        {
            int col = i % GameDefine.MaxCol;
            int row = i / GameDefine.MaxCol;
            var left = GetPanelObject(col - 1, row);
            var right = GetPanelObject(col + 1, row);
            var up = GetPanelObject(col, row - 1);
            var down = GetPanelObject(col, row + 1);
            panelList[i].SetLink(left, right, up, down);
        }

        // 最後のパネルは非表示
        panelList.Last().SetVisible(false);
    }

    /// <summary>
    /// 隣接するパネルで非表示のあるパネルを取得する
    /// </summary>
    /// <returns></returns>
    public List<PanelObject> GetLinkHidePanelObjectList()
    {
        List<PanelObject> hideLinkPanelList = new List<PanelObject>();
        foreach (var panelObject in panelList)
        {
            var hideLinkPanel = panelObject.GetLinkHidePanelObject();
            if (hideLinkPanel != null)
            {
                hideLinkPanelList.Add(hideLinkPanel);
            }
        }

        return hideLinkPanelList;
    }

    /// <summary>
    /// 隣接するパネルで非表示のパネルがあれば非表示のパネルを返す
    /// </summary>
    /// <returns></returns>
    public PanelObject GetHidePanelObjectFromLink()
    {
        foreach (var panelObject in panelList)
        {
            var hidePanel = panelObject.GetHidePanelObjectFromLink();
            if (hidePanel != null)
            {
                return hidePanel;
            }
        }

        return null;
    }

    public PanelObject GetPanelObject(int index)
    {
        if (index < 0 || panelList.Count <= index)
        {
            UnityEngine.Assertions.Assert.IsTrue(true, "GetPanelObject関数で取得に失敗しました。");
            return null;
        }
        return panelList[index];
    }

    public void ChangePanelPosition(PanelObject panel1, PanelObject panel2)
    {
        int idx1 = panelList.Find(x => x == panel1).realIndex;
        int idx2 = panelList.Find(x => x == panel2).realIndex;

        // 位置の交換
        panel1.gameObject.transform.localPosition = positionList[idx2];
        panel2.gameObject.transform.localPosition = positionList[idx1];
        panel1.ChangePanelPosition(idx2);
        panel1.ChangePanelPosition(idx1);

        // 隣接するパネル設定
        //for (int i = 0; i < panelList.Count; i++)
        //{
        //    if (panelList[i] == panel1)
        //    {

        //    }

        //    int col = i % GameDefine.MaxCol;
        //    int row = i / GameDefine.MaxCol;
        //    var left = GetPanelObject(col - 1, row);
        //    var right = GetPanelObject(col + 1, row);
        //    var up = GetPanelObject(col, row - 1);
        //    var down = GetPanelObject(col, row + 1);
        //    panelList[i].SetLink(left, right, up, down);
        //}
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
Debug.Log($"** col:{col} row:{row} range overflow");
            return null;
        }

        int index = row * GameDefine.MaxCol + col;
Debug.Log($"** col:{col} row:{row} index:{index}");
        return panelList[index];
    }
}
