using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public enum Stars
    {
        Star_S, Star_M, Star_L , Star_Run
    }

    public Stars starType;
    public int score;


    public int GetScore(Stars star)
    {
        switch (star)
        {
            case Stars.Star_S:
                return 10;
            case Stars.Star_M:
                return 20;
            case Stars.Star_L:
                return 30;
            case Stars.Star_Run:
                return 50;
            default:
                return 10;
        }
    }

    private void Start()
    {
        score = GetScore(starType);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // ¿Ã∆Â∆Æ ≥÷±‚
            GameManager.Instance.totalScore += score;
            GameManager.Instance.StarCount();
            AudioManager.Instacne.PlaySFX("Star");
            Destroy(this.gameObject);
        }
    }

}
