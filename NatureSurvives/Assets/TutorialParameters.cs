using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialParameters : MonoBehaviour
{
    public enum TutorialState
    {
        E_ONE = 1,
        E_TWO = 2,
        E_THIRD = 3
    }
    public TutorialState m_eTutorialLevel;
    // Start is called before the first frame update
    void Start()
    {
        m_eTutorialLevel = TutorialState.E_ONE;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
