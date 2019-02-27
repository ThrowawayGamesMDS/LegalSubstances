using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogCubeHider : MonoBehaviour
{
    public GameObject m_fogGameObject;
    public float m_radius;
    //public float m_maxDistance = 100;
    //public bool m_doShow = true;
    public bool m_doScale;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Transform child in m_fogGameObject.transform)
        {
            //we need to optimize this
            //1.  we need to skip the cubes that have already been de-activated
            //2.  we need don't need to do this method anymore when the entire map is revealed

            float distance = Vector3.Distance(child.position, transform.position);

            if(distance < m_radius)
            {
                ////if false, we are showing objects
                //if(!m_doShow)
                //{
                //    //if the fog cube is not active, activate it
                //    if(!child.gameObject.activeInHierarchy)
                //    {
                //        child.gameObject.SetActive(true);  
                //    }
                //}
                //else //if true, we are hiding the objects
                //{
                    //hide the child, if it is active and we are not scaling
                    if(child.gameObject.activeInHierarchy && !m_doScale)
                    {
                        child.gameObject.SetActive(false);
                    }
                    //if the child is active and we are scaling, turn off the scaling
                    else if (child.gameObject.activeInHierarchy && m_doScale)
                    {
                        print("toggle scale");
                        child.GetComponent<FogScaler>().ToggleScale(false);
                    }
                //}
            }
            // if the distance is greater than the desired distance
            //else
            //{
            //    //if not showing, we are hiding the objects
            //    if (!m_doShow)
            //    {
            //        //if the child is active, then hide it
            //        if(child.gameObject.activeInHierarchy)
            //        {
            //            child.gameObject.SetActive(false);
            //        }
            //        else
            //        {
            //            ////if we are within the max distance
            //            //if(distance < m_maxDistance)
            //            //{
            //            //    //if the child is not active and we are not scaling, then show it
            //            //    if(!child.gameObject.activeInHierarchy && !m_doScale)
            //            //    {
            //            //        child.gameObject.SetActive(true);
            //            //    }
            //            //    //if the child is KeyNotFoundException active and we are scaling, then show it
            //            //    else if(!child.gameObject.activeInHierarchy && m_doScale)
            //            //    {
            //            //        child.gameObject.SetActive(true);
            //            //        //child.GetComponent<Scaler>().ToggleScale(true);
            //            //    }
            //            //}
            //            //else
            //            //{
            //            //    child.gameObject.SetActive(false);
            //            //}
            //        }
            //    }
            //}
        }
    }
}
