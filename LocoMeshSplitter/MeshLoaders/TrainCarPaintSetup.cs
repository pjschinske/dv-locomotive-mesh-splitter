using DV.Customization.Paint;
using System;
using System.Collections.Generic;
using System.Text;
using static DV.Customization.Paint.TrainCarPaint;
using UnityEngine;
using HarmonyLib;
using System.Linq;

namespace LocoMeshSplitter.MeshLoaders
{
	internal class TrainCarPaintSetup
	{
		/*
		 * NOTES on Skin Manager compatibility:
		 * As far as I can tell, we're still completely compatible with Skin Manager.
		 * Skin Manager has a bug in B99 where the livery overrides whatever skin you
		 * select.
		 * 
		 * I think we can just wait and figure this out when Skin Manager updates to B99.
		 * Presumably Skin Manager will use the new livery system and this won't be a problem.
		 * 
		 */

		internal static void SetupTrainCarPaintRenderers(TrainCarPaint tcp, Transform t)
		{
			if (t is null)
			{
				return;
			}
			List<RendererMaterialReplacement> rmr;
			Renderer[] renderers = t.GetComponentsInChildren<Renderer>(true);
			//Each set holds a group of renderers for a certain texture
			foreach (MaterialSet materialSet in tcp.sets)
			{
				Material originalMaterial = materialSet.renderers[0].renderer.sharedMaterial;
				rmr = materialSet.renderers.ToList();
				//Each renderer can hold multiple materials (color, normals, etc)
				foreach (Renderer renderer in renderers)
				{
					Material[] materials = renderer.sharedMaterials;
					for (int i = 0; i < materials.Length; i++)
					{
						if (materials[i] == originalMaterial)
						{
							rmr.Add(new RendererMaterialReplacement(renderer, i));
						}
					}
				}
				materialSet.renderers = rmr.ToArray();
			}
		}

		internal static void SetupTrainCarPaintRenderers(TrainCarPaint tcp)
		{
			List<RendererMaterialReplacement> rmr = new();
			Renderer[] renderers = tcp.GetComponentsInChildren<Renderer>(true);
			//Each set holds a group of renderers for a certain texture
			foreach (MaterialSet materialSet in tcp.sets)
			{
				Material originalMaterial = materialSet.renderers[0].renderer.sharedMaterial;
				rmr.Clear();
				//Each renderer can hold multiple materials (color, normals, etc)
				foreach (Renderer renderer in renderers)
				{
					Material[] materials = renderer.sharedMaterials;
					for (int i = 0; i < materials.Length; i++)
					{
						if (materials[i] == originalMaterial)
						{
							rmr.Add(new RendererMaterialReplacement(renderer, i));
						}
					}
				}
				materialSet.renderers = rmr.ToArray();
			}
		}
	}

}
