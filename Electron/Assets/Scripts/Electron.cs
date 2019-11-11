using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Electron
{
    public GameObject electron;
    public GameObject[] positrons;
    private float temporaryElectronY;

    public void AssignGameObjects()
    {
        electron = GameObject.FindGameObjectWithTag("Player");
        positrons = GameObject.FindGameObjectsWithTag("Positron");
    }

    public bool ElectronFalling()
    {
        if (electron != null)
        {
            if (electron.transform.position.y < temporaryElectronY)
            {
                temporaryElectronY = electron.transform.position.y;
                return true;
            }
            else
            {
                temporaryElectronY = electron.transform.position.y;
                return false;
            }
        }
        else
            return false;
    }

    public bool PositronFalling(int certainPositron, float temporaryPositronY)
    {
        if (positrons[certainPositron] != null)
        {
            if (positrons[certainPositron].transform.position.y < temporaryPositronY)
            {
                temporaryPositronY = positrons[certainPositron].transform.position.y;
                return true;
            }
            else
            {
                temporaryPositronY = positrons[certainPositron].transform.position.y;
                return false;
            }
        }
        else
            return false;
    }
}
