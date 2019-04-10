using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour {
    public int startingWood, startingCrystal, startingFood;
    public static int WhiteAmount;
    public static int WoodAmount;
    public static int CrystalAmount;

	// Use this for initialization
	void Start () {
        WhiteAmount    = startingFood;
        WoodAmount     = startingWood;
        CrystalAmount  = startingCrystal;
	}

    public void Update()
    {/*
        if (DisplayHandler.m_sDHControl.m_bButtonsLoaded && DisplayHandler.m_sDHControl.m_bDisplayingUnit)
        {
            if (HomeSpawning.m_sHomeSpawningControl.iCurrentWongleCount >= HomeSpawning.m_sHomeSpawningControl.iMaximumWongleCount)
            {
                if (DisplayHandler.m_sDHControl.CheckAlphaOfUI("Worker"))
                    DisplayHandler.m_sDHControl.AlterUIAlpha(true, "Worker");
                 if (DisplayHandler.m_sDHControl.CheckAlphaOfUI("Knight"))
                     DisplayHandler.m_sDHControl.AlterUIAlpha(true, "Knight");
                 if (DisplayHandler.m_sDHControl.CheckAlphaOfUI("Wizard"))
                     DisplayHandler.m_sDHControl.AlterUIAlpha(true, "Wizard");
            }
            else
            {
                if (!DisplayHandler.m_sDHControl.CheckAlphaOfUI("Worker"))
                    DisplayHandler.m_sDHControl.AlterUIAlpha(false, "Worker");
                if (!DisplayHandler.m_sDHControl.CheckAlphaOfUI("Knight"))
                    DisplayHandler.m_sDHControl.AlterUIAlpha(false, "Knight");
                if (!DisplayHandler.m_sDHControl.CheckAlphaOfUI("Wizard"))
                    DisplayHandler.m_sDHControl.AlterUIAlpha(false, "Wizard");
            }
        }*/
        
    }

}
