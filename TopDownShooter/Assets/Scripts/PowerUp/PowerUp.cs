using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    
    public float previousValue; 
    public PowerUpInfo powerUpInfo;
    

    public GameObject player;
    Type ScriptToAccess;
    FieldInfo fieldInfo;
    UIManager uiManager;

    float startTime;
    private void Awake()
    {
        player = FindObjectOfType<PlayerMover>().gameObject;
    }
    private void Start()
    {
        
        uiManager = FindObjectOfType<UIManager>();

        BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
        Component[] playerComponents = player.GetComponents(typeof(MonoBehaviour));
        
        foreach (Component component in playerComponents)
        {
                Debug.Log($"component: {component.GetType()}");
            MemberInfo member = component.GetType().GetMembers(flags).FirstOrDefault(x => x.Name.Equals(powerUpInfo.AttributeToChange));
            ScriptToAccess = member?.ReflectedType;
            if (ScriptToAccess != null)
            {
                fieldInfo = (FieldInfo)member;
                previousValue = (float)fieldInfo.GetValue(player.GetComponent(ScriptToAccess));
                break;
            }
        }

        /*
        MemberInfo[] members =  GetMembers(flags);
        foreach (MemberInfo member in members)
        {
            if (member.Name == powerUpInfo.testString)
            {
                Debug.Log($"Member: {member.Name}");
                ScriptToAccess = member.ReflectedType;
                Debug.Log(member.ReflectedType);

                previousValue = (float)fieldInfo.GetValue(player.GetComponent(ScriptToAccess));
            }
          
        }*/


    }
    private void Update()
    {
        
        if (startTime > 0)
        {
            uiManager.showPowerUp(powerUpInfo.AttributeToChange, (powerUpInfo.duration - (Time.time - startTime))/ powerUpInfo.duration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            startTime = Time.time;
            //previousValue = (float)powerUpInfo.fieldInfo.GetValue(instanceObject);
            fieldInfo.SetValue(player.GetComponent(ScriptToAccess), powerUpInfo.newValue);
            GetComponent<Collider>().enabled = false;
            GetComponentInChildren<MeshRenderer>().enabled = false;
            StartCoroutine(returnToNormal());
        }
    }

    IEnumerator returnToNormal()
    {
        yield return new WaitForSeconds(powerUpInfo.duration);
        fieldInfo.SetValue(player.GetComponent(ScriptToAccess), previousValue);

    }

}
