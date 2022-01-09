using UnityEngine;
using UnityEngine.SceneManagement;

public class Test1Cube : MonoBehaviour
{
    float rotSpeed = 2f;

    void Update()
    {
        transform.Rotate(new Vector3(0, 90, 0) * Time.deltaTime * rotSpeed, Space.Self);
    }
}
