using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Retake
{
    [ExecuteInEditMode]
    public class IconMaker : MonoBehaviour
    {
        public bool create;
        public RenderTexture ren;
        public Camera bakeCam;

        public string spriteName;

        void Update()
        {
            if(create)
            {
                create = false;
                CreateIcon();
            }
        }

        void CreateIcon()
        {
            if(string.IsNullOrEmpty(spriteName))
            {
                spriteName = "icon";
            }

            string path = SaveLocation();
            path += spriteName;
            bakeCam.targetTexture = ren;

            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = bakeCam.targetTexture;
            bakeCam.Render();

            Texture2D imgPng = new Texture2D(bakeCam.targetTexture.width, bakeCam.targetTexture.height, TextureFormat.ARGB32,false);
            imgPng.ReadPixels(new Rect(0, 0, bakeCam.targetTexture.width, bakeCam.targetTexture.height),0,0);
            imgPng.Apply();
            RenderTexture.active = currentRT;
            byte[] bytesPng = imgPng.EncodeToPNG();
            System.IO.File.WriteAllBytes(path + ".png", bytesPng);        }

        string SaveLocation()
        {
            string saveLocation = Application.streamingAssetsPath + "/Icons/";

            if(!Directory.Exists(saveLocation))
            {
                Directory.CreateDirectory(saveLocation);
            }
            return saveLocation;
        }
    }
}
