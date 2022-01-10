using System;
using UnityEngine;
using UnityEngine.UI;

public class PanelObject : MonoBehaviour
{
    [SerializeField]
    private GameObject frameObject;

    [SerializeField]
    private Image image;

    [SerializeField]
    private CommonButton commonButton;

    public int startIndex { get; private set; }

    public int realIndex { get; set; }

    public bool IsVisible { get; private set; } = true;

    private Action<PanelObject> onClickCallback;

    public void Initialize(int startIndex, Action<PanelObject> onClickCallback)
    {
        this.startIndex = startIndex;
        this.realIndex = startIndex;
        
        this.onClickCallback = onClickCallback;

        LoadImage(startIndex + 1);

        commonButton.Initialize();
        commonButton.onClickButton = OnClick;
    }

    public void LoadImage(int index)
    {
        int stage = GameController.instance.nowStage;
        string fileName = string.Format("{0}_{1:D2}", stage, index);
        string path = $"img/{stage}/{fileName}";
        image.sprite = Resources.Load<Sprite>(path);
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
        this.IsVisible = visible;
    }

    public void SetEnabledButton(bool enabled)
    {
        commonButton.enabled = enabled;
    }

    public  void SetVisibleFrame(bool visible)
    {
        frameObject.SetActive(visible);
    }

    public void OnClick()
    {
        onClickCallback.Invoke(this);
    }
}
