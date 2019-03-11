using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAccepted : MonoBehaviour
{
    private void Start()
    {
       int _iLayerMask = LayerMask.GetMask("Ground");
        RaycastHit _rh;
        Ray ray = new Ray();
        ray.direction = Vector3.down;
        ray.origin = gameObject.transform.position;

        if (Physics.Raycast(ray.origin, ray.direction * 5.0f, out _rh, 25.0f, _iLayerMask))
        {
            print("OBJ survives");
            //Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
}
