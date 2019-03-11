using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackScript : MonoBehaviour
{
    public GameObject smash;
    public Transform handpos;
    public EnemyController contr;
    public void AttackAOE()
    {
        contr.attackEnemy();
        Instantiate(smash, handpos.position, smash.transform.rotation);
    }
}
