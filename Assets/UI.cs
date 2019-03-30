using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI instance;

    [SerializeField]
    Text fundsDisplay;
    [SerializeField]
    Slider powerDisplay;

    [SerializeField]
    Image tileImage;
    [SerializeField]
    Text tileName;

    [SerializeField]
    Image[] defenseShields;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        
    } 

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GameManager.instance.activePlayer.AddPower(30);
        }
    }

    public void UpdateFundsDisplay()
    {
        fundsDisplay.text = GameManager.instance.activePlayer.GetFunds().ToString();
    }

    public void UpdatePowerDisplay()
    {
        powerDisplay.maxValue = GameManager.instance.activePlayer.GetMaxPower();
        powerDisplay.value = GameManager.instance.activePlayer.GetPower();
    }

    public void UpdateTileInfo(ClickableTile tile)
    {
        tileName.text = tile.typeOfTerrain.visibleName;

        for(int x = 0; x < defenseShields.Length; x++)
        {
            if(x < tile.typeOfTerrain.defensiveStat)
            {
                defenseShields[x].color = Color.white;
            }
            else
            {
                defenseShields[x].color = Color.gray;
            }
        }
    }
}
