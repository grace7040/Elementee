using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintBrush : MonoBehaviour
{
    //Setting
    public int resolution = 512;
    Texture2D whiteMap;

    //Paint & Brush
    float xCoordinate;
    float yCoordinate;
    RawImage rawImage;
    RenderTexture renderTexture;
    Colors brushColor = Colors.def;
    public float brushSize;
    public Texture2D[] brushTextures;
    Texture2D brushTexture;

    //Result
    Texture2D sourceTex;
    public GameObject customObj;

    void OnEnable()
    {
        /*  TEST CODE  */
        SetBrushColor(Colors.def);
        /*  TEST CODE  */

        CreateClearTexture();// clear transparent texture to draw on
        rawImage = GetComponent<RawImage>();
        renderTexture = getWhiteRT();
        rawImage.texture = renderTexture;
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) {
            xCoordinate = Mathf.Floor(Input.mousePosition.x - rawImage.rectTransform.position.x);
            yCoordinate = Mathf.Floor(Input.mousePosition.y - rawImage.rectTransform.position.y);
            DrawTexture(renderTexture, xCoordinate, yCoordinate);
        }        
    }

    void DrawTexture(RenderTexture rt, float posX, float posY)
    {
        RenderTexture.active = rt; // activate rendertexture for drawtexture;
        GL.PushMatrix();                       // save matrixes
        GL.LoadPixelMatrix(0, resolution, resolution, 0);      // setup matrix for correct size

        // draw brushtexture
        
        Graphics.DrawTexture(new Rect(posX - brushTexture.width / brushSize, (rt.height - posY) - brushTexture.height / brushSize, brushTexture.width / (brushSize * 0.5f), brushTexture.height / (brushSize * 0.5f)), brushTexture);
        GL.PopMatrix();
        RenderTexture.active = null;// turn off rendertexture
    }

    RenderTexture getWhiteRT()
    {
        RenderTexture rt = new RenderTexture(resolution, resolution, 32);
        Graphics.Blit(whiteMap, rt);
        return rt;
    }

    void CreateClearTexture()
    {
        whiteMap = new Texture2D(1, 1, TextureFormat.ARGB32, false);
        whiteMap.SetPixel(0, 0, new Color(0,0,0,0));
        whiteMap.Apply();
    }

    Texture2D toTexture2D(RenderTexture rTex)
    {
        sourceTex = new Texture2D(rTex.width, rTex.height, TextureFormat.ARGB32, false); ;
        RenderTexture.active = rTex;
        sourceTex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        sourceTex.Apply();
        RenderTexture.active = null;
        return sourceTex;
    }

    public void SaveCustomTexture()
    {
        customObj.GetComponent<MeshRenderer>().material.mainTexture = toTexture2D(renderTexture);
    }

    public void Reset()
    {
        CreateClearTexture();// clear transparent texture to draw on
        renderTexture = getWhiteRT();
        rawImage.texture = renderTexture;
    }


    // ** PaintBrush를 활성화할 때 항상 브러시 컬러를 지정해줘야 함. **
    public void SetBrushColor(Colors color)
    {
        brushTexture = brushTextures[(int)color];
    }

}