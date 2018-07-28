using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnityFractals : MonoBehaviour
{
    public Material mat;
    Texture2D texture;

    public int width = 100;
    public int height = 100;

    public int iterations;
    public int limit;

    public float add = 2;

    float xmin = -1; // zooming should be possible, by adjusting these values
    float xmax = 1;
    float ymin = -1;
    float ymax = 1;

    public Vector2 pos;
    public float zoom;

    int ix = 0;
    void Start()
    {

        Reset();

    }
    private void Reset()
    {
        texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        mat.mainTexture = texture;
    }
    void Update()
    {

        //int iterations = 15;
        if (zoom == 0) zoom = 0.00001f;

        xmin = (pos.x - 1) * zoom;
        xmax = pos.x;
        ymin = (pos.y - 1) * zoom;
        ymax = pos.y;



        if (Input.GetKeyDown(KeyCode.Space))
        {

            RenderFractal();
        }
        if (Input.GetKeyDown(KeyCode.Q)) Scr();
        texture.Apply();
    }

    public static string ScreenShotName(int width, int height, string Path)
    {
        return string.Format(@"{ 0}/ screen_{ 1}
    x{ 2}
    _{ 3}.png",
    Path,
    width, height,
    System.DateTime.Now.ToString("yyyy - MM - dd_HH - mm - ss"));
    }

    public void Scr()
    {
        int resWidth = texture.width;
        int resHeight = texture.height;
        //RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        //camera.targetTexture = rt;
        // Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        // camera.Render();
        // RenderTexture.active = rt;

        texture.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        //camera.targetTexture = null;
        //RenderTexture.active = null; // JC: added to avoid errors
        // Destroy(rt);
        byte[] bytes = texture.EncodeToPNG();

        string filename = ScreenShotName(resWidth, resHeight, "C:\\");
        System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("Took screenshot to: { 0}", filename));

    }
    public int Mandelbrot(float cx, float cy, int maxiter)
    {
        int i = 0;
        float x = 0f;
        float y = 0f;
        for (i = 0; i < maxiter && (x * x + y * y) <= limit; i++)
        {
            float tmp = add * x * y;
            x = x * x - y * y + cx;
            y = tmp + cy;
        }
        return i;
    }

    void RenderFractal()
    {

        ix = 0;
        while (ix < width)
        {
            for (int iy = 0; iy < height; iy++)
            {
                float x = xmin + (xmax - xmin) * ix / (width - 1);
                float y = ymin + (ymax - ymin) * iy / (height - 1);

                float i = Mandelbrot(x, y, iterations);

                if (i == iterations)
                {
                    texture.SetPixel(ix, iy, Color.black);
                }
                else
                {
                    float c = 3f * Mathf.Log(i) / Mathf.Log(iterations - 1f);
                    if (c < 1f)
                    {
                        texture.SetPixel(ix, iy, new Color(c, 0, 0, 1f));
                    }
                    else if (c < 2f)
                    {
                        texture.SetPixel(ix, iy, new Color(1f, c - 1f, 0, 1f));
                    }
                    else
                    {
                        texture.SetPixel(ix, iy, new Color(1f, 1f, c - 2f, 1f));
                    }
                }
            }
            ix++;
        }
    }

}