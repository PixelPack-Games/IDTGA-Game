using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BattleHUD : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    //public TextMeshProUGUI levelText;
    public Slider hpSlider;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI levelText;

    public void SetPlayerHUD (PlayerStats playerStats)
    {
        nameText.text = playerStats.Name.ToString();
        levelText.text = "LVL " + playerStats.player.getLevel();
        hpSlider.maxValue = playerStats.MaxHealth;
        hpSlider.value = playerStats.player.getCurrHealth();
        Debug.Log("Player HUD Set");
    }

    public void SetEnemyHUD(EnemyStats enemyStats)
    {
        nameText.text = enemyStats.Name.ToString();
        levelText.text = "LVL " + enemyStats.enemy.getLevel();
        hpSlider.maxValue = enemyStats.MaxHealth;
        hpSlider.value = enemyStats.enemy.getCurrHealth();
        Debug.Log("Enemy HUD Set");
    }

    public void subtractHP(int hp)
    {
        hpSlider.value = hp;
    }

    public void addHP(int hp)
    {
        hpSlider.value = hpSlider.value + hp;
    }

    public void SetHP(int hp)
    {
        
        if(hp <= 0)
        {
            hpSlider.value = 0;
        }
        else
        {
            hpSlider.value = hp;
        }
    
        
    }

    public void UpdatePlayerHPtext(PlayerStats playerStats)
    {
        if(playerStats.player.getCurrHealth() <= 0)
        {
            hpText.text = 0 + "/" + playerStats.MaxHealth;
        }
        else
        {
            hpText.text = playerStats.player.getCurrHealth() + "/" + playerStats.MaxHealth;
        }
        
    }

    public void UpdateEnemyHPtext(EnemyStats enemyStats)
    {
        

        if(enemyStats.enemy.getCurrHealth() <= 0)
        {
            hpText.text = 0 + "/" + enemyStats.MaxHealth;
        }
        else
        {
            hpText.text = enemyStats.enemy.getCurrHealth() + "/" + enemyStats.MaxHealth;
        }
    }
}