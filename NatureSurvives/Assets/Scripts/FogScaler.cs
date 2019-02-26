using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogScaler : MonoBehaviour
{
    public float m_scaleSpeed = 5;
    public float maxSize = 5;
    private bool m_desiredState;
    private bool m_doScaling = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_doScaling)
        {
            if (!m_desiredState)
            {
                Vector3 desiredSize = new Vector3(transform.localScale.x - Time.deltaTime * m_scaleSpeed, transform.localScale.y - Time.deltaTime * m_scaleSpeed, transform.localScale.z - Time.deltaTime * m_scaleSpeed);
                transform.localScale = desiredSize;

                print("smaller");

                if (transform.localScale.x <= 0.1)
                {
                    gameObject.SetActive(false);
                    m_doScaling = false;
                }
            }
            else
            {
                Vector3 desiredSize = new Vector3(transform.localScale.x + Time.deltaTime * m_scaleSpeed, transform.localScale.y + Time.deltaTime * m_scaleSpeed, transform.localScale.z + Time.deltaTime * m_scaleSpeed);
                transform.localScale = desiredSize;

                if (transform.localScale.x >= maxSize)
                {
                    transform.localScale = new Vector3(maxSize, maxSize, maxSize);
                    gameObject.SetActive(true);
                    m_doScaling = false;
                }
            }
        }
    }

    public void ToggleScale(bool setType)
    {
        m_desiredState = setType;
        m_doScaling = true;
    }

}

