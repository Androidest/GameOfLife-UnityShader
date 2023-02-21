using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameofLifeController : MonoBehaviour
{
    [SerializeField] Material GOLComputingMat;
    [SerializeField] RenderTexture CurGenerationRT;
    [SerializeField] RenderTexture NextGenerationRT;
    [SerializeField] float FrameRate = 10000f;

    public bool IsPause;
    private Color[] Pixels;
    private Texture2D InputTexture;
    private float DeltaTime;

    void Awake()
    {
        IsPause = true;
        InputTexture = new Texture2D(CurGenerationRT.width, CurGenerationRT.height, TextureFormat.R8, false);
        Pixels = new Color[CurGenerationRT.width * CurGenerationRT.height];
        for (int i = 0; i < Pixels.Length; ++i)
        {
            Pixels[i] = new Color(0,0,0,0);
        }
        Reset();
    }

    public float PlaySpeed
    {
        set
        {
            var easeVal = Mathf.Exp(-4 * value);
            const float maxFrameTime = 1.5f;
            FrameRate = Mathf.Clamp(maxFrameTime * easeVal, 0, maxFrameTime);
        }
    }

    public void Reset()
    {
        for (int i = 0; i < Pixels.Length; ++i)
        {
            Pixels[i].r = 0;
        }
        UpdatePixelsToRenderTexture();
    }

    public void DrawPixel(int x, int y)
    {
        var hasCell = GetPixel(x, y);
        SetPixel(x, y, !hasCell);
        InputTexture.SetPixel(x, y, hasCell ? new Color(0, 0, 0, 0) : new Color(1, 0, 0, 0));
        InputTexture.Apply();
        Graphics.Blit(InputTexture, CurGenerationRT);
    }

    public bool GetPixel(int x, int y)
    {
        if (x < 0 || CurGenerationRT.width < x || y < 0 || CurGenerationRT.height < y)
        {
            Debug.LogError("Wrong Pos");
            return false;
        }
        return Pixels[CurGenerationRT.width * y + x].r > 0;
    }

    public void SetPixel(int x, int y, bool val)
    {
        if (x < 0 || CurGenerationRT.width < x || y < 0 || CurGenerationRT.height < y)
        {
            Debug.LogError("Wrong Pos");
            return;
        }
        Pixels[CurGenerationRT.width * y + x].r = val ? 1 : 0;
    }

    public void UpdateRenderTextureToPixel()
    {
        RenderTexture.active = CurGenerationRT;
        InputTexture.ReadPixels(new Rect(0, 0, InputTexture.width, InputTexture.height), 0, 0);
        InputTexture.Apply();
        Pixels = InputTexture.GetPixels();
        RenderTexture.active = null;
    }

    private void UpdatePixelsToRenderTexture()
    {
        InputTexture.SetPixels(Pixels);
        // Apply the pixel values to the RenderTexture
        InputTexture.Apply();
        Graphics.Blit(InputTexture, CurGenerationRT);
    }

    void Update()
    {
        if (IsPause)
            return;

        DeltaTime += Time.deltaTime;
        if (DeltaTime >= FrameRate)
        {
            DeltaTime -= FrameRate;
            Graphics.Blit(CurGenerationRT, NextGenerationRT, GOLComputingMat);
            Graphics.Blit(NextGenerationRT, CurGenerationRT);
        }
    }
}
