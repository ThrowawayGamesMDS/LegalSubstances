using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEAttack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5);
        int i = 0;
        while (i < hitColliders.Length)
        {
            hitColliders[i].SendMessage("TakeDamage", 15);
            i++;
        }
    }
    
}
