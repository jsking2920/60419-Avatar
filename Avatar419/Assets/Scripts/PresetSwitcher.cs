using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class PresetSwitcher : MonoBehaviour
{
    [SerializeField] private RenderTexture rt;

    private Preset lo;
    private Preset mid;
    private Preset hi;

    [SerializeField] Material renderMat;

    void Start()
    {
        lo = new Preset(GraphicsFormat.R8G8B8A8_UNorm, 240, 180);
        mid = new Preset(GraphicsFormat.R8G8B8A8_UNorm, 320, 240);
        hi = new Preset(GraphicsFormat.R32G32B32A32_SFloat, 640, 480); // was GraphicsFormat.R16G16B16_UNorm but thats not supported??
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetRenderTexture(lo);
            renderMat.EnableKeyword("STATIC_ON");
            renderMat.DisableKeyword("STATIC_ON");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetRenderTexture(mid);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetRenderTexture(hi);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            ToggleKeyword("STATIC_ON");
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            ToggleKeyword("DISTORT_ON");
        }
    }

    private void SetRenderTexture(Preset p)
    {
        rt.Release();
        rt.width = p.width;
        rt.height = p.height;
        rt.graphicsFormat = p.color_format;
    }

    private void ToggleKeyword(string key)
    {
        if (renderMat.IsKeywordEnabled(key))
        {
            renderMat.DisableKeyword(key);
        }
        else
        {
            renderMat.EnableKeyword(key);
        }
    }
}

[System.Serializable]
public struct Preset
{
    public GraphicsFormat color_format;
    public int width;
    public int height;

    public Preset(GraphicsFormat _color_format, int _width, int _height)
    {
        color_format = _color_format;
        width = _width;
        height = _height;
    }
}