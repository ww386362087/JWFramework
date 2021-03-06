﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace JWFramework.UGUI
{
	[AddComponentMenu ("UI/Effects/Gradient")]
	public class Gradient : BaseMeshEffect
	{
		[SerializeField]
		private Color32 topColor = Color.white;
		[SerializeField]
		private Color32 bottomColor = Color.black;
		[SerializeField]
		private bool useGraphicAlpha = true;

		public override void ModifyMesh (VertexHelper vh)
		{
			if (!IsActive () || vh.currentVertCount == 0) {
				return;
			}

			var count = vh.currentVertCount;

			var vertexs = new List<UIVertex> ();
			for (var i = 0; i < count; i++) {
				var vertex = new UIVertex ();
				vh.PopulateUIVertex (ref vertex, i);
				vertexs.Add (vertex);
			}

			var topY = vertexs [0].position.y;
			var bottomY = vertexs [0].position.y;

			for (var i = 1; i < count; i++) {
				var y = vertexs [i].position.y;
				if (y > topY) {
					topY = y;
				} else if (y < bottomY) {
					bottomY = y;
				}
			}

			var height = topY - bottomY;
			for (var i = 0; i < count; i++) {
				var vertex = vertexs [i];

				Color color = Color32.Lerp (bottomColor, topColor, (vertex.position.y - bottomY) / height);
				if (useGraphicAlpha) {
					color.a = color.a * ((int)vertex.color.a / 255f);
				}
				
				vertex.color = color;

				vh.SetUIVertex (vertex, i);
			}
		}
	}
}