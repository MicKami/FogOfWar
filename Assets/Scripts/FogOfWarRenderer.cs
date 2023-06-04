using UnityEngine;
using System.Collections.Generic;

public class FogOfWarRenderer : MonoBehaviour
{
    [SerializeField]
    private FogOfWarData data;
    [SerializeField]
    private Texture2D LUT;
    [field: SerializeField]
    public bool UseStaticFog { get; set; }

    [field: SerializeField]
    public bool Upscale { get; set; }
    [field: SerializeField, Range(0, 8)]
    public int BlurIterations { get; set; }

    [field: SerializeField, Range(0f, 1f), Header("FogProperties")]
    public float Opacity { get; set; }
    [field: SerializeField, Range(0f, 1f)]
    public float EdgeMin { get; set; }
    [field: SerializeField, Range(0f, 1f)]
    public float EdgeMax { get; set; }

    private GameObject FogOfWarPlane;
    private RenderTexture output_dynamic;
    private RenderTexture output_persistent;
    private Material upscaleShader;
    private Material blurShader;
    [SerializeField]
    private Material fogOfWarMaterial;

    private void Awake()
    {
        output_dynamic = new RenderTexture(data.Grid.CellCount.x * 4, data.Grid.CellCount.y * 4, 0);
        output_persistent = new RenderTexture(data.Grid.CellCount.x * 4, data.Grid.CellCount.y * 4, 0);

        upscaleShader = new Material(Shader.Find("Custom/Upscale"));
        upscaleShader.SetTexture("_LUT", LUT);
        blurShader = new Material(Shader.Find("Custom/Blur"));

        CreateFoWPlane();
    }

    private void OnValidate()
    {
        if (FogOfWarPlane)
        {
            FogOfWarPlane.GetComponent<Renderer>().sharedMaterial.SetFloat("_EdgeMin", EdgeMin);
            FogOfWarPlane.GetComponent<Renderer>().sharedMaterial.SetFloat("_EdgeMax", EdgeMax);
            FogOfWarPlane.GetComponent<Renderer>().sharedMaterial.SetFloat("_Opacity", Opacity);
        }
    }

    private void Update()
    {
        fogOfWarMaterial.SetFloat("_UseStatic", UseStaticFog ? 0 : 1);
        Texture2D tex_persistent = CellsToTexture(data.StaticFogCells, data.Grid.CellCount.x, data.Grid.CellCount.y);
        Texture2D tex_dynamic = CellsToTexture(data.DynamicFogCells, data.Grid.CellCount.x, data.Grid.CellCount.y);
        if (Upscale)
        {
            output_dynamic.filterMode = FilterMode.Bilinear;
            output_persistent.filterMode = FilterMode.Bilinear;
            Process(tex_persistent, output_persistent);
            Process(tex_dynamic, output_dynamic);
        }
        else
        {
            output_dynamic.filterMode = FilterMode.Point;
            output_persistent.filterMode = FilterMode.Point;
            Graphics.Blit(tex_persistent, output_persistent);
            Graphics.Blit(tex_dynamic, output_dynamic);
        }
    }

    private void CreateFoWPlane()
    {
        FogOfWarPlane = GameObject.CreatePrimitive(PrimitiveType.Quad);
        FogOfWarPlane.transform.rotation = Quaternion.Euler(new Vector3(90f, 0, 0));
        FogOfWarPlane.transform.parent = transform;
        FogOfWarPlane.hideFlags = HideFlags.HideInHierarchy;
        FogOfWarPlane.layer = LayerMask.NameToLayer("Floor");
        fogOfWarMaterial.SetTexture("_Persistent_FoW", output_persistent);
        fogOfWarMaterial.SetTexture("_Dynamic_FoW", output_dynamic);
        FogOfWarPlane.GetComponent<Renderer>().material = fogOfWarMaterial;
        FogOfWarPlane.GetComponent<Renderer>().sharedMaterial.SetFloat("_EdgeMin", EdgeMin);
        FogOfWarPlane.GetComponent<Renderer>().sharedMaterial.SetFloat("_EdgeMax", EdgeMax);
        FogOfWarPlane.GetComponent<Renderer>().sharedMaterial.SetFloat("_Opacity", Opacity);
        FogOfWarPlane.transform.localScale = new Vector3(data.Grid.CellCount.x * data.Grid.CellSize, data.Grid.CellCount.y * data.Grid.CellSize, 1);
        FogOfWarPlane.transform.localPosition = data.Grid.Center;
    }
    private Texture2D CellsToTexture(HashSet<Vector2Int> input, int width, int height)
    {
        Texture2D tex = new Texture2D(width, height);
        tex.filterMode = FilterMode.Point;
        Color[] pixels = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = y * width + x;
                var postionInTextureSpace = new Vector2Int(Mathf.FloorToInt(x * data.Grid.CellSize), Mathf.FloorToInt(y * data.Grid.CellSize)) - data.Grid.SizeInt / 2;
                if (input.Contains(postionInTextureSpace))
                {
                    pixels[index] = Color.white;
                }
                else
                {
                    pixels[index] = Color.black;
                }
            }
        }

        tex.SetPixels(pixels);
        tex.Apply();
        return tex;
    }
    private void Process(Texture2D input, RenderTexture output)
    {
        var buffer1 = RenderTexture.GetTemporary(output.width, output.height);
        var buffer2 = RenderTexture.GetTemporary(output.width, output.height);
        buffer1.filterMode = FilterMode.Bilinear;
        buffer2.filterMode = FilterMode.Bilinear;

        Graphics.Blit(input, buffer1, upscaleShader);
        for (int i = 1; i <= BlurIterations; i++)
        {
            Graphics.Blit(buffer1, buffer2, blurShader, 0);
            Graphics.Blit(buffer2, buffer1, blurShader, 1);
        }
        Graphics.Blit(buffer1, output);
        RenderTexture.ReleaseTemporary(buffer1);
        RenderTexture.ReleaseTemporary(buffer2);
    }

    public void SetBlurIterations(float value)
    {
        BlurIterations = (int)value;
    }
}