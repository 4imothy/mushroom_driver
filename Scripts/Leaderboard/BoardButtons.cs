using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoardButtons : MonoBehaviour
{
    public void leaveToMain()
    {
        GetComponent<Animator>().Play("LeaveScoreboard");
        StartCoroutine(LoadSceneAfterAnim(0));
    }
    private IEnumerator LoadSceneAfterAnim(int index)
    {
        yield return new WaitForSeconds(.75f);
        SceneManager.LoadScene(index);
    }
}
