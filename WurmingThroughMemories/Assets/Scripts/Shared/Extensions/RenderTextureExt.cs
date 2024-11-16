using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using Object = UnityEngine.Object;

public static class RenderTextureExt {
	private static DefaultDictionary<RenderTexture, Texture2D> _cachedTextures =
		new DefaultDictionary<RenderTexture, Texture2D>();

	public static Texture2D Screenshot(this Camera cam, int width, int height) {
		Debug.Log($"screenshotting with res {width}x{height}");
		var rt =new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32 );
		var prev = cam.targetTexture;
		RenderTexture.active = cam.targetTexture = rt;
		cam.Render();
		RenderTexture.active = null;
		cam.targetTexture = prev;
		var texture = rt.ToTexture2D();
		Object.Destroy(rt);

		return texture;
	}
	public static Texture2D MirrorToTexture2D(this Texture tex) {

		var RT = new RenderTexture(tex.width, tex.height, 0, RenderTextureFormat.ARGB32 );
		Graphics.Blit(tex, RT, new Vector2(-1,1), new Vector2(1,0));
		RenderTexture.active = null;
		var t2D = RtTo2D(RT);
		Object.Destroy(RT);

		return t2D;
	}

	public static Texture2D ToTexture2D(this Texture tex, TextureFormat tf = TextureFormat.RGBA32) {
		if (tex is Texture2D t2D)
			return t2D;

		if (tex is RenderTexture rt)
			return RtTo2D(rt,tf);

		rt = ToRenderTexture(tex);
		t2D = RtTo2D(rt,tf);
		Object.Destroy(rt);
		return t2D;
	}

	private static Texture2D RtTo2D(RenderTexture renderTexture, TextureFormat tf = TextureFormat.RGBA32) {

		if (renderTexture.format != RenderTextureFormat.ARGB32)
			throw new ArgumentException("The format for the render texture must be ARGB32.");

		var prevRt = RenderTexture.active;
		var tex = new Texture2D(renderTexture.width, renderTexture.height, tf, false);

		RenderTexture.active = renderTexture;
		tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
		tex.Apply();
		RenderTexture.active = prevRt;
		return tex;
	}

	public static RenderTexture ToRenderTexture(this Texture tex) {
		var RT = new RenderTexture(tex.width, tex.height, 0, RenderTextureFormat.ARGB32 );
		Graphics.Blit(tex, RT);
		RenderTexture.active = null;
		return RT;
	}

	public static void SaveAs(this Texture texture, string fileName) {
		Debug.Log("saving rendertexture to file");
		var tex = texture.ToTexture2D();

		// save the texture
		byte[] bytes = null;

		if (fileName.EndsWith("png"))
			bytes = tex.EncodeToPNG();
		else if (fileName.EndsWith("jpg") || fileName.EndsWith("jpeg"))
			bytes = tex.EncodeToJPG(90);
		else
			throw new ArgumentException("File DisplayName with extension " + fileName +
			                            " not supported. Please use png or jpg.");

		string dir = Path.GetDirectoryName(fileName);

		if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
			Directory.CreateDirectory(dir);

		File.WriteAllBytes(fileName, bytes);

		//GameObject.Destroy(tex);
	}
}
