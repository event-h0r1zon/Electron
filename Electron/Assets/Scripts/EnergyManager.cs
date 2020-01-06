using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EnergyManager : MonoBehaviour
{
    public Slider slider;
    public GameObject Pause;
    private float currentTime = 0;
    public ParticleSystem energyParticle;
    public ParticleSystem superpositionParticle;
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
    public GameObject countdownText;
    private TextMeshProUGUI textUI;

    private void Awake()
    {
        currentTime = 3;
        electron.AssignGameObjects();
        textUI = countdownText.GetComponent<TextMeshProUGUI>();
        electronProperties = electron.electron.GetComponent<PlayerMovement>();
        slider.value = slider.maxValue;
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
            if (electron.electron != null)
                electron.electron.GetComponent<Rigidbody2D>().gravityScale = 5;
            for (int i = 0; i < electron.positrons.Length; i++)
            {
                if (electron.positrons[i] != null)
                    electron.positrons[i].GetComponent<Rigidbody2D>().gravityScale = 5;
            }
            if (electron.electron != null && !pauseScript.slowingDown)
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

