using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Naninovel
{
    public partial class TransitionalSpriteRenderer
    {
        public void UpdateRenderer()
        {
            SpriteMaterial.SetFloat(opacityId, TintColor.a);
            PrepareRenderTexture();
            RenderToTexture(renderTexture);
            var matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
            Graphics.DrawMesh(spriteMesh, matrix, SpriteMaterial, gameObject.layer);
            if (DepthPassEnabled) Graphics.DrawMesh(spriteMesh, matrix, DepthMaterial, gameObject.layer);
        }
    }
}

