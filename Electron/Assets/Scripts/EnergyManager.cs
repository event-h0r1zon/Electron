using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    public Slider slider;
    public GameObject Pause;
    private PauseMenu pauseScript;
    public float energyBonus;
    public float energyDecreaseDuration;
    private bool electronFalling;
    public Electron electron;
    private PlayerMovement electronProperties;
    public GameObject proton;
    private ProtonOrbit protonScript;
    public GameObject[] superpositions;
    public List<Superpositioner> superpositioners;
    public GameObject[] antiprotons;
    public List<AntiProton> antiprotonScripts;
    public GameObject[] quantumEnergys;
    public List<EnergyAdder> energyAdders;
    public float jumpEnergy = 0.15f;
    public float fallingBonus = 0.1f;

    private void Awake()
    {
        electron.AssignGameObjects();
        electronProperties = electron.electron.GetComponent<PlayerMovement>();
        slider.value = 1f;
        pauseScript = Pause.GetComponent<PauseMenu>();
        proton = GameObject.FindGameObjectWithTag("Proton");
        antiprotons = GameObject.FindGameObjectsWithTag("Antiproton");
        superpositions = GameObject.FindGameObjectsWithTag("Superpositioner");
        quantumEnergys = GameObject.FindGameObjectsWithTag("Bonus");
        protonScript = proton.GetComponent<ProtonOrbit>();
        for (int i = 0; i < superpositions.Length; i++)
            superpositioners.Add(superpositions[i].GetComponent<Superpositioner>());
        for (int i = 0; i < quantumEnergys.Length; i++)
            energyAdders.Add(quantumEnergys[i].GetComponent<EnergyAdder>());
        for (int i = 0; i < antiprotons.Length; i++)
            antiprotonScripts.Add(antiprotons[i].GetComponent<AntiProton>());
    }

    private void Update()
    {
        if (electron.electron != null && !pauseScript.slowingDown) {

            electronFalling = electron.ElectronFalling();

            if(slider.value > 0f)


            for (int i = 0; i < antiprotons.Length; i++)
            {
                if (antiprotonScripts[i].lowerEnergy)
                    StartCoroutine(LowerEnergy(i));
            }
            for (int i = 0; i < quantumEnergys.Length; i++)
            {
                if (energyAdders[i].booster)
                    StartCoroutine(AddEnergy(i));
            }
            for (int i = 0; i < superpositions.Length; i++)
            {
                if (superpositioners[i].setVelocity)
                    StartCoroutine(SuperpositionEnergy(i));
            }

            if (electronProperties.jumped)
                StartCoroutine(JumpDecrease());

            if (electronFalling)
                slider.value += fallingBonus / 10f;

            if (protonScript.electronInOrbit)
                slider.value += fallingBonus / 10f;

            if (slider.value <= 0f)
                Destroy(electron.electron);

        }

        else if (electron.electron == null)
            slider.value -= fallingBonus / 10f;
    }

    IEnumerator JumpDecrease()
    {
        slider.value -= jumpEnergy / 7;
        yield return new WaitForSeconds(0.15f);
        electronProperties.jumped = false;
    }

    IEnumerator LowerEnergy(int currentScript)
    {
        slider.value -= 0.08f;
        yield return new WaitForSeconds(energyDecreaseDuration);
        antiprotonScripts[currentScript].lowerEnergy = false;
    }

    IEnumerator AddEnergy(int currentScript)
    {
        slider.value += energyBonus;
        Destroy(quantumEnergys[currentScript]);
        yield return new WaitForSeconds(1f);
        energyAdders[currentScript].booster = false;
    }
    IEnumerator SuperpositionEnergy(int currentScript)
    {
        slider.value -= 0.005f;
        yield return new WaitForSeconds(electronProperties.superpositionDuration);
        superpositioners[currentScript].setVelocity = false;
    }
}

