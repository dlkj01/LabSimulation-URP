/// <summary>
/// Add by zyq
/// </summary>

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DLKJ
{

    public class EventManager : MonoBehaviour
    {
        public delegate void OnTipsHandler(TipsType type, string content, UnityAction noCallback = null, UnityAction yesCallback = null);
        public static event OnTipsHandler OnTipsEvent;
        public static void OnTips(TipsType type, string content, UnityAction noCallback = null, UnityAction yesCallback = null)
        {
            if (OnTipsEvent != null)
            {
                OnTipsEvent(type,content, noCallback,yesCallback);
            }
        }

        public delegate void OnTipsDecidedBHandler();
        public static event OnTipsDecidedBHandler OnTipsDecidedEvent;
        public static void OnTipsDecided()
        {
            if (OnTipsDecidedEvent != null)
            {
                OnTipsDecidedEvent();
            }
        }


        public delegate void ReloadDBHandler();
        public static event ReloadDBHandler OnReloadDBEvent;
        public static void ReloadDBEvent()
        {
            if (OnReloadDBEvent != null)
            {
                OnReloadDBEvent();
            }
        }

        public delegate void OnMouseEnterItemHandler(Item item);
        public static event OnMouseEnterItemHandler OnMouseEnterItemEvent;
        public static void OnMouseEnterItem(Item item)
        {
            if (OnMouseEnterItemEvent != null)
            {
                OnMouseEnterItemEvent(item);
            }
        }

        public delegate void OnScrollItemHandler(UIItem uIItem);
        public static event OnScrollItemHandler OnScrollItemEvent;
        public static void OnScrollItem(UIItem uIItem)
        {
            if (OnScrollItemEvent != null)
            {
                OnScrollItemEvent(uIItem);
            }
        }

        public delegate void OnSelectedItemHandler(UIItem uIItem);
        public static event OnSelectedItemHandler OnSelectedItemEvent;
        public static void OnSelectedItem(UIItem uIItem)
        {
            if (OnSelectedItemEvent != null)
            {
                OnSelectedItemEvent(uIItem);
            }
        }

        public delegate void OnUsingItemsHandler(List<Item> items);
        public static event OnUsingItemsHandler OnUsingItemsEvent;
        public static void OnUsingItems(List<Item> items)
        {
            if (OnUsingItemsEvent != null)
            {
                OnUsingItemsEvent(items);
            }
        }

        public delegate void OnAttractItemHandler(Vector3 position);
        public static event OnAttractItemHandler OnAttractItemsEvent;
        public static void OnAttractItem(Vector3 position)
        {
            if (OnAttractItemsEvent != null)
            {
                OnAttractItemsEvent(position);
            }
        }

        public delegate void OnDetectionHandler(int targetItemID, int targetPortsID,bool isTrigger);
        public static event OnDetectionHandler OnDetectionEvent;
        public static void OnDetection(int targetItemID, int targetPortsID, bool isTrigger)
        {
            if (OnDetectionEvent != null)
            {
                OnDetectionEvent(targetItemID, targetPortsID, isTrigger);
            }
        }

        public delegate void OnLinkNextHandler(List<Item> needToConnect);
        public static event OnLinkNextHandler OnLinkNextEvent;
        public static void OnLinkNext(List<Item> needToConnect)
        {
            if (OnLinkNextEvent != null)
            {
                OnLinkNextEvent(needToConnect);
            }
        }

    }
}
