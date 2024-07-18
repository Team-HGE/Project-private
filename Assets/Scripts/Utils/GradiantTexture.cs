using UnityEngine;

public class GradientTexture : MonoBehaviour
{
    public int width = 2000;
    public int height = 300;
    public Color bottomColor = Color.black;
    public Color topColor = Color.black;

    void Start()
    {
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        for (int y = 0; y < height; y++)
        {
            float alpha = (float)y / (height - 1); // 아래에서 위로 투명도가 점차 증가
            for (int x = 0; x < width; x++)
            {
                Color color = Color.Lerp(bottomColor, topColor, alpha);
                color.a = alpha;
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();

        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        Rect rect = new Rect(0, 0, width, height);
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        Sprite sprite = Sprite.Create(texture, rect, pivot);
        spriteRenderer.sprite = sprite;
    }
}
