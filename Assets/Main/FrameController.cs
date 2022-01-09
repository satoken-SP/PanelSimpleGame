using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FrameController : MonoBehaviour
{
    [SerializeField]
    private List<FrameButtonObject> frameList = new List<FrameButtonObject>();

    private Action<int> onClick;

    public void Initialize(Action<int> onClick)
    {
        this.onClick = onClick;

        // �eFrameButtonObject�̏�����
        for (int i = 0; i < frameList.Count; i++)
        {
            frameList[i].Initialize(OnClick, i);
        }
    }

    public FrameButtonObject GetFrameButtonObjectFromIndex(int index)
    {
        if (index < 0 || frameList.Count <= index)
        {
            UnityEngine.Assertions.Assert.IsTrue(true, "GetFrameButtonObjectFromIndex�֐��Ŏ擾�Ɏ��s���܂����B");
            return null;
        }
        return frameList[index];
    }

    private void OnClick(FrameButtonObject panelButtonObject)
    {
        Debug.Log($"FrameController OnClick : {frameList[panelButtonObject.index].name}");

        onClick.Invoke(panelButtonObject.index);
    }
}
