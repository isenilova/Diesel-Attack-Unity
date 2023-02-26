//----------------------------------------------
// Flowmap Generator
// Copyright © 2013 Superposition Games
// http://www.superpositiongames.com
// support@superpositiongames.com
//----------------------------------------------
using UnityEngine;
using System.Collections;
using Flowmap;

[AddComponentMenu("Flowmaps/Heightmap/Texture")]
public class FlowTextureHeightmap : FlowHeightmap {

	[SerializeField]
	Texture2D heightmap;
	public bool isRaw;
	Texture2D rawPreview;
	
	public override Texture HeightmapTexture {
		get {
			return heightmap;
		}
		set{
			heightmap = value as Texture2D;	
		}
	}
	
	public override Texture PreviewHeightmapTexture {
		get {
			if(isRaw){
				if(rawPreview == null){
					GenerateRawPreview ();
				}
				return rawPreview;
			}else{
				return HeightmapTexture;
			}
		}
	}
	
	public void GenerateRawPreview (){
		if(rawPreview){
			DestroyImmediate (rawPreview);	
		}
		if(heightmap){
			rawPreview = TextureUtilities.GetRawPreviewTexture (heightmap);
		}
	}
	
	protected override void OnDestroy (){
		base.OnDestroy ();
		if(rawPreview){
			DestroyImmediate (rawPreview);	
		}	
	}
}
