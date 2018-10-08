using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectDescription : MonoBehaviour
{
    private GameObject m_pgoSelectedObj;
    public List<string> m_lsPossibleTags;
    public Text m_tNameText;
    public Text m_tDescriptionText;
    public GameObject m_goTechTree;
    public GameObject m_goDescriptionBox;
    private GameObject m_goUtilize;

    private bool DetermineSelectedObject(RaycastHit _hit)
    {
        for (int i = 0; i < m_lsPossibleTags.Count; i++)
        {
            if (_hit.transform.tag == m_lsPossibleTags[i])
            {
                return true;
            }
        }
        return false;
    }
    RaycastHit GenerateRayCast(float _fDistanceOfRay)
    {
        RaycastHit _rh;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * _fDistanceOfRay, new Color(1f, 0.922f, 0.016f, 1f));
        if (Physics.Raycast(ray.origin, ray.direction * _fDistanceOfRay, out _rh, 250.0f))
        {
            if (DetermineSelectedObject(_rh))
            {
                m_pgoSelectedObj = _rh.transform.gameObject;
                return _rh;
            }
            else
            {
                return _rh;
            }
        }
        else
        {
                return _rh;
        }
    }

    // Use this for initialization
    void Start ()
    {
        m_pgoSelectedObj = null;
        m_goTechTree.SetActive(false);

        for (int i = 0; i < 4; i++)
        {
            if (i == 0)
            {
                m_lsPossibleTags.Add( "HomeBuilding");
            }
            else if (i == 1)
            {
                m_lsPossibleTags.Add("Wongle");
            }
            else if (i == 2)
            {
                m_lsPossibleTags.Add("Standing_shroom");
            }
            else 
            {
                m_lsPossibleTags.Add("Tech_Master");
            }
        }
    }

    public void OnClickExit()
    {
        m_goTechTree.SetActive(false);
        m_goDescriptionBox.SetActive(true);
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKeyUp(KeyCode.Mouse0))
        {
           RaycastHit _clickView = GenerateRayCast(9999.5f);
        }
        if (m_pgoSelectedObj != null)
        {
            switch (m_pgoSelectedObj.transform.tag)
            {
                case "HomeBuilding":
                    {
                        m_tNameText.text = "Name: Home Building";
                        m_tDescriptionText.text = "Description: This is your empire lel";
                        break;
                    }
                case "Wongle":
                    {
                        m_tNameText.text = "Name: Wongle Slave";
                        m_tDescriptionText.text = "Description: One of your empires many slaves lel";
                        break;
                    }
                case "Tech_Master":
                    {
                        m_tNameText.text = "Name: Tech Master Building";
                        m_tDescriptionText.text = "Description: Access your tech tree abilities here!!";
                        m_goTechTree.SetActive(true);
                        m_goDescriptionBox.SetActive(false);
                        break;
                    }
            }
            m_pgoSelectedObj = null;
        }
    }
}
