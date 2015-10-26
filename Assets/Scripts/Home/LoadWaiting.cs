using UnityEngine;
using System.Collections;

public class LoadWaiting : MonoBehaviour
{

    public UnityEngine.UI.Image loadbar;    // Image loading fake
	public static string LoadScene = "HomeScene";
	public AsyncOperation async;

    /// <summary>
    /// fill image by second and go to Home scene
    /// </summary>
    /// <returns></returns>
    IEnumerator Start()
    {
        for (int i = 0; i < 60; i++)
        {
            loadbar.fillAmount += 1 / 60f;
            yield return new WaitForEndOfFrame();
        }
//		Application.LoadLevel(LoadScene);
		async = Application.LoadLevelAsync(LoadScene);
		yield return async;
    }
}
