using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(hud);
            Destroy(menu);
            return;
        }


        instance = this;
        SceneManager.sceneLoaded += LoadState;
        SceneManager.sceneLoaded += OnSceneLoaded;
      
    }

    // Ressources

    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;

    // References
    public Player player;
    public Weapon weapon;
    public FloatingTextManager floatingTextManager;
    public RectTransform hitPointBar;
    public Animator deathMenuAnim;
    public GameObject hud;
    public GameObject menu;


    // weapon

    // logic
    public int pesos;
    public int experience;

    // Hitpoint bar
    public void OnHitPointChange()
    {
        float ratio = (float)player.hitPoint / (float)player.maxHitPoint;
        hitPointBar.localScale = new Vector3(1, ratio, 1);
    }

    // Floating text
    public void ShowText(string msg, int fonSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fonSize, color, position, motion, duration);
    }

    // Upgrade weapon
    public bool TryUpgradeWeapon()
    {
        // Is the weapon max level?
        if (weaponPrices.Count <= weapon.weaponLevel)
        {
            return false;
        }
        if (pesos >= weaponPrices[weapon.weaponLevel])
        {
            pesos -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;
        }
        return false;
    }
   

    // Experience System
    public int GetCurrentLevel()
    {
        int r = 0;
        int add = 0;

        while (experience >= add)
        {
            add += xpTable[r];
            r++;

            if (r == xpTable.Count) // Max Level
            {
                return r;
            }
        }
        return r;
    }
    public int GetXptoLevel(int level)
    {
        int r = 0;
        int xp = 0;
        while (r < level)
        {
            xp += xpTable[r];
            r++;
            
        }
        return xp;
    }
    public void GrantXp(int xp)
    {
        int currLevel = GetCurrentLevel();
        experience = xp;
        if (currLevel < GetCurrentLevel())
        {
            OnlevelUp();
        }
    }

    public void OnlevelUp()
    {
        Debug.Log("Level Up");
        player.OnLvlUp();
        OnHitPointChange();
    }


    // Save State
    public void SaveState()
    {

        Debug.Log("SaveState");
        string s = "";
        s = s + "0" + "|";
        s = s + pesos.ToString() + "|";
        s = s + experience.ToString() + "|";
        s = s + weapon.weaponLevel.ToString();

        PlayerPrefs.SetString("SaveState", s);

    }

    public void LoadState(Scene s, LoadSceneMode load)
    {
        SceneManager.sceneLoaded -= LoadState;

        if (!PlayerPrefs.HasKey("SaveState"))
        {
            return;
        }

        Debug.Log("LoadState");
        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        // Change player skin
        pesos = int.Parse(data[1]);

        // Experience
        experience = int.Parse(data[2]);
        if (GetCurrentLevel() != 1)
        {
            player.SetLevel(GetCurrentLevel());
        }

        // Change the weapon level
        weapon.SetWeaponLevel(int.Parse(data[3]));

    }
    public void OnSceneLoaded(Scene s, LoadSceneMode load)
    {
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;

    }

    //Death Menu and Respawn
    public void Respawn()
    {
        deathMenuAnim.SetTrigger("hide");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        player.Respawn();
    }


}
