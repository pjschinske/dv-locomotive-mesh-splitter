/*
 * Created by Dummiesman 2013-2019
 * Thanks to mikezila for improving the initial TGA loading code
*/

using System;
using UnityEngine;
using System.Collections;
using System.IO;
using B83.Image.BMP;

namespace Dummiesman
{
    public class ImageLoader
    {
        /// <summary>
        /// Converts a DirectX normal map to Unitys expected format
        /// </summary>
        /// <param name="tex">Texture to convert</param>
        public static void SetNormalMap(ref Texture2D tex)
        {
            Color[] pixels = tex.GetPixels();
            for (int i = 0; i < pixels.Length; i++)
            {
                Color temp = pixels[i];
                temp.r = pixels[i].g;
                temp.a = pixels[i].r;
                pixels[i] = temp;
            }
            tex.SetPixels(pixels);
            tex.Apply(true);
        }

        public enum TextureFormat
        {
            DDS,
            TGA,
            BMP,
            PNG,
            JPG,
            CRN
        }


        /// <summary>
        /// Loads a texture from a stream
        /// </summary>
        /// <param name="stream">The stream</param>
        /// <param name="format">The format **NOT UNITYENGINE.TEXTUREFORMAT**</param>
        /// <returns></returns>
        public static Texture2D LoadTexture(Stream stream, TextureFormat format)
        {
            return null;
        }

      
        /// <summary>
        /// Loads a texture from a file
        /// </summary>
        /// <param name="fn"></param>
        /// <param name="normalMap"></param>
        /// <returns></returns>
        public static Texture2D LoadTexture(string fn)
        {
			return null;
        }

    }
}
