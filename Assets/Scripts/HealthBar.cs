
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public Image fill;

    public void SetMaxHealth(int health)    //Funktion der sætter spillerens max liv 
    {
        slider.maxValue = health;           //Sætter health sliderens maks værdi til at være lig med variablen health
        slider.value = health;              //Sætter health sliderens værdi til at være lig med variablen health

    }

    public void SetHealth(int health)       //Funktion der bruges til at ændre spillerens liv
    {
        slider.value = health;              //Sætter health sliderens værdi til at være lig med variablen health
    }

}
