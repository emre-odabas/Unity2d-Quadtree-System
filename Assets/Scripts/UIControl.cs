using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIControl : MonoBehaviour {
    public void startGame () {
        StartCoroutine ("Coroutine", true);
    }
    public void QuitGame () {
        StartCoroutine ("Coroutine", false);
    }

    IEnumerator Coroutine (bool isStart) {
        gameObject.GetComponent<Animation> ().Play ();

        yield return new WaitForSeconds (0.5f);

        if (isStart)
            SceneManager.LoadScene ("Quadtree");
        else
        Debug.Log("Quit");
            Application.Quit ();

    }

}