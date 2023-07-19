using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

public class SpriteCompressor : MonoBehaviour
{
    public List<Sprite> spritesToPack; // Add individual sprites to this list in the Inspector
    public int padding = 2; // Adjust padding between sprites in the packed texture

    private void Start()
    {
        PackSprites();
    }

    [Button]
    private void PackSprites()
    {
        // Create a list of all the individual sprite textures
        List<Texture2D> texturesToPack = new List<Texture2D>();
        foreach (Sprite sprite in spritesToPack)
        {
            Texture2D spriteTexture = sprite.texture;
            texturesToPack.Add(spriteTexture);
        }

        // Pack the textures into a single texture
        Texture2D packedTexture = new Texture2D(1, 1); // Create a new texture to hold the packed sprites
        Rect[] packedUVs = packedTexture.PackTextures(texturesToPack.ToArray(), padding, 2048); // Adjust the size as needed

        // Assign the packed UVs to each sprite
        for (int i = 0; i < spritesToPack.Count; i++)
        {
            Rect uv = packedUVs[i];
            Sprite sprite = spritesToPack[i];
            Rect spriteRect = sprite.rect;
            float spriteWidth = spriteRect.width;
            float spriteHeight = spriteRect.height;

            float offsetX = uv.x * packedTexture.width;
            float offsetY = uv.y * packedTexture.height;
            float scaleX = uv.width * packedTexture.width / spriteWidth;
            float scaleY = uv.height * packedTexture.height / spriteHeight;

            Vector2 pivot = sprite.pivot;
            pivot.x *= scaleX;
            pivot.y *= scaleY;

            SpriteMetaData metaData = new SpriteMetaData();
            metaData.rect = new Rect(offsetX, offsetY, spriteWidth, spriteHeight);
            metaData.pivot = pivot;
            metaData.name = sprite.name;

            TextureImporter textureImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(packedTexture)) as TextureImporter;
            textureImporter.spritesheet = new SpriteMetaData[] { metaData };
            textureImporter.SaveAndReimport();
        }

        // Save the packed texture as an asset
        byte[] bytes = packedTexture.EncodeToPNG();
        string savePath = "Assets/Art/PackedTexture.png"; // Adjust the save path as needed
        System.IO.File.WriteAllBytes(savePath, bytes);

        Debug.Log("Texture packing complete. Packed texture saved at: " + savePath);
    }
}
