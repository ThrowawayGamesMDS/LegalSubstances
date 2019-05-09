using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private void Update()
    {
        transform.up = gameObject.GetComponent<Rigidbody>().velocity;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<PlayerBuilding>() != null)
        {
            collision.gameObject.SendMessage("TakeDamage", 5);
            Destroy(gameObject);
        }
    }
}
