using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barack : Building
{
    [Header("Barack")]

    [SerializeField] private Transform spawn;
    [SerializeField] private Transform positionSendingTheCreatedUnit;
    [SerializeField] private float positionStayRadiusCreatedUnit = 1;
    [SerializeField] private float timeActiveShield;
    [SerializeField] private float timeShieldCooldown;
    [SerializeField] private GameObject effectShield;
    [SerializeField] private AudioSource audioShield;

    private ResourceManager resourceManager;
    private MenuBarack menuBarack;
    private AdText adText;
    private float timerActiveShield; public float TimerActiveShield => timerActiveShield;
    private bool shieldIsActive; public bool ShieldIsActive => shieldIsActive;
    private float timerShieldCooldown; public float TimerShieldCooldown => timerShieldCooldown;
    private bool shieldIsCooldown; public bool ShieldIsCooldown => shieldIsCooldown;


    protected override void Awake() {
        base.Awake();
        menuBuilding = menuBarack = FindObjectOfType<MenuBarack>();
        adText = FindObjectOfType<AdText>();
    }

    protected override void Start() {
        base.Start();
        resourceManager = FindObjectOfType<ResourceManager>();
    }

    private void Update() {
        if (shieldIsActive) {
            timerActiveShield -= Time.deltaTime;
            if (timerActiveShield < 0) {
                ShieldDisabled();
            }
        }

        if (shieldIsCooldown) {
            timerShieldCooldown -= Time.deltaTime;
            if (timerShieldCooldown < 0) {
                shieldIsCooldown = false;
            }
        }
    }

    public override void Select() {
        base.Select();
        menuBarack.SetBuildingInMenu(this);
    }


    public void BuyUnit(GameObject unit) {
        Resources prise = unit.GetComponent<Unit>().Price;
        if (resourceManager.Buy(prise)) {
            CreateUnit(unit);
        } else {
            adText.Show("Не хватает " + Resources.ShowNegativeNumbers(resourceManager.Resources - prise), 1, 0.2f, 0.2f);
        }
    }

    public void CreateUnit(GameObject unit) {
        Unit currentUnit = Instantiate(unit, spawn.position, spawn.rotation).GetComponent<Unit>();
        Vector3 unitPosition = positionSendingTheCreatedUnit.position + new Vector3(Random.Range(-positionStayRadiusCreatedUnit, positionStayRadiusCreatedUnit), 0, Random.Range(-positionStayRadiusCreatedUnit, positionStayRadiusCreatedUnit));
        currentUnit.WhenClickOnGround(unitPosition);
    }


    public override void takeDamage(int damageValue) {
        if (shieldIsActive) return;

        base.takeDamage(damageValue);
    }


    public void ShieldEnabled() {
        effectShield.SetActive(true);
        audioShield.Play();
        timerActiveShield = timeActiveShield;
        shieldIsActive = true;
    }

    public void ShieldDisabled() {
        effectShield.SetActive(false);
        audioShield.Stop();
        timerActiveShield = timeActiveShield;
        shieldIsActive = false;
        shieldIsCooldown = true;
        timerShieldCooldown = timeShieldCooldown;
    }
}
