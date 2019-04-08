using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour {
    public static int WhiteAmount;
    public static int WoodAmount;
    public static int CrystalAmount;

	// Use this for initialization
	void Start () {
        WhiteAmount    = 150;
        WoodAmount     = 200;
        CrystalAmount  = 200;
	}

    public void Update()
    {
        /*if (DisplayHandler.m_sDHControl.m_bButtonsLoaded)
        {
            if (HomeSpawning.m_sHomeSpawningControl.iCurrentWongleCount >= HomeSpawning.m_sHomeSpawningControl.iMaximumWongleCount && DisplayHandler.m_sDHControl.m_bDisplayingUnit)
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
