using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(zombieAI))]
public class zombieFOVinEditor : Editor
{
    //taken from a youtube tutorial find it here : https://www.youtube.com/watch?v=j1-OyLo77ss
    public void OnSceneGUI()
    {
        zombieAI zombieai = (zombieAI)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(zombieai.transform.position, Vector3.up, Vector3.forward, 360, zombieai.FOVradius);

        Vector3 viewAngle01 = dirFromAngle(zombieai.transform.eulerAngles.y, -zombieai.FOV / 2);
        Vector3 viewAngle02 = dirFromAngle(zombieai.transform.eulerAngles.y, zombieai.FOV / 2);

        Handles.color = Color.cyan;
        Handles.DrawLine(zombieai.transform.position, zombieai.transform.position + viewAngle01 * zombieai.FOVradius);
        Handles.DrawLine(zombieai.transform.position, zombieai.transform.position + viewAngle02 * zombieai.FOVradius);

    }

    private Vector3 dirFromAngle(float eulerY , float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
