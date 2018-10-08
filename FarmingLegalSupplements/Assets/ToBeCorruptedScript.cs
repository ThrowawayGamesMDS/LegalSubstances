using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToBeCorruptedScript : MonoBehaviour {
    public List<GameObject> tobeCorrupted;
    // Use this for initialization
    void Start () {
        Invoke("startmycourotine", 3);
	}
	
	void startmycourotine()
    {
        StartCoroutine(checkChildren());
    }

    IEnumerator checkChildren()
    {
        while(true)
        {
            if (tobeCorrupted.Count > 0)
            {
                for (int i = 0; i < tobeCorrupted.Count; i++)
                {
                    tobeCorrupted[i].GetComponent<GridObject>().CheckCorrupt();
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }
    }
}
