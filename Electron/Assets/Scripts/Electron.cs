using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Electron
{
    //Public Monobehaviours
    public GameObject electron;
    public GameObject[] positrons;

    //Private Variables
    private float temporaryElectronY;

    public void AssignGameObjects()
    {
        electron = GameObject.FindGameObjectWithTag("Player");
        int arraySize = GameObject.FindGameObjectsWithTag("Positron").Length;
        positrons = new GameObject[arraySize];
        foreach (GameObject positron in GameObject.FindGameObjectsWithTag("Positron"))
        {
            if (positron.name.Length >= 10)
                positrons[positron.name[10] - '0'] = positron;
            else
                positrons[0] = positron;
        }
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
