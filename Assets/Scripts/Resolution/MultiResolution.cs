using UnityEngine;
using System.Collections;

public class MultiResolution : MonoBehaviour
{
    private Transform m_tranform;               // tranform need scale
    private static float BASE_WIDTH = 480f;
    private static float BASE_HEIGHT = 800f;
    private float baseRatio;
    private float percentScale;
    void Start()
    {
        m_tranform = transform;
        setScale();
    }


    /// <summary>
    /// scale tranform by width and high of scene
    /// </summary>
    void setScale()
    {
        #if UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8
            baseRatio = (float)BASE_WIDTH / BASE_HEIGHT * Screen.height;
            percentScale = Screen.width / baseRatio;
            m_tranform.localScale = new Vector3(m_tranform.localScale.x * percentScale, m_tranform.localScale.y, 1);
        #endif
    }
}
