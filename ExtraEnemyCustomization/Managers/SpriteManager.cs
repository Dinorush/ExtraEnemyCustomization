﻿using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EECustom.Managers
{
    public class SpriteManager
    {
        public static string BaseSpritePath { get; private set; }

        private static readonly Dictionary<string, Texture2D> _TextureCache = new Dictionary<string, Texture2D>();
        private static readonly Dictionary<string, Sprite> _SpriteCache = new Dictionary<string, Sprite>();

        public static void Initialize()
        {
            BaseSpritePath = Path.Combine(ConfigManager.BasePath, "icons");
            if (!Directory.Exists(BaseSpritePath))
                return;

            var files = Directory.GetFiles(BaseSpritePath, "*.png", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                TryCacheTexture2D(file);
            }
        }

        public static void TryCacheTexture2D(string file)
        {
            if (!File.Exists(file))
                return;

            var fileNameWOExt = Path.GetFileNameWithoutExtension(file).ToLower();
            if (!_TextureCache.ContainsKey(fileNameWOExt))
            {
                var fileData = File.ReadAllBytes(file);
                var texture2D = new Texture2D(2, 2);
                if (!ImageConversion.LoadImage(texture2D, fileData))
                    return;

                _TextureCache.Add(fileNameWOExt, texture2D);
            }
        }

        public static Sprite GenerateSprite(string fileName, float pixelsPerUnit = 64.0f)
        {
            SetFilenameFormat(ref fileName);

            if (!_TextureCache.TryGetValue(fileName, out var texture2D))
                return null;

            var spriteKey = GetSpriteKey(fileName, pixelsPerUnit);
            if (_SpriteCache.TryGetValue(spriteKey, out var sprite))
                return sprite;

            var newSprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
            var antiDestroy = new GameObject();
            var spriteRenderer = antiDestroy.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = newSprite;
            UnityEngine.Object.DontDestroyOnLoad(antiDestroy);
            antiDestroy.name = "PluginGenerated_CustomSpriteHolder_" + spriteKey;
            antiDestroy.SetActive(false);
            _SpriteCache.Add(spriteKey, sprite);

            return newSprite;
        }

        public static bool TryGetSpriteCache(string fileName, float pixelsPerUnit, out Sprite sprite)
        {
            SetFilenameFormat(ref fileName);
            return _SpriteCache.TryGetValue(GetSpriteKey(fileName, pixelsPerUnit), out sprite);
        }

        private static string GetSpriteKey(string name, float pixelsPerUnit)
        {
            var spriteKey = $"{name}__{pixelsPerUnit:0.##}";
            return spriteKey;
        }

        private static void SetFilenameFormat(ref string fileName)
        {
            fileName = fileName.ToLower();

            if (Path.HasExtension(fileName))
                fileName = Path.GetFileNameWithoutExtension(fileName);
        }
    }
}