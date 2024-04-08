using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolTime : MonoBehaviour
{
    public Image CoolImg; // 게임 오브젝트 대신 Image 컴포넌트 직접 참조
    private float coolTime;
    private bool isUseSkill = false;

    void Update()
    {
        if (isUseSkill)
        {
            coolTime -= Time.deltaTime;
            CoolImg.fillAmount = coolTime / CoolImg.fillAmount;

            if (coolTime <= 0)
            {
                isUseSkill = false;
                CoolImg.gameObject.SetActive(false);
            }
        }
    }

    public void StartCoolTime(float cool)
    {
        CoolImg.fillAmount = 0f; // 쿨타임 시작 시 완전히 채워진 상태로 설정
        CoolImg.gameObject.SetActive(true);
        isUseSkill = true;
        coolTime = cool;
       
    }
}

