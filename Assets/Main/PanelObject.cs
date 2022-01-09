using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelObject : MonoBehaviour
{
    /// <summary>
    /// パネルの上下左右の隣接するパネルの参照
    /// </summary>
    public class PanelLink
    {
        public PanelObject left = null;
        public PanelObject right = null;
        public PanelObject up = null;
        public PanelObject down = null;

        public PanelLink(PanelObject left, PanelObject right, PanelObject up, PanelObject down)
        {
            this.left = left;
            this.right = right;
            this.up = up;
            this.down = down;
        }
    }

    public int startIndex { get; private set; }

    public int realIndex { get; set; }

    public bool IsVisible { get; private set; } = true;

    public PanelLink panelLink { get; private set; }

    public void Initialize(int startIndex)
    {
        this.startIndex = startIndex;
        this.realIndex = startIndex;
    }

    public void SetLink(PanelObject left, PanelObject right, PanelObject up, PanelObject down)
    {
        panelLink = new PanelLink(left, right, up, down);
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
        this.IsVisible = visible;
    }

    public void ChangePanelPosition(int nextIndex)
    {
        this.realIndex = nextIndex;
    }

    /// <summary>
    /// 隣接するパネルで非表示のパネルがあれば自分自身を返す
    /// </summary>
    /// <returns></returns>
    public PanelObject GetLinkHidePanelObject()
    {
        if (panelLink.left?.IsVisible == false) return this;
        if (panelLink.right?.IsVisible == false) return this;
        if (panelLink.up?.IsVisible == false) return this;
        if (panelLink.down?.IsVisible == false) return this;
        return null;
    }

    /// <summary>
    /// 隣接するパネルで非表示のパネルがあれば非表示のパネルを返す
    /// </summary>
    /// <returns></returns>
    public PanelObject GetHidePanelObjectFromLink()
    {
        if (panelLink.left?.IsVisible == false) return panelLink.left;
        if (panelLink.right?.IsVisible == false) return panelLink.right;
        if (panelLink.up?.IsVisible == false) return panelLink.up;
        if (panelLink.down?.IsVisible == false) return panelLink.down;
        return null;
    }
}
