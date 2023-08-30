using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
//using Unity.Burst.Intrinsics;
//using UnityEditor.SearchService;
using System.IO;
using System;

public class SpriteCompressor : MonoBehaviour
{
    public List<Sprite> spritesToPack; // Add individual sprites to this list in the Inspector
    public int padding = 2; // Adjust padding between sprites in the packed texture

    //might not need all that just put it all in one file

#if UNITY_EDITOR
    [InfoBox("DONT FORGET TO MOVE TEXTURES TO SAFE ZONE AFTER COMPLEATION")]
    [Button(ButtonSizes.Small), ShowIf("showSpritePacker")]
    private void PackSprites()
    {
        if (spritesToPack == null || spritesToPack.Count == 0)
        {
            Debug.LogWarning("No sprites selected. Please add sprites to the spritesToPack list in the Inspector.");
            return;
        }

        int packedWidth = 0;
        int packedHeight = 0;

        // Calculate the total width and height needed for the packed texture
        foreach (Sprite sprite in spritesToPack)
        {
            packedWidth += (int)sprite.rect.width + padding;
            packedHeight = Mathf.Max(packedHeight, (int)sprite.rect.height);
        }

        // Create a new texture to hold the packed sprites
        Texture2D packedTexture = new Texture2D(packedWidth, packedHeight, TextureFormat.RGBA32, false);

        // Keep track of the current X position for arranging the sprites side by side
        int currentX = 0;

        // Pack the sprites into the new texture
        foreach (Sprite sprite in spritesToPack)
        {
            Color[] pixels = sprite.texture.GetPixels((int)sprite.rect.x, (int)sprite.rect.y, (int)sprite.rect.width, (int)sprite.rect.height);
            packedTexture.SetPixels(currentX, 0, (int)sprite.rect.width, (int)sprite.rect.height, pixels);

            // Move the currentX position to the next sprite's position, adding padding
            currentX += (int)sprite.rect.width + padding;
        }
 
        packedTexture.Apply();

        // Save the packed texture as an asset
        string outputPath = $"Assets/Art/icons/_PaackedMatTexture{UnityEngine.Random.Range(0, 9999999)}.png"; // Adjust the save path as needed

        byte[] bytes = packedTexture.EncodeToPNG();
        File.WriteAllBytes(outputPath, bytes);
        AssetDatabase.Refresh();

        Debug.Log("Sprite packing complete. Packed texture saved at: " + outputPath);
    }

    [SerializeField] private bool showSpriteFinder = false;
    [SerializeField, ShowIf("showSpriteFinder")] private ItemCollection itemCollection;
    [SerializeField, ShowIf("showSpriteFinder")] int _index = 0;
    [InlineEditor]
    [SerializeField, LabelWidth(30), ShowIf("showSpriteFinder")] Item _item = null;
    [Button(ButtonSizes.Medium), ShowIf("showSpriteFinder")]
    private void PlaceNextItemInPlace()
    {
        _item = itemCollection.Test_FindItemFromSprite(spritesToPack[_index]);
            _index++;
    }

    [SerializeField] private bool showSpritePacker = false;
    
#endif
}
