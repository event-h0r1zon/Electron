using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class EnergyManager : MonoBehaviour
{
    //Public Monobehaviours
    public Slider slider;
    public GameObject Pause;
    public ParticleSystem energyParticle;
    public ParticleSystem superpositionParticle;
    public Electron electron;
    public GameObject proton;
    public GameObject[] superpositions;
    public List<Superpositioner> superpositioners;
    public GameObject[] antiprotons;
    public List<AntiProton> antiprotonScripts;
    public GameObject[] quantumEnergys;
    public List<EnergyAdder> energyAdders;
    public GameObject countdownText;

    //Public Variables
    public float energyBonus;
    public float energyDecreaseDuration;
    public float jumpEnergy = 0.15f;
    public float fallingBonus = 0.1f;

    //Private Variables
    private float currentTime = 0;   
    private bool electronFalling;

    //Private Monobehaviours 
    private UImanager uiScript;
    private PlayerMovement electronProperties;
    private ProtonOrbit protonScript;
    private TextMeshProUGUI textUI;

    private void Awake()
    {
        currentTime = 3;
        electron.AssignGameObjects();
        textUI = countdownText.GetComponent<TextMeshProUGUI>();
        electronProperties = electron.electron.GetComponent<PlayerMovement>();
        slider.value = slider.maxValue;
        uiScript = Pause.GetComponent<UImanager>();
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
        if (currentTime > 0)
        {
            countdownText.SetActive(true);
            currentTime -= 1 * Time.deltaTime;
            textUI.text = currentTime.ToString("0");
            if (electron.electron != null)
                electron.electron.GetComponent<PlayerMovement>().countdown = true;
            for (int i = 0; i < electron.positrons.Length; i++)
            {
                if (electron.positrons[i] != null)
                    electron.positrons[i].GetComponent<Tracker>().countdown = true;
            }
        }
        else
        {
            countdownText.SetActive(false);
            if (electron.electron != null)
                electron.electron.GetComponent<PlayerMovement>().countdown = false;
            for (int i = 0; i < electron.positrons.Length; i++)
            {
                if (electron.positrons[i] != null)
                    electron.positrons[i].GetComponent<Tracker>().countdown = false;
            }

            if (electron.electron != null && !uiScript.slowingDown)
            {

                electronFalling = electron.ElectronFalling();

                for (int i = 0; i < antiprotons.Length; i++)
                {
                    if (antiprotonScripts[i].lowerEnergy)
                        StartCoroutine(LowerEnergy(i));
                }
                for (int i = 0; i < quantumEnergys.Length; i++)
                {
                    if (energyAdders[i].booster)
                    {
                        if (quantumEnergys[i] != null)
                            ParticleSystem.Instantiate(energyParticle, quantumEnergys[i].transform.position, Quaternion.identity);
                        StartCoroutine(AddEnergy(i));
                    }
                }
                for (int i = 0; i < superpositions.Length; i++)
                {
                    if (superpositioners[i].setVelocity)
                    {
                        if (superpositions[i] != null)
                            ParticleSystem.Instantiate(superpositionParticle, superpositions[i].transform.position, Quaternion.identity);
                        StartCoroutine(SuperpositionEnergy(i));
                    }
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
    }

    IEnumerator JumpDecrease()
    {
        slider.value -= jumpEnergy / 7;
        yield return new WaitForSeconds(0.15f);
        electronProperties.jumped = false;
    }

    IEnumerator LowerEnergy(int currentScript)
    {
        slider.value -= 0.04f;
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

