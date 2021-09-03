using DLKJ;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ItemElement : MonoBehaviour
{
    public enum SubUnitType { port, knob, lever }
    public Transform subUnit;
    public SubUnitType subUnitType;

    public List<Transform> combinationLinkageList = new List<Transform>();

    public delegate void CombinationLinkageActionHandler(List<Transform> combinationLinkageList);

    public event CombinationLinkageActionHandler CombinationLinkageActionEvent;

    public void CombinationLinkageAction(List<Transform> combinationLinkageList)
    {
        CombinationLinkageActionEvent(combinationLinkageList);
    }

    public delegate void OnSubUnitTriggerHandler(Transform colliderTransform);

    public event OnSubUnitTriggerHandler OnSubUnitTriggerEvent;

    public void OnSubUnitTrigger(Transform colliderTransform)
    {
        OnSubUnitTriggerEvent(colliderTransform);
    }

    private void Awake()
    {
        CombinationLinkageActionEvent += CombineFunc;
        OnSubUnitTriggerEvent += SubUnitTrigger;
    }

    private void SubUnitTrigger(Transform colliderTransform)
    {
        if (!colliderTransform.GetComponent<Item>() && colliderTransform.name != "Desk")
        {

        }
    }
    private void CombineFunc(List<Transform> combinationLinkageList)
    {
        switch (subUnitType)
        {
            case SubUnitType.port:
                PortCombination(combinationLinkageList);
                break;
            case SubUnitType.knob:
                KnobCombination(combinationLinkageList);
                break;
            case SubUnitType.lever:
                LeverCombination(combinationLinkageList);
                break;
            default:
                break;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("other Stay");
    }

    private void OnTriggerEnter(Collider other)
    {
        ItemElement itemElement = other.GetComponent<ItemElement>();
        if (itemElement)
        {
            if (transform.root.GetComponent<Rigidbody>())
                Debug.Log("Port And Port" + transform.name + "Other" + other.name);
            
        }

    }

    private void PortCombination(List<Transform> combinationLinkageList)
    {

    }

    private void KnobCombination(List<Transform> combinationLinkageList)
    {

    }

    private void LeverCombination(List<Transform> combinationLinkageList)
    {

    }



}
