using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Test_UI : MonoBehaviour
{
    public GameObject hideImg; // Editor에서 Hide Image 할당
    private float hideImgFill = 1;
    private bool isUseSkill = false;
    //private float coolTime = 5;
    private float starTime = 1;


    void Start()
    {
        hideImgFill = hideImg.GetComponent<Image>().fillAmount;
       // hideImg.SetActive(false); 
        //Tween cool = DOTween.To(() => starTime, x => hideImgFill.fillAmount = x, 0f, 10f);
        Tween cool = DOTween.To(() => hideImgFill, x => hideImgFill = x, 0f, 10f);
        cool.Play();

    }

    void Update()
    {
        print(starTime);
        if (isUseSkill)
        {
            SkillTimeChk();
        }
    }
    public void StartCoolTime()
    {
        hideImg.SetActive(true);
        isUseSkill = true;
    }

    public void SkillTimeChk()
    {
        Tween cool = DOTween.To(() => starTime, x => hideImgFill = x, 0f, 10f);
        hideImg.SetActive(false);
    }

    //public void StartCoolTime()
    //{
    //    hideImg.SetActive(true);
    //    isUseSkill = true;
    //}


    //IEnumerator SkillTimeChk()
    //{

    //    yield return null;

    //    if (starTime > 0)
    //    {
    //        starTime -= Time.deltaTime;

    //        if(starTime < 0)
    //        {
    //            starTime = 0;
    //            isUseSkill = false;
    //            hideImg.SetActive(false);
    //            starTime = coolTime;

    //        }
    //        float time = starTime / coolTime;
    //        hideImgFill = time;

    //    }

    //}


}
