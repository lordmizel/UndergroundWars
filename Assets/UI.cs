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
    Image tileImage;
    [SerializeField]
    Text tileName;

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
        
    }

    public void UpdateFundsDisplay()
    {
        fundsDisplay.text = GameManager.instance.activePlayer.GetFunds().ToString();
    }

    public void UpdateTileInfo(ClickableTile tile)
    {
        tileName.text = tile.typeOfTerrain.visibleName;
    }
}
