#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects, CustomEditor(typeof(MonoBehaviour), true)] // Target all MonoBehaviours and descendants
public class ShowMethodDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Type type;
        Attribute attribute;

        foreach (object myTarget in targets)
        {
            type = myTarget.GetType();

            //Iterate over each private or public instance method, no static methods
            foreach (MethodInfo method in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
            {
                //Check it is decorated by attribute
                attribute = method.GetCustomAttribute(typeof(ShowMethodAttribute), true);

                if (attribute != null)
                {
                    if (GUILayout.Button(method.Name))
                        method.Invoke((MonoBehaviour)target, null);
                }
            }   
        }
    }
}
#endif