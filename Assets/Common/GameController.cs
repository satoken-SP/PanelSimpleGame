using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;

    [SerializeField]
    public SceneChanger sceneChanger = null;

    /// <summary>現在プレイ中のステージ</summary>
    public int nowStage { get; set; } = 1;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            Setup();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Setup()
    {
        sceneChanger.ChangeScene(SceneType.TitleScene);
    }
}
