using System;
using UnityEngine;

public class FrameButtonObject : MonoBehaviour
{
    private Action<FrameButtonObject> onClick;

    /// <summary>リストでのインデックス</summary>
    public int index { get; private set; }

    public void Initialize(Action<FrameButtonObject> onClick, int index)
    {
        this.onClick = onClick;
        this.index = index;
    }

    public void OnClick()
    {
        onClick.Invoke(this);
    }
}
