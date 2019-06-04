using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;

class TextureModifier : AssetPostprocessor
{
    //导入图片时调用
    void OnPostprocessTexture(Texture2D texture)
    {

        string AtlasName = new DirectoryInfo(Path.GetDirectoryName(assetPath)).Name;
        TextureImporter textureImporter = assetImporter as TextureImporter;
        textureImporter.textureType = TextureImporterType.Sprite;
        if (assetPath.IndexOf("Assets/UI/") != -1)
        {
            textureImporter.spritePackingTag = AtlasName;
        }
        textureImporter.mipmapEnabled = false;
        textureImporter.spriteImportMode = SpriteImportMode.Single;
        textureImporter.alphaIsTransparency = true;
        textureImporter.wrapMode = TextureWrapMode.Clamp;
        AssetDatabase.Refresh();
    }
}