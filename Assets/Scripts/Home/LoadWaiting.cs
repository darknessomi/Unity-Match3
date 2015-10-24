using UnityEngine;
using System.Collections;

public class LoadWaiting : MonoBehaviour
{

    public UnityEngine.UI.Image loadbar;    // Image loading fake

    /// <summary>
    /// fill image by second and go to Home scene
    /// </summary>
    /// <returns></returns>
    IEnumerator Start()
    {
        for (int i = 0; i < 120; i++)
        {
            loadbar.fillAmount += 1 / 120f;
            yield return new WaitForEndOfFrame();
        }
        Application.LoadLevel("HomeScene");
    }
}
