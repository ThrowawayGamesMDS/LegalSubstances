using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyself : MonoBehaviour {
    public float timer;
	// Use this for initialization
	void Start () {
        Invoke("DestroyMe", timer);
	}
    void DestroyMe()
    {
        Destroy(gameObject);
    }
}
