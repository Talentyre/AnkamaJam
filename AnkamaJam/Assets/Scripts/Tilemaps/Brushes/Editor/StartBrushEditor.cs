using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StartBrush))]
public class StartBrushEditor : GridBrushEditorBase {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void OnPaintInspectorGUI()
	{
		StartBrush myTarget = (StartBrush)target;
		myTarget.IsStart = GUILayout.Toggle(myTarget.IsStart, "Start");
	}
}
