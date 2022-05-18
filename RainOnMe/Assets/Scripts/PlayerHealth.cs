using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public PlayerCombat playerCombat;
    public Image maxHealth;
    public Image currentHealth;


    // Start is called before the first frame update
    void Start()
    {
        maxHealth.fillAmount = playerCombat.currentHealth / 10;
        currentHealth.fillAmount = maxHealth.fillAmount;
        Debug.Log("Healthbar: max = " + maxHealth.fillAmount + " current = " + currentHealth.fillAmount);
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth.fillAmount = playerCombat.currentHealth / 10;
    }
}
