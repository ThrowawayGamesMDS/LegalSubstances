using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deadWongleDestroyer : MonoBehaviour
{


    private void Update()
    {
        transform.Translate(Vector3.down * 0.3f * Time.deltaTime);
        if(transform.position.y <= -4)
        {
            Destroy(gameObject);
        }
    }
}
