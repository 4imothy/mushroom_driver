using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Camera))]
public class ResetOrigin : MonoBehaviour
{
    public float threshold;
    public GroundManager layoutGenerator;
    private Vector3 cameraPosition;

    void LateUpdate()
    {
        cameraPosition = gameObject.transform.position;
        cameraPosition.y = 0f;

        if (cameraPosition.magnitude > threshold)
        {
            Reposition();
        }

    }

    public void Reposition()
    {
        for (int z = 0; z < SceneManager.sceneCount; z++)
        {
            foreach (GameObject g in SceneManager.GetSceneAt(z).GetRootGameObjects())
            {
                g.transform.position -= cameraPosition;
            }
        }
    }
}

