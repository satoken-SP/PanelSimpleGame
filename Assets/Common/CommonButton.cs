using System;
using UnityEngine.UI;

public class CommonButton : Button
{
    public Action onClickButton { get; set; }

    public void Initialize()
    {
        onClick.AddListener(Callback);
    }

    public void Callback()
    {
        onClickButton.Invoke();
    }
}
