using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DLKJ
{
    public enum ViewType
    {
        LabView,
        DeviceView,
        EquipmentView,
    }

    public class UIEquipmentPanel : MonoBehaviour
    {
        [SerializeField] private UIItem uiItemPrefab;

        [HeaderAttribute("设备库")]
        [SerializeField] private GameObject e_view;
        [SerializeField] private RectTransform e_scrollRect;
        [SerializeField] private RectTransform e_content;
        [SerializeField] private Scrollbar e_scrollbar;
        [SerializeField] private GridLayoutGroup e_layoutGroup;

        [HeaderAttribute("器件库")]
        [SerializeField] private GameObject d_view;
        [SerializeField] private RectTransform d_scrollRect;
        [SerializeField] private RectTransform d_content;
        [SerializeField] private Scrollbar d_scrollbar;
        [SerializeField] private GridLayoutGroup d_layoutGroup;

        [HeaderAttribute("实验器材库")]
        [SerializeField] private GameObject t_view;
        [SerializeField] private RectTransform t_scrollRect;
        [SerializeField] private RectTransform t_content;
        [SerializeField] private Scrollbar t_scrollbar;
        [SerializeField] private GridLayoutGroup t_layoutGroup;

        [HeaderAttribute("Compents")]
        [SerializeField] private Button sureButton;

        private List<Item> equipmentItems;
        private List<Item> deviceItems;
        private List<Item> labItems = new List<Item>();
        [HideInInspector] public List<UIItem> uIItems = new List<UIItem>();

        private bool scoreChecking = true;
        private Lab currentLab;

        private void Start()
        {
            sureButton.onClick.AddListener(delegate { OnSureCallBack(); });
        }

        private void OnEnable()
        {
            EventManager.OnSelectedItemEvent += OnSelectedItem;
        }

        private void OnDisable()
        {
            EventManager.OnSelectedItemEvent -= OnSelectedItem;
        }

        public void Initialized(int labID)
        {
            LabDB labDB = DBManager.GetInstance().GetDB<LabDB>();
            ItemDB itemDB = DBManager.GetInstance().GetDB<ItemDB>();

            currentLab = labDB.GetLabByID(labID).Clone();
            currentLab.Initialized();
            SceneManager.GetInstance().currentLab = currentLab;

            equipmentItems = itemDB.GetItemsByLibraryType(LibraryType.Equipment);
            deviceItems = itemDB.GetItemsByLibraryType(LibraryType.Device);
            deviceItems.AddRange(itemDB.GetItemsByLibraryType(LibraryType.Wires));

            RefreshEquipmentView();
            InitCameraView(equipmentItems, ViewType.EquipmentView, e_content);
            //for (int i = 0; i < equipmentItems.Count; i++)
            //{
            //    UIItem uIItem = Instantiate(uiItemPrefab, e_content.transform) as UIItem;
            //    uIItem.Initialized(equipmentItems[i]);
            //    uIItem.SetView(ViewType.EquipmentView);
            //    uIItems.Add(uIItem);
            //}
            e_scrollbar.value = 0;


            RefreshDeviceView();
            InitCameraView(deviceItems, ViewType.DeviceView, d_content);
            //for (int i = 0; i < deviceItems.Count; i++)
            //{
            //    UIItem uIItem = Instantiate(uiItemPrefab, d_content.transform) as UIItem;
            //    uIItem.Initialized(deviceItems[i]);
            //    uIItem.SetView(ViewType.DeviceView);
            //    uIItems.Add(uIItem);
            //}
            d_scrollbar.value = 0;
        }

        void InitCameraView(List<Item> itemList, ViewType type, RectTransform parent)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                UIItem uIItem = Instantiate(uiItemPrefab, parent) as UIItem;
                uIItem.Initialized(itemList[i]);
                uIItem.SetView(type);
                uIItems.Add(uIItem);
            }
        }

        void RefreshDeviceView()
        {
            float d_Height = deviceItems.Count * (d_layoutGroup.cellSize.y + e_layoutGroup.spacing.y) + e_layoutGroup.padding.top + e_layoutGroup.padding.bottom - e_layoutGroup.spacing.y;
            if (d_Height > d_scrollRect.rect.height)
            {
                d_content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, d_Height);
            }
            else
            {
                d_content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, d_scrollRect.rect.height);
            }
        }

        void RefreshEquipmentView()
        {
            float e_Height = equipmentItems.Count * (e_layoutGroup.cellSize.y + e_layoutGroup.spacing.y) + e_layoutGroup.padding.top + e_layoutGroup.padding.bottom - e_layoutGroup.spacing.y;
            if (e_Height > e_scrollRect.rect.height)
            {
                e_content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, e_Height);
            }
            else
            {
                e_content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, e_scrollRect.rect.height);
            }
        }

        void OnSelectedItem(UIItem uIItem)
        {
            switch (uIItem.viewType)
            {
                case ViewType.LabView:
                    {
                        if (uIItem.item.libraryType == LibraryType.Equipment)
                        {
                            equipmentItems.Add(uIItem.item);
                            labItems.Remove(uIItem.item);
                            RefreshEquipmentView();
                            UpdateLabView();
                            uIItem.SetView(ViewType.EquipmentView);
                            uIItem.transform.SetParent(e_content.transform);
                        }
                        else if (uIItem.item.libraryType == LibraryType.Device || uIItem.item.libraryType == LibraryType.Wires)
                        {
                            deviceItems.Add(uIItem.item);
                            labItems.Remove(uIItem.item);
                            RefreshDeviceView();
                            uIItem.SetView(ViewType.DeviceView);
                            uIItem.transform.SetParent(d_content.transform);
                        }
                    }
                    break;
                case ViewType.DeviceView:
                    {
                        labItems.Add(uIItem.item);
                        deviceItems.Remove(uIItem.item);
                        uIItem.SetView(ViewType.LabView);
                        RefreshDeviceView();
                        UpdateLabView();
                        uIItem.transform.SetParent(t_content.transform);
                    }
                    break;
                case ViewType.EquipmentView:
                    {
                        labItems.Add(uIItem.item);
                        equipmentItems.Remove(uIItem.item);
                        uIItem.SetView(ViewType.LabView);
                        RefreshEquipmentView();
                        UpdateLabView();
                        uIItem.transform.SetParent(t_content.transform);
                    }
                    break;
                default:
                    break;
            }
        }

        void UpdateLabView()
        {
            int rows = labItems.Count / t_layoutGroup.constraintCount + 1;
            float t_Height = rows * (t_layoutGroup.cellSize.y + t_layoutGroup.spacing.y) + t_layoutGroup.padding.top + t_layoutGroup.padding.bottom - t_layoutGroup.spacing.y;

            if (t_Height > t_scrollRect.rect.height)
            {
                t_content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, t_Height);
            }
            else
            {
                t_content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, t_scrollRect.rect.height);
            }
            t_scrollbar.value = 1;
        }

        void OnSureCallBack()
        {
            if (currentLab.currentStep.GetScore() <= 0 && scoreChecking)
            {
                ResetUI();
                SetAnswer();
                scoreChecking = false;
                return;
            }

            List<Item> sceneItemInstances = SceneManager.GetInstance().GetAllLabItems(labItems);
            currentLab.AddComponents(sceneItemInstances);
            if (currentLab.VerifyEnableStart())
            {
                currentLab.currentStep.completedState = CompletedState.Finish;
                EventManager.OnTips(TipsType.Toast, "匹配成功!", null
                    , Testing);
            }
            else
            {
                currentLab.TriggerScore();
            }
        }

        void ResetUI()
        {
            while (t_content.childCount != 0)
            {
                UIItem uIItem = t_content.GetChild(0).GetComponent<UIItem>();
                OnSelectedItem(uIItem);
            }
        }

        void SetAnswer()
        {
            List<Item> labUsingItems = currentLab.defaultComponents;

            for (int i = 0; i < labUsingItems.Count; i++)
            {
                UIItem uIItem = GetUIItemByItem(labUsingItems[i]);
                GameObject itemObject = SceneManager.GetInstance().GetItemObject(labUsingItems[i]);
                itemObject.SetActive(false);
                OnSelectedItem(uIItem);
            }
        }

        public void Testing()
        {
            SceneManager.GetInstance().IsEntryScence = true;
            EventManager.OnUsingItems(currentLab.GetSelectedItems());
            EventManager.OnTipsDecided();
            UIManager.GetInstance().ShowScene();
            UIManager.GetInstance().InitLabReportUI();
            UIManager.GetInstance().uiMainPanle.SetPanleActive(true);
            DestroyImmediate(gameObject);
        }

        //public void PushItemToLab(UIItem uIItem)
        //{
        //    uIItems.Add(uIItem);
        //    labItems.Add(uIItem.item);

        //    float t_width = labItems.Count * (t_layoutGroup.cellSize.x + t_layoutGroup.spacing.x) + t_layoutGroup.padding.left + t_layoutGroup.padding.right - t_layoutGroup.spacing.x;
        //    if (t_width>t_scrollRect.rect.width)
        //    {
        //        t_content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,t_width);
        //    }
        //    else
        //    {
        //        t_content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, t_scrollRect.rect.width);
        //    }
        //}

        UIItem GetUIItemByItem(Item item)
        {
            for (int i = 0; i < uIItems.Count; i++)
            {
                if (uIItems[i].item == item)
                {
                    return uIItems[i];
                }
            }
            return null;
        }

    }
}