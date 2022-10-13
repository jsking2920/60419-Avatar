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

    private Coroutine distortion_co;
    private bool distortion_enabled = false;

    void Start()
    {
        lo = new Preset(GraphicsFormat.R8G8B8A8_UNorm, 240, 180);
        mid = new Preset(GraphicsFormat.R8G8B8A8_UNorm, 320, 240);
        hi = new Preset(GraphicsFormat.R32G32B32A32_SFloat, 640, 480); // was GraphicsFormat.R16G16B16_UNorm but thats not supported??

        renderMat.EnableKeyword("DISTORT_ON");
        renderMat.SetFloat("_DistortionStrength", 0.0f);
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
            if (distortion_co == null)
            {
                if (distortion_enabled)
                {
                    distortion_co = StartCoroutine(FadeDistrotionFX(0.75f, 0.0f, "_DistortionStrength", "DISTORT_ON"));
                }
                else
                {
                    distortion_co = StartCoroutine(FadeDistrotionFX(0.0f, 0.75f, "_DistortionStrength", "DISTORT_ON"));
                }
                distortion_enabled = !distortion_enabled;
            }
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

    private IEnumerator FadeDistrotionFX(float start_val, float target_value, string name, string keyword)
    {
        float cur = start_val; ;
        renderMat.SetFloat(name, start_val);

        while (Mathf.Abs(target_value - cur) > 0.05f)
        {
            cur = Mathf.Lerp(cur, target_value, Time.deltaTime * 15.0f);
            renderMat.SetFloat(name, cur);
            yield return new WaitForSeconds(0.05f);
        }

        renderMat.SetFloat(name, target_value);
        distortion_co = null;
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