using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG;

public class Test_UI : MonoBehaviour
{
    public GameObject hideSkillBtn;
    public Image hideSkillImg;
    private float coolTime = 5;
    private bool isHideSkill = false;
    private float getTime = 5;


    // Start is called before the first frame update
    void Start()
    {
        hideSkillBtn.SetActive(false);

    }

    void Update()
    {
        print(isHideSkill);
        if (isHideSkill)
        {
            StartCoroutine("SkillTimeChk");
        }
    }

    public void HideSkillSetting()
    {
        hideSkillBtn.SetActive(true);
        isHideSkill = true;
    }

    private void HideSkillCheck()
    {

    }
    
    IEnumerator SkillTimeChk()
    {

        yield return null;

        if (getTime > 0)
        {
            getTime -= Time.deltaTime;

            if(getTime < 0)
            {
                getTime = 0;
                isHideSkill = false;
                hideSkillBtn.SetActive(false);
                getTime = coolTime;

            }
            float time = getTime / coolTime;
            hideSkillImg.fillAmount = time;

        }
        
    }


}
