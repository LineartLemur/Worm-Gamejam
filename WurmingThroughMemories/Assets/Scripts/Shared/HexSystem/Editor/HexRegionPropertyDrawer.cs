using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using HexSystem;
using UnityEditor;

[CustomPropertyDrawer(typeof(HexRegion))]
public class HexRegionPropertyDrawer : PropertyDrawer {
	
	private static Texture2D _staticRectTexture;
	private static GUIStyle _staticRectStyle;

	private float textHeight = (EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight);
	
	private float HexHeight = 26;
	private float HexDistance = 30;
	
	private int range = 3;
	private int minRange;
	private List<Hex> copyOfHexes = new List<Hex>();
	private List<Hex> neighbores = new List<Hex>();

	private bool collapsed;

	private Rect GetLineRect(Rect propertyPosition, float verticalOffset) {
		return new Rect(propertyPosition.x, propertyPosition.y + verticalOffset, propertyPosition.width,
			EditorGUIUtility.singleLineHeight);
	}
	


	// Draw the property inside the given rect
	public override void OnGUI(Rect propertyPosition, SerializedProperty property, GUIContent label) {
		float v = 0;
		
		collapsed = !EditorGUI.Foldout(GetLineRect(propertyPosition,v),!collapsed, label);
		v += textHeight;
		if (collapsed) return;
		EditorGUI.indentLevel++;
		propertyPosition = EditorGUI.IndentedRect(propertyPosition);
		
		EditorGUI.indentLevel--;
		
		CopyHexesFromProperty(property.FindPropertyRelative("hexes"));
		
		bool changed = false;
		
		range =  Mathf.Max(minRange, EditorGUI.IntField(GetLineRect(propertyPosition,v),"Range", range));
		v += textHeight;
		
		Vector2 center = new Rect(propertyPosition.x, propertyPosition.y + v, propertyPosition.width,
			propertyPosition.height - v - EditorGUIUtility.standardVerticalSpacing ).center;

		//draw hexes
		Hex.Zero.GetNeighboursNonAlloc(neighbores, range);
		neighbores.Add(Hex.Zero);
		for (int i = 0; i < neighbores.Count; i++) {
			bool enabled = copyOfHexes.Contains(neighbores[i]);
			bool result = DrawHexGUI(center + (neighbores[i].GetLoc() * HexDistance).ChangeY(y=>-y), enabled);
			if (result ^ enabled) {
				changed = true;
				if (result) {
					copyOfHexes.Add(neighbores[i]);
				} else {
					copyOfHexes.Remove(neighbores[i]);
				}
			}
		}

		if (changed) {
			CopyHexesToProperty(property.FindPropertyRelative("hexes"));
		}

	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		return (collapsed)?textHeight: textHeight * 2 + HexDistance * 2 * range* Mathf.Cos(Mathf.Deg2Rad *30) + HexHeight ;
	}
	
	private void CopyHexesFromProperty(SerializedProperty property) {
		copyOfHexes.Clear();
		for (int i = 0; i < property.arraySize; i++) {
			var hexProperty = property.GetArrayElementAtIndex(i);
			copyOfHexes.Add( new Hex(
				hexProperty.FindPropertyRelative("q").intValue,
				hexProperty.FindPropertyRelative("r").intValue));
		}

		minRange = 1;
		if (copyOfHexes.Count > 0) {
			minRange = copyOfHexes.Select(hex => hex.magnitude).Max();
		}

		if (minRange < 1) minRange = 1;
	}
	
	private void CopyHexesToProperty(SerializedProperty property) {
		
		property.ClearArray();
		for (int i = 0; i < copyOfHexes.Count; i++) {
			property.InsertArrayElementAtIndex(0);
			property.GetArrayElementAtIndex(0).FindPropertyRelative("q").intValue = copyOfHexes[i].q;
			property.GetArrayElementAtIndex(0).FindPropertyRelative("r").intValue = copyOfHexes[i].r;
		}
	}

	private bool DrawHexGUI(Vector2 center, bool on) {
		
		if (_staticRectTexture == null ) {
			_staticRectTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("assets/InspectorResources/hexbg.png");
                    
			_staticRectStyle = new GUIStyle();
			_staticRectStyle.normal.background = _staticRectTexture;
		}
		
		Rect rectArroundCenter = new Rect(center.x - HexHeight/2, center.y -HexHeight/2, HexHeight,HexHeight);
		
		GUI.Label(rectArroundCenter, GUIContent.none, _staticRectStyle);
		
		
		Rect toggleRect = new Rect(center.x - 7, center.y -8, 15,15);
		
		return GUI.Toggle(toggleRect, on, GUIContent.none);
	}

	

	public override bool CanCacheInspectorGUI(SerializedProperty property) {
		return false;
	}
}
