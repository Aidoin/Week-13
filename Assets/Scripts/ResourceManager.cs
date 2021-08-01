using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public struct Resources {

    [SerializeField] public int Golds;

    public static Resources operator +(Resources a, Resources b) {
        return new Resources(a.Golds + b.Golds);
    }
    public static Resources operator -(Resources a, Resources b) {
        return new Resources(a.Golds - b.Golds);
    }
    public static Resources operator *(Resources a, Resources b) {
        return new Resources(a.Golds * b.Golds);
    }
    public static Resources operator *(int a, Resources b) {
        return new Resources(a * b.Golds);
    }
    public static Resources operator *(Resources a, int b) {
        return new Resources(a.Golds * b);
    }
    public static Resources operator /(Resources a, Resources b) {
        return new Resources(a.Golds / b.Golds);
    }
    public static Resources operator /(Resources a, int b) {
        return new Resources(a.Golds / b);
    }
    public static bool operator ==(Resources lhs, Resources rhs) {
        return lhs.Golds == rhs.Golds;
            
    }
    public static bool operator !=(Resources lhs, Resources rhs) {
        return lhs.Golds != rhs.Golds;
    }

    public static bool LessThanZero(Resources a) {
        if(a.Golds < 0) {
            return true;
        } else {
            return false;
        }
    }
    public static bool GreaterThanZero(Resources a) {
        if (a.Golds > 0) {
            return true;
        } else {
            return false;
        }
    }
    public static bool AreValuesPositive(Resources a) {
        if (a.Golds < 0) {
            return false;
        } else {
            return true;
        }
    }
    
    public override string ToString() {
        return "(golds " + Golds + ")";
    }
    public static string ShowNegativeNumbers(Resources a) {
        string text = null;

        if(a.Golds < 0) {
            text += -a.Golds + " Golds";
        }
        return text;
    }

    public Resources(int Gold) {
        Golds = Gold;
    }
}


public class ResourceManager : MonoBehaviour
{
    [SerializeField] private Resources StartResource;
    [SerializeField] private Text textGol;
    [SerializeField] private Text textWood;

    private Resources resources; public Resources Resources => resources;
    private bool blocked = false; public bool Blocked => blocked;


    private void Start() {
        resources = StartResource;
        UppdateResources();
    }


    public void AddResources(Resources adding) {
        if (blocked) return;

        resources += adding;
        UppdateResources();
    }

    public void ReduceResources(Resources reducing) {
        if (blocked) return;

        resources -= reducing;
        UppdateResources();
    }

    public void UppdateResources() {
        textGol.text = resources.Golds.ToString();
        //textWood.text = resources.Woods.ToString();
    }

    public bool Buy(Resources price) {
        if (blocked) return false;

        if (Resources.LessThanZero(resources - price)) {
            return false;
        } else {
            resources = resources - price;
            UppdateResources();
            return true;
        }
    }

    public void Block() {
        blocked = true;
    }
    public void Unblock() {
        blocked = false;
    }
}
