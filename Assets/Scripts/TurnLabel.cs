using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnLabel : MonoBehaviour
{
    [SerializeField]
    Text turnLabel;

    [SerializeField]
    float timeBeforeFade = 3f;
    float counter = 0f;

    [SerializeField]
    float fadingSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if(counter >= timeBeforeFade)
        {
            turnLabel.color = new Color(turnLabel.color.r, turnLabel.color.g, turnLabel.color.b, turnLabel.color.a - (Time.deltaTime * fadingSpeed));
            if(turnLabel.color.a <= 0)
            {
                gameObject.SetActive(false);
            }
        }

    }

    void OnEnable()
    {
        turnLabel.color = GameManager.instance.activePlayer.assignedColor;
        turnLabel.text = "Turn " + GameManager.instance.globalTurnIndex;
        counter = 0f;
    }
}
