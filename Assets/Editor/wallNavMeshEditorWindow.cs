using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class wallNavMeshEditorWindow : EditorWindow
{
    GameObject wallNav , navMeshLink , fallTrigger;
    // Start is called before the first frame update
    
    [MenuItem("Tools/Level Tools")]
    public static void ShowWindow()
    {
        GetWindow(typeof(wallNavMeshEditorWindow));
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Tools", EditorStyles.boldLabel);

        wallNav = EditorGUILayout.ObjectField("Wall Nav Mesh",wallNav , typeof(GameObject),false) as GameObject;
        if(GUILayout.Button("Create wall NavMesh"))
        {
            createWallNavMesh();
        }

        navMeshLink = EditorGUILayout.ObjectField("NavMeshLink", navMeshLink, typeof(GameObject), false) as GameObject;
        if (GUILayout.Button("Create navMeshLink"))
        {
            createNavMeshLink();
        }

        fallTrigger = EditorGUILayout.ObjectField("Fall Trigger", fallTrigger, typeof(GameObject), false) as GameObject;
        if (GUILayout.Button("Create fallTrigger+Link"))
        {
            createFallTrigger();
        }
    }

    private void createWallNavMesh()
    {
      
            GameObject newObject = Instantiate(wallNav, Selection.activeTransform.position, Quaternion.identity);
        
        
    }

    private void createNavMeshLink()
    {
       
            GameObject newObject = Instantiate(navMeshLink,Selection.activeTransform.position, Quaternion.identity);
       

    }
    private void createFallTrigger()
    {

     
            GameObject newObject = Instantiate(fallTrigger, Selection.activeTransform.position, Quaternion.identity);
        

    }
}
