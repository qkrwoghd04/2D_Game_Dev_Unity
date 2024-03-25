using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType{Level, Kill, Time, Score, Health}
    public InfoType type;

    Text myText;
    Slider mySlider;

    void Awake() {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    void LateUpdate() {
        switch(type){
            case InfoType.Level:
                myText.text = string.Format("Level. {0:F0}",GameManager.instance.level);
                break;

            case InfoType.Kill:
                myText.text = string.Format("{0:F0}",GameManager.instance.kill);
                break;

            case InfoType.Time:
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                
                break;
            case InfoType.Score:

                break;
            case InfoType.Health:
                mySlider.value = GameManager.instance.health;
                break;
        }   
    }
}
