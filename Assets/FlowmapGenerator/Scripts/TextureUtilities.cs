//----------------------------------------------
// Flowmap Generator
// Copyright © 2013 Superposition Games
// http://www.superpositiongames.com
// support@superpositiongames.com
//----------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;

namespace Flowmap{
	
	/** Utilities for writing and reading textures to disk and support for texture sampling operations when using multithreading. */
	public class TextureUtilities {
		
		public struct FileTextureFormat {
			public string name;
			public string extension;
			public FileTextureFormat (string name, string extension){
				this.name = name;
				this.extension = extension;
			}
		}
		
		public static FileTextureFormat[] SupportedFormats = new FileTextureFormat[]{
			new FileTextureFormat ("Tga", "tga"),
			new FileTextureFormat ("Png", "png")
		};
		
		public static string[] GetSupportedFormatsWithExtension (){
			string[] formats = new string[SupportedFormats.Length];
			for(int i=0; i<SupportedFormats.Length; i++){
				formats[i] = SupportedFormats[i].name +" (*."+SupportedFormats[i].extension+")";
			}
			return formats;
		}
		
		public static FileTextureFormat[] SupportedRawFormats = new FileTextureFormat[]{
			new FileTextureFormat ("Raw", "raw")
		};
		
		/** Write a render texture to file. The render texture is blited to an ARGB32 first, so HDR formats are supported but lose precision. */
		public static void WriteRenderTextureToFile (RenderTexture textureToWrite, string filename, FileTextureFormat format){
			WriteRenderTextureToFile (textureToWrite, filename, false, format);	
		}
		
