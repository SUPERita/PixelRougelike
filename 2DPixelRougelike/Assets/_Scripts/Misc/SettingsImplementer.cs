using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SettingsImplementer : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    // Start is called before the first frame update
    void Start()
    {
        //makes it draw top to bottom and switch bathc whenever it switches material
        //basicly keep it as it ;
        GraphicsSettings.transparencySortMode = TransparencySortMode.CustomAxis;
        GraphicsSettings.transparencySortAxis = new Vector3(0.0f, 0.0f, 1.0f);

        //start compile
        PassiveUpgradesManager.Instance.CompileStatsFromChildren();//compile children to a list and push to passiveUpgrades
        //ERROR: if got here after an error than know that i need a PassiveUpgradesManager withs its children in every single scene for the passives to load proparly

        playerStats.BasePlayerStats_RequestStatsCompile();

        Time.timeScale = 1;
    }

    private void OnDisable()
    {
        if(ResourceSystem.Instance != null)
        {
            ResourceSystem.Instance.ResetResource(ResourceType.Gold);
        }
    }

  
}
