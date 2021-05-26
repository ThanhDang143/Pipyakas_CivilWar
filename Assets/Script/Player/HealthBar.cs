using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;

public class HealthBar : NetworkBehaviour
{
    public Slider healthSlider;
    public Image healthImg;

    private void Start()
    {
        if (IsLocalPlayer)
        {
            healthImg.color = Color.green;
        }
        else
        {
            healthImg.color = Color.red;
        }
    }

    public void SetMaxHealth(int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }
    
    public void SetHealth(int health)
    {
        healthSlider.value = health;
    }
}
