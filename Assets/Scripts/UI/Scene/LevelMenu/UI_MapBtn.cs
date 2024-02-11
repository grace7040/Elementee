using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_MapBtn : MonoBehaviour
{
    public int mapNum = 0;
    public List<GameObject>Stars = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(MapStart);
        
        //// º° °ü·Ã
        //if (GameManager.Instance.developMode || mapNum < GameManager.Instance.mapBest)
        //{ 
        //    for (int f = 0; f < Stars.Count; f++)
        //    {
        //        Stars[f].SetActive(true);
        //        // GameManager.Instance.mapStar[mapNum]
        //        if (f < GameManager.Instance.mapStar[mapNum])
        //            Stars[f].GetComponent<Image>().color = new Color32(255, 250, 99, 255);
        //        else
        //            Stars[f].GetComponent<Image>().color = new Color32(150, 150, 150, 255);
        //    }
        //}

    }

    private void MapStart()
    {
        GameManager.Instance.currentMapNum = mapNum;
        SceneManager.LoadScene("Map_" + mapNum);
    }
}
