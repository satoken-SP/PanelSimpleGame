using UnityEngine;

public class TitleButton : MonoBehaviour
{
    public void ChangeScene()
    {
        GameController.instance.sceneChanger.ChangeScene(SceneType.MainScene);
    }
}
