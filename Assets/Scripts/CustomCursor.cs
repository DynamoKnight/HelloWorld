using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    // The original cursor image
    [SerializeField] private Texture2D cursorTexture;
    // The scaled cursor image
    private Texture2D scaledCursorTexture;
    // The desired size for the cursor
    [SerializeField] private Vector2 cursorSize = new Vector2(32, 32);

    // The hotspot is the point at which to click
    private Vector2 cursorHotspot;

    void Start(){
        // Scales the original texture
        scaledCursorTexture = ScaleTexture(cursorTexture, (int)cursorSize.x, (int)cursorSize.y);
        
        // The hotspot is set to the center
        //cursorHotspot = new Vector2(cursorSize.x / 2, cursorSize.y / 2);
        // The hotspot is set to the top left
        cursorHotspot = new Vector2(0, 0);

        // Sets the cursor
        Cursor.SetCursor(scaledCursorTexture, cursorHotspot, CursorMode.Auto);
    }

    // Scales a texture to a new size
    private Texture2D ScaleTexture(Texture2D texture, int width, int height){
        // Creates a new texture with the desired size
        Texture2D scaledTexture = new Texture2D(width, height);

        // Scales the texture using bilinear filtering
        for (int y = 0; y < scaledTexture.height; y++){
            for (int x = 0; x < scaledTexture.width; x++){
                float xCoord = x * 1.0f / scaledTexture.width * texture.width;
                float yCoord = y * 1.0f / scaledTexture.height * texture.height;
                Color color = texture.GetPixelBilinear(xCoord, yCoord);
                scaledTexture.SetPixel(x, y, color);
            }
        }
        // Apply changes
        scaledTexture.Apply();

        return scaledTexture;
    }

}