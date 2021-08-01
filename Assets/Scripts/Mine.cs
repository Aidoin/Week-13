using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Building
{
    [Header("Mine")]

    [SerializeField] private Resources bringsResources;
    [SerializeField] private float miningTime;
    [SerializeField] private float timeEnhancedMining = 10;
    [SerializeField] private AudioSource enhancedMiningAudio;
    [SerializeField] private float timeEnhancedCooldown = 10;
    [SerializeField] private GameObject effectEnhanced;

    private MenuMine menuMine;
    private ResourceManager resourceManager;
    private float timer;
    private float timerEnhanced; public float TimerEnhanced => timerEnhanced;
    private bool enhancedMiningIsActive = false; public bool EnhancedMiningIsActive => enhancedMiningIsActive;
    private float timerEnhancedCooldown; public float TimerEnhancedCooldown => timerEnhancedCooldown;
    private bool enhancedIsCooldown; public bool EnhancedIsCooldown => enhancedIsCooldown;


    protected override void Awake() {
        base.Awake();
        menuBuilding = menuMine = FindObjectOfType<MenuMine>();
        resourceManager = FindObjectOfType<ResourceManager>();
    }

    private void Update() {
        timer += Time.deltaTime;

        if (timer > miningTime) {
            timer = 0;
            resourceManager.AddResources(bringsResources);
        }

        if (enhancedMiningIsActive) {
            timerEnhanced -= Time.deltaTime;

            if (timerEnhanced < 0) {
                EnhancedMiningDisabled();
            }
        }

        if (enhancedIsCooldown) {
            timerEnhancedCooldown -= Time.deltaTime;
            if (timerEnhancedCooldown < 0) {
                enhancedIsCooldown = false;
            }
        }
    }


    public override void Select() {
        base.Select();
        menuMine.SetBuildingInMenu(this);
    }

    public void EnhancedMiningEnabled() {
        effectEnhanced.SetActive(true);
        timerEnhanced = timeEnhancedMining;
        timerEnhancedCooldown = timeEnhancedCooldown;
        enhancedMiningIsActive = true;
        miningTime = miningTime / 2;
        enhancedMiningAudio.pitch = Random.Range(2f, 2.4f);
        enhancedMiningAudio.Play();
    }

    public void EnhancedMiningDisabled() {
        effectEnhanced.SetActive(false);
        enhancedMiningIsActive = false;
        enhancedIsCooldown = true;
        miningTime = miningTime * 2;
        enhancedMiningAudio.Stop();
    }
}
