using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretWongle : MonoBehaviour {
    public GameObject Target;
    private bool canAttack;
    public List<GameObject> Enemies;
    public Animator anim;
    public GameObject attackinstance;
    public GameObject attackEffect;
    public GameObject handPosition;
    // Use this for initialization
    void Start () {
        canAttack = true;
	}


    public void AttackCooldown()
    {
        canAttack = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Enemies.Insert(Enemies.Count, other.gameObject);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Enemies.Remove(other.gameObject);
        }
    }

    public GameObject FindClosestTag(string tag)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(tag);
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }


    IEnumerator PlayAnim()
    {
        yield return new WaitForSeconds(2.3f);
        print("anim");
        anim.Play("BasicSwingAttack");
        yield return new WaitForSeconds(0.5f);
        if (attackinstance != null)
        {
            attackinstance.transform.parent = null;
        }
    }


    // Update is called once per frame
    void Update () {
        if(Enemies.Count > 0)
        {
            for (int i = 0; i < Enemies.Count; i++)
            {
                if (Enemies[i] == null)
                {
                    Enemies.RemoveAt(i);
                }
            }
        }
        

        if(Enemies.Count > 0)
        {
            Target = Enemies[0];
        }


        if (Target != null)
        {
            if (canAttack)
            {
                attackinstance = Instantiate(attackEffect, handPosition.transform.position, handPosition.transform.rotation);
                attackinstance.transform.parent = handPosition.transform;
                attackinstance.GetComponent<LookAtTarget>().target = Target.transform.GetChild(1).gameObject;

                StartCoroutine(PlayAnim());

                canAttack = false;
                Invoke("AttackCooldown", 3f);
            }
            
        }
    }
}
