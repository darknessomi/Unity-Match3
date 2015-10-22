using UnityEngine;
using System.Collections;

public class Resolution : MonoBehaviour {

    public  float BASE_WIDTH = 480f;
    public  float BASE_HEIGHT = 800f;

    private Transform m_tranform;
    private float baseRatio;
    private float percentScale;

    void Start()
    {
        m_tranform = transform;
        setScale();
    }
    void setScale()
    {
        baseRatio = (float)BASE_WIDTH / BASE_HEIGHT * Screen.height;
        percentScale = Screen.width / baseRatio;
        if (percentScale<1)
            m_tranform.localScale = new Vector3(m_tranform.localScale.x * percentScale, m_tranform.localScale.y * percentScale, 1);
    }
}
