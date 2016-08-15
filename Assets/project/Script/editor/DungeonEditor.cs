using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(RoomGenerationManager))]
public class DungeonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
		RoomGenerationManager myScript = (RoomGenerationManager)target;
        if(GUILayout.Button("Build Dungeon")) {
			myScript.GenerateDungeon();
        }

    }
}
