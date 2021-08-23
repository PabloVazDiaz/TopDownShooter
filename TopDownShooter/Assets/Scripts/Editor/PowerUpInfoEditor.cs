
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PowerUpInfo))]
public class PowerUpInfoEditor : Editor
{

    int selected;
    //List<PowerableAttribute> powerableAttributes = new List<PowerableAttribute>();
    List<MemberInfo> powerableAttributes = new List<MemberInfo>();
    List<string> testOptions = new List<string>();
    
    
    
    
    
    private void OnEnable()
    {


        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (Assembly assembly in assemblies)
        {
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
                MemberInfo[] members = type.GetMembers(flags);
                foreach (MemberInfo member in members)
                {
                    if(member.CustomAttributes.ToArray().Length > 0)
                    {
                        PowerableAttribute attribute = member.GetCustomAttribute<PowerableAttribute>();
                        if (attribute != null)
                        {
                            powerableAttributes.Add(member);
                        }
                    }

                }

            }
        }

        foreach (MemberInfo info in powerableAttributes)
        {
            testOptions.Add(info.Name);
            //Debug.Log( info.ReflectedType);
            FieldInfo FInfo = (FieldInfo)info;
            
            //Debug.Log( FInfo.GetValue(info));

        }

    }


    public override void OnInspectorGUI()
    {
        
        base.OnInspectorGUI();
        PowerUpInfo powerUpInfo = target as PowerUpInfo;
        

        selected = EditorGUILayout.Popup("Attribute to change", selected , testOptions.ToArray());
        
        Debug.Log($"Size:{powerableAttributes.Count} selected: {selected}");
        

        foreach (MemberInfo info in powerableAttributes)
        {
            testOptions.Add(info.Name);
            FieldInfo FInfo = (FieldInfo)info;

            //powerUpInfo.fieldInfo = FInfo;
            //Debug.Log(FInfo.FieldType.BaseType);
           // Debug.Log(FInfo.GetValue(FindObjectOfType(info.ReflectedType)));

        }
        if(GUILayout.Button("Set Parameters"))
        {
            SetParameters(powerUpInfo);
        }
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("ReflectedType");
        EditorGUILayout.LabelField(powerableAttributes[selected].ReflectedType.Name);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("SavedType");
        //EditorGUILayout.LabelField(powerUpInfo.ScriptToFind?.Name);
        GUILayout.EndHorizontal();
    }

    private void SetParameters(PowerUpInfo powerUpInfo)
    {
        powerUpInfo.AttributeToChange = testOptions[selected];
        powerUpInfo.attributeToChange = powerableAttributes[selected].GetCustomAttribute<PowerableAttribute>();
       // powerUpInfo.ScriptToFind = powerableAttributes[selected].ReflectedType;
        Debug.Log(powerableAttributes[selected].ReflectedType);
        EditorUtility.SetDirty(target);
        powerUpInfo.SetDirty();
        AssetDatabase.SaveAssets();
    }
}
