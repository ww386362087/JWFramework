﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using JWFramework.Anim;

[CanEditMultipleObjects]
[CustomEditor (typeof(JWAnimTools))]
public class JWAnimToolsEditor : Editor
{
	private string deleteEventIndex = "0";
	private bool useUnifiedDuration = false;
	private float unifiedDuration = 0;

	public override void OnInspectorGUI ()
	{
		SerializedProperty anims = serializedObject.FindProperty ("anims");
		JWAnimTools uiAnimTools = target as JWAnimTools;
		GUILayout.Space (3f);

		serializedObject.Update ();

		uiAnimTools.AnimGroupName = EditorGUILayout.TextField ("Anim Name:", uiAnimTools.AnimGroupName, GUILayout.MinWidth (60f));
		if (anims.arraySize > 1) {
			useUnifiedDuration = EditorGUILayout.ToggleLeft ("Use Unified Duration", useUnifiedDuration);
			if (useUnifiedDuration) {
				unifiedDuration = EditorGUILayout.FloatField ("Anim Duration", unifiedDuration, GUILayout.MinWidth (80f));
			} else {
				JWAnimBase uiAnimBase = uiAnimTools.anims [0];
				unifiedDuration = uiAnimBase.duration;
			}
		}
		for (int i = 0, imax = anims.arraySize; i < imax; i++) {
			JWAnimBase uiAnimBase = uiAnimTools.anims [i];
			if (JWEditorTools.DrawHeader (string.Format ("Anim Event {0}: {1}", i, uiAnimBase.animType))) {
				JWEditorTools.BeginContents ();
				{
					uiAnimBase.animType = (UIAnimType)EditorGUILayout.EnumPopup ("Anim Type", uiAnimBase.animType);
					if (uiAnimBase.animType != UIAnimType.None) {
						if (JWEditorTools.DrawHeader ("Base Attribute")) {
							uiAnimBase.playMode = (UIAnimPlayMode)EditorGUILayout.EnumPopup ("Anim Play Mode", uiAnimBase.playMode);
							uiAnimBase.delay = EditorGUILayout.FloatField ("Start Delay", uiAnimBase.delay);
							if (useUnifiedDuration) {
								uiAnimBase.duration = unifiedDuration;
								GUILayout.BeginHorizontal ();
								EditorGUILayout.LabelField ("Anim Duration: ", GUILayout.MaxWidth (115f));
								EditorGUILayout.LabelField (uiAnimBase.duration.ToString ());
								GUILayout.EndHorizontal ();
							} else {
								uiAnimBase.duration = EditorGUILayout.FloatField ("Anim Duration", uiAnimBase.duration);
							}
						}
					}
					switch (uiAnimBase.animType) {
					case UIAnimType.Position:
						{
							if (JWEditorTools.DrawHeader ("Position Anim Attribute")) {
								uiAnimBase.beginVec3 = EditorGUILayout.Vector3Field ("Anim Begin Position", uiAnimBase.beginVec3);
								uiAnimBase.endVec3 = EditorGUILayout.Vector3Field ("Anim End Position", uiAnimBase.endVec3);
								uiAnimBase.curve = EditorGUILayout.CurveField ("Position Curve", uiAnimBase.curve);
							}
						}
						break;
					case UIAnimType.Euler:
						{
							if (JWEditorTools.DrawHeader ("Euler Anim Attribute")) {
								uiAnimBase.beginVec3 = EditorGUILayout.Vector3Field ("Anim Begin Euler", uiAnimBase.beginVec3);
								uiAnimBase.endVec3 = EditorGUILayout.Vector3Field ("Anim End Euler", uiAnimBase.endVec3);
								uiAnimBase.curve = EditorGUILayout.CurveField ("Euler Curve", uiAnimBase.curve);
							}
						}
						break;
					case UIAnimType.Scale:
						{
							if (JWEditorTools.DrawHeader ("Scale Anim Attribute")) {
								uiAnimBase.curve = EditorGUILayout.CurveField ("Scale Curve", uiAnimBase.curve);
								uiAnimBase.scaleOfX = EditorGUILayout.ToggleLeft ("Use to Scale X", uiAnimBase.scaleOfX);
								uiAnimBase.scaleOfY = EditorGUILayout.ToggleLeft ("Use to Scale Y", uiAnimBase.scaleOfY);
								uiAnimBase.scaleOfZ = EditorGUILayout.ToggleLeft ("Use to Scale Z", uiAnimBase.scaleOfZ);
							}
						}
						break;
					case UIAnimType.UGUI_Alpha:
						{
							if (JWEditorTools.DrawHeader ("Alpha Anim Attribute")) {
								if (uiAnimBase.alphaGroup == null) {
									uiAnimBase.alphaGroup = new System.Collections.Generic.List<CanvasGroup> ();
								}
								uiAnimBase.curve = EditorGUILayout.CurveField ("Alpha Curve", uiAnimBase.curve);
//								int alphaItemCount = EditorGUILayout.IntField ("Alpha Rect", uiAnimBase.alphaGroup.Count);
//								while (alphaItemCount != uiAnimBase.alphaGroup.Count) {
//									if (alphaItemCount > uiAnimBase.alphaGroup.Count) {
//										uiAnimBase.alphaGroup.Add (null);
//									} else {
//										uiAnimBase.alphaGroup.RemoveAt (uiAnimBase.alphaGroup.Count - 1);
//									}
//								}
//								for (int j = 0, jmax = uiAnimBase.alphaGroup.Count; j < jmax; j++) {
//									uiAnimBase.alphaGroup [j] = (CanvasGroup)EditorGUILayout.ObjectField ("    Element " + j, uiAnimBase.alphaGroup [j], typeof(CanvasGroup), true);
//								}
								EditorGUI.indentLevel += 1;
								EditorGUILayout.PropertyField (anims.GetArrayElementAtIndex (i).FindPropertyRelative ("alphaGroup"), new GUIContent ("Alpha Rect"), true);
								EditorGUI.indentLevel -= 1;
							}
						}
						break;
					case UIAnimType.UGUI_ImageFilled:
						{
							if (JWEditorTools.DrawHeader ("Image Filled Anim Attribute")) {
								uiAnimBase.curve = EditorGUILayout.CurveField ("Image Filled Curve", uiAnimBase.curve);
								uiAnimBase.image = (UnityEngine.UI.Image)EditorGUILayout.ObjectField ("Target Image ", uiAnimBase.image, typeof(UnityEngine.UI.Image), true);
							}
						}
						break;
					case UIAnimType.UGUI_Color:
						{
							if (JWEditorTools.DrawHeader ("Color Anim Attribute")) {
								if (uiAnimBase.colorGroup == null) {
									uiAnimBase.colorGroup = new System.Collections.Generic.List<UnityEngine.UI.Graphic> ();
								}
								uiAnimBase.curve = EditorGUILayout.CurveField ("Color Curve", uiAnimBase.curve);
								uiAnimBase.beginColor = EditorGUILayout.ColorField ("Begin Color", uiAnimBase.beginColor);
								uiAnimBase.endColor = EditorGUILayout.ColorField ("End Color", uiAnimBase.endColor);
//								int colorItemCount = EditorGUILayout.IntField ("Color Rect", uiAnimBase.colorGroup.Count);
//								while (colorItemCount != uiAnimBase.colorGroup.Count) {
//									if (colorItemCount > uiAnimBase.colorGroup.Count) {
//										uiAnimBase.colorGroup.Add (null);
//									} else {
//										uiAnimBase.colorGroup.RemoveAt (uiAnimBase.colorGroup.Count - 1);
//									}
//								}
//								for (int j = 0, jmax = uiAnimBase.colorGroup.Count; j < jmax; j++) {
//									uiAnimBase.colorGroup [j] = (UnityEngine.UI.Graphic)EditorGUILayout.ObjectField ("    Element " + j, uiAnimBase.colorGroup [j], typeof(UnityEngine.UI.Graphic), true);
//								}
								EditorGUI.indentLevel += 1;
								EditorGUILayout.PropertyField (anims.GetArrayElementAtIndex (i).FindPropertyRelative ("colorGroup"), new GUIContent ("Color Rect"), true);
								EditorGUI.indentLevel -= 1;
							}
						}
						break;
					}
				}
				JWEditorTools.EndContents ();
			}
		}

		GUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Add Event")) {
			if (uiAnimTools.anims == null) {
				uiAnimTools.anims = new System.Collections.Generic.List<JWAnimBase> ();
			}
			uiAnimTools.anims.Add (new JWAnimBase ());
		}
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Delect Last Event")) {
			uiAnimTools.anims.RemoveAt (uiAnimTools.anims.Count - 1);
		}
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Del Anim Event ")) {
			try {
				int index = int.Parse (deleteEventIndex);
				anims.DeleteArrayElementAtIndex (index);
			} catch {
			}
		}
		EditorGUILayout.LabelField ("Index:", GUILayout.MaxWidth (60f));
		deleteEventIndex = EditorGUILayout.TextField (deleteEventIndex, GUILayout.MaxWidth (80f));
		GUILayout.EndHorizontal ();

		int tryId = 0;
		bool parseSuc = int.TryParse (deleteEventIndex, out tryId);
		if (!parseSuc) {
			EditorGUILayout.HelpBox ("Error Index", MessageType.Error, true);
		}

		serializedObject.ApplyModifiedProperties ();
	}
}