		public static void WriteRenderTextureToFile (RenderTexture textureToWrite, string filename, bool linear, FileTextureFormat format)
		{
			#if !UNITY_WEBPLAYER
			Texture2D tempTex = new Texture2D(textureToWrite.width, textureToWrite.height, TextureFormat.ARGB32, false, linear);
			RenderTexture tempRT = RenderTexture.GetTemporary(textureToWrite.width, textureToWrite.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
			
			Graphics.Blit (textureToWrite, tempRT);
			
			RenderTexture.active = tempRT;
			tempTex.ReadPixels (new Rect(0,0,tempTex.width, tempTex.height), 0,0);
			tempTex.Apply(false);
			WriteTexture2DToFile (tempTex, filename, format);
			if(Application.isPlaying)
				UnityEngine.Object.Destroy (tempTex);
			else
				UnityEngine.Object.DestroyImmediate (tempTex);
			RenderTexture.ReleaseTemporary (tempRT);
			#endif
		}
		
		public static void WriteRenderTextureToFile (RenderTexture textureToWrite, string filename, bool linear, FileTextureFormat format, string customShader)
		{
			#if !UNITY_WEBPLAYER
			Texture2D tempTex = new Texture2D(textureToWrite.width, textureToWrite.height, TextureFormat.ARGB32, false, linear);
			RenderTexture tempRT = RenderTexture.GetTemporary(textureToWrite.width, textureToWrite.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
			
			Material toLdrMat = new Material(Shader.Find (customShader));
			toLdrMat.SetTexture ("_RenderTex", textureToWrite);
			Graphics.Blit (null, tempRT, toLdrMat);
			if(Application.isPlaying){
				UnityEngine.Object.Destroy (toLdrMat);
			}else{
				UnityEngine.Object.DestroyImmediate (toLdrMat);
			}
			
			RenderTexture.active = tempRT;
			tempTex.ReadPixels (new Rect(0,0,tempTex.width, tempTex.height), 0,0);
			tempTex.Apply(false);
			WriteTexture2DToFile (tempTex, filename, format);
			if(Application.isPlaying)
				UnityEngine.Object.Destroy (tempTex);
			else
				UnityEngine.Object.DestroyImmediate (tempTex);
			RenderTexture.ReleaseTemporary (tempRT);
			#endif
		}
		
		public static void WriteTexture2DToFile (Texture2D textureToWrite, string filename, FileTextureFormat format)
		{
			#if !UNITY_WEBPLAYER
			byte[] bytes = null;
			switch(format.name){
			case "Png":
				bytes = textureToWrite.EncodeToPNG ();
				break;
			case "Tga":
				bytes = EncodeToTGA (textureToWrite);
				break;
			}
			if(!filename.EndsWith ("."+ format.extension)){
				filename += "."+ format.extension;
			}
			System.IO.File.WriteAllBytes (filename, bytes);
			#endif
		}
		
		static public Color SampleColorBilinear (Color[] data, int resolutionX, int resolutionY, float u, float v)
	    {
	        u = Mathf.Clamp(u * (resolutionX - 1), 0, resolutionX-1);
	        v = Mathf.Clamp(v * (resolutionY - 1), 0, resolutionY-1);
			if((Mathf.FloorToInt(u) + resolutionX * Mathf.FloorToInt(v)) >= data.Length || (Mathf.FloorToInt(u) + resolutionX * Mathf.FloorToInt(v)) < 0){
				Debug.Log ("out of range " + u +" "+ v +" "+ resolutionX +" "+ resolutionY);
				return Color.black;
			}
	        Color p1 = data[Mathf.FloorToInt(u) + resolutionX * Mathf.FloorToInt(v)];
	        Color p2 = data[Mathf.CeilToInt(u) + resolutionX * Mathf.FloorToInt(v)];
	        Color p3 = data[Mathf.FloorToInt(u) + resolutionX * Mathf.CeilToInt(v)];
	        Color p4 = data[Mathf.CeilToInt(u) + resolutionX * Mathf.CeilToInt(v)];
	
	        float x0 = Mathf.Floor(u);
	        float x1 = Mathf.Floor(u + 1);
	        float y0 = Mathf.Floor(v);
	        float y1 = Mathf.Floor(v + 1);
	        Color xSample1 = ((x1 - u) / (x1 - x0)) * p1 + ((u - x0) / (x1 - x0)) * p2;
	        Color xSample2 = ((x1 - u) / (x1 - x0)) * p3 + ((u - x0) / (x1 - x0)) * p4;
	        return ((y1 - v) / (y1 - y0)) * xSample1 + ((v - y0) / (y1 - y0)) * xSample2;
	    }
		
		static public float[,] ReadRawImage(string path, int resX, int resY, bool pcByteOrder)
	    {
			float[,] rawData = new float[resX, resY];
			UnityEngine.Profiling.Profiler.BeginSample ("Read raw image");	        
	        System.IO.FileStream fs = new System.IO.FileStream (path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
	        System.IO.BinaryReader reader = new System.IO.BinaryReader (fs);
			
			for (int y = resY-1; y > -1; y--)
	        {
	            for (int x = 0; x < resX; x++)
	            {
					byte[] data = reader.ReadBytes(2);
					if(!pcByteOrder){
						byte tempByte = data[0];
						data[0] = data[1];
						data[1] = tempByte;
					}
					System.UInt16 short1 = System.BitConverter.ToUInt16(data, 0);
					rawData[x,y] = (float)short1/65536f;
				}
			}
	        reader.Close();
			UnityEngine.Profiling.Profiler.EndSample ();
	        return rawData;
	    }
		
		static public Texture2D ReadRawImageToTexture (string path, int resX, int resY, bool pcByteOrder){
			float[,] heights = ReadRawImage (path, resX, resY, pcByteOrder);
			UnityEngine.Profiling.Profiler.BeginSample ("Write raw to texture");
			Texture2D heightmap = new Texture2D(resX, resY, TextureFormat.ARGB32, false, true);
			heightmap.wrapMode = TextureWrapMode.Clamp;
			heightmap.anisoLevel = 9;
			heightmap.filterMode = FilterMode.Trilinear;
			
			int threads = SystemInfo.processorCount;
			int chunkSize = Mathf.CeilToInt(resY / threads);
			Color[] colors = new Color[resX * resY];
			ManualResetEvent[] resetEvents = new ManualResetEvent[threads];
			for(int i=0; i<threads; i++){
				int start = i * chunkSize;
				int length = (i==threads-1) ? ((resX - 1) - i * chunkSize) : chunkSize;
				resetEvents[i] = new ManualResetEvent(false);
				ThreadPool.QueueUserWorkItem (ThreadedEncodeFloat, new ColorArrayThreadedInfo(start, length, ref colors, resX, resY, heights, resetEvents[i]));
			}
			WaitHandle.WaitAll (resetEvents);
			UnityEngine.Profiling.Profiler.BeginSample ("Set pixels");
			heightmap.SetPixels (colors);
			UnityEngine.Profiling.Profiler.EndSample ();
			
			UnityEngine.Profiling.Profiler.EndSample ();
			UnityEngine.Profiling.Profiler.BeginSample ("Apply texture");
			heightmap.Apply (false);
			UnityEngine.Profiling.Profiler.EndSample ();
			return heightmap;
		}
		
		class ColorArrayThreadedInfo {
			public int start;
			public int length;
			public ManualResetEvent resetEvent;
			public Color[] colorArray;
			public float[,] heightArray;
			public int resX;
			public int resY;
			public ColorArrayThreadedInfo (int start, int length, ref Color[] colors, int resX, int resY, float[,] heights, ManualResetEvent resetEvent){
				this.start = start;
				this.length = length;
				this.resetEvent = resetEvent;
				this.colorArray = colors;
				this.resX = resX;
				this.resY = resY;
				this.heightArray = heights;
			}
		}
		
		static void ThreadedEncodeFloat (object info){
			ColorArrayThreadedInfo arrayThreadedInfo = info as ColorArrayThreadedInfo;
			try{
				for(int x = arrayThreadedInfo.start; x<arrayThreadedInfo.start + arrayThreadedInfo.length; x++){
					for(int y = 0; y<arrayThreadedInfo.resY; y++){
						arrayThreadedInfo.colorArray[x + y * arrayThreadedInfo.resX] = EncodeFloatRGBA (arrayThreadedInfo.heightArray[x,y]);
					}
				}
			}catch (System.Exception e){
				Debug.Log (e.ToString ());
			}
			arrayThreadedInfo.resetEvent.Set ();
		}
		
		static public Texture2D GetRawPreviewTexture (Texture2D rawTexture){
			Texture2D preview = new Texture2D(rawTexture.width, rawTexture.height, TextureFormat.ARGB32, true, true);
			Color[] colors = new Color[preview.width * preview.height];
			for(int y=0; y<preview.height; y++){
				for(int x=0; x<preview.width; x++){
					float height = DecodeFloatRGBA (rawTexture.GetPixel (x,y));
					colors[x + y * preview.width] = new Color(height, height, height, 1);
				}
			}
			preview.SetPixels (colors);
			preview.Apply ();
			return preview;
		}
		
		public static Color EncodeFloatRGBA(float v)
	    {
	        v = Mathf.Min(v, 0.999f);
	        Color kEncodeMul = new Color(1.0f, 255.0f, 65025.0f, 160581375.0f);
	        float kEncodeBit = 1.0f / 255.0f;
	        Color enc = kEncodeMul * v;
	        enc.r = enc.r - Mathf.Floor(enc.r);
	        enc.g = enc.g - Mathf.Floor(enc.g);
	        enc.b = enc.b - Mathf.Floor(enc.b);
	        enc.a = enc.a - Mathf.Floor(enc.a);
	        enc.r -= enc.g * kEncodeBit;
	        enc.g -= enc.b * kEncodeBit;
	        enc.b -= enc.a * kEncodeBit;
	        enc.a -= enc.a * kEncodeBit;
	        return enc;
	    }
	
	    public static float DecodeFloatRGBA(Color enc)
	    {
	        Color kDecodeDot = new Color(1.0f, 1 / 255.0f, 1 / 65025.0f, 1 / 160581375.0f);
							
			return Vector4.Dot(enc, kDecodeDot);
	    }
		
		public static Texture2D ImportTGA (string path) {
			try{
				System.IO.FileStream fs = new System.IO.FileStream (path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
		        System.IO.BinaryReader reader = new System.IO.BinaryReader (fs);
//				idLength
				reader.ReadByte ();
//				colormapType
				reader.ReadByte ();
//				imageTypeCode
				reader.ReadByte ();
//				colormapOrigin
				reader.ReadInt16 ();
//				colormapLength
				reader.ReadInt16 ();
//				colormapDepth
				reader.ReadByte ();
//				originX
				reader.ReadInt16 ();
//				originY
				reader.ReadInt16 ();
				Int16 width = reader.ReadInt16 ();
				Int16 height = reader.ReadInt16();
				byte pixelDepth = reader.ReadByte();
//				image descriptor
				reader.ReadByte ();
				Texture2D texture = new Texture2D(width, height, pixelDepth == 32 ? TextureFormat.ARGB32 : TextureFormat.RGB24, true);
				Color32[] colors = new Color32[width*height];
				
				for(int y=0; y<height; y++){
					for(int x=0; x<width; x++){
						if(pixelDepth == 32){
							byte blue = reader.ReadByte ();	
							byte green = reader.ReadByte ();
							byte red = reader.ReadByte ();
							byte alpha = reader.ReadByte ();
							colors[x + y * width] = new Color32(red, green, blue, alpha);
						}else{
							byte blue = reader.ReadByte ();	
							byte green = reader.ReadByte ();
							byte red = reader.ReadByte ();
							colors[x + y * width] = new Color32(red, green, blue, 1);
						}
					}
				}
				texture.SetPixels32 (colors);
				texture.Apply ();
				return texture;
			}catch{
				return null;	
			}
		}
		
		public static byte[] EncodeToTGA (Texture2D texture){
			List<byte> bytes = new List<byte>();
			bytes.Add (0); // idLength
			bytes.Add (0); // colormapType
			bytes.Add (2); // unmapped RGB
			bytes.AddRange (BitConverter.GetBytes ((Int16)0)); // colormapOrigin
			bytes.AddRange (BitConverter.GetBytes ((Int16)0)); // colormapLength
			bytes.Add (0); // colormapDepth
			bytes.AddRange (BitConverter.GetBytes ((Int16)0)); // originX
			bytes.AddRange (BitConverter.GetBytes ((Int16)0)); // originY
			bytes.AddRange (BitConverter.GetBytes ((Int16)texture.width)); // width
			bytes.AddRange (BitConverter.GetBytes ((Int16)texture.height)); // height
			Int16 pixelDepth = 0;
			switch(texture.format){
			case TextureFormat.ARGB32:
				pixelDepth = 32;
				break;
			case TextureFormat.RGB24:
				pixelDepth = 24;
				break;
			}
			bytes.AddRange (BitConverter.GetBytes (pixelDepth));
//			imageDescriptor
			switch(pixelDepth){
			case 24:
				bytes.Add (0);
				break;
			case 32:
				bytes.Add (8);
				break;
			}
			Color32[] colors = texture.GetPixels32 ();
			for(int y=0; y<texture.height; y++){
				for(int x=0; x<texture.width; x++){
					bytes.Add (colors[x + y * texture.width].g);
					bytes.Add (colors[x + y * texture.width].r);
					if(pixelDepth == 32){
						bytes.Add (colors[x + y * texture.width].a);
					}
					bytes.Add (colors[x + y * texture.width].b);
				}
			}
			return bytes.ToArray();
		}
	}
}