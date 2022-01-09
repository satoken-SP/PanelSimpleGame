using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public enum SceneType
{
    TitleScene,
    MainScene,
}

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    Canvas canvas;

    [SerializeField]
    Image transitionImage;

    public void ChangeScene(SceneType sceneType)
    {
        StartCoroutine(ChangeSceneTransition(sceneType));
    }

    public IEnumerator ChangeSceneTransition(SceneType sceneType)
    {
        yield return Transition(true);
        SceneManager.LoadScene(sceneType.ToString());
        yield return Transition(false);
    }

    private IEnumerator Transition(bool isStart)
    {
        float startAlpha, endAlpha;
        startAlpha = isStart ? 0f : 1f;
        endAlpha = isStart ? 1f : 0f;

        Color color = transitionImage.color;
        color.a = startAlpha;
        transitionImage.color = color;

        while (true)
        {
            yield return null;
            color.a = isStart ? color.a + 0.01f : color.a - 0.01f;
            transitionImage.color = color;
            if ((isStart && color.a >= endAlpha) || (!isStart && color.a <= endAlpha))
            {
                color.a = endAlpha;
                transitionImage.color = color;
                break;
            }
        }
    }
}
