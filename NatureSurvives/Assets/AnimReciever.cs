using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimReciever : MonoBehaviour
{
    void ShootProjectile()
    {
        transform.parent.SendMessage("ShootProjectile");
    }
}
