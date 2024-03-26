using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    public GameObject retryButton; // Inspector에서 할당
    public GameObject[] titles;
    
    public void Lose()
    {
        titles[0].SetActive(true);
    }
    
    public void Win()
    {
        titles[1].SetActive(true);
    }

}
