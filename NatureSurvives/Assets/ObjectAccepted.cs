using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAccepted : MonoBehaviour
{
    private void Awake()
    {
       int _iLayerMask = LayerMask.GetMask("Ground");
        RaycastHit _rh;
        Ray ray = new Ray();
        ray.direction = Vector3.down;
        ray.origin = gameObject.transform.position;

        if (!Physics.Raycast(ray.origin, ray.direction * 5.0f, out _rh, 25.0f, _iLayerMask))
        {
            Destroy(gameObject);
        }
      /*  else
        {
            Destroy(gameObject);
        }*/
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
