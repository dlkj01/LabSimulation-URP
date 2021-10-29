using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
using System.Linq;
using System.Collections;
using static DLKJ.InstrumentAction;

namespace DLKJ
{

    public class SceneManager : MonoBehaviour
    {
        public const string FIRST_EXPERIMENT_NAME = "二端口微波网络参量测量";
        public const string SECOND_EXPERIMENT_NAME = "负载阻抗测量";
        public const string THIRD_EXPERIMENT_NAME = "负载阻抗匹配和定向耦合器特性的测量";
        public static UserData loginUserData;

        public static bool didExperiment = false;
        [SerializeField] Texture2D holdTexture;
        [SerializeField] Texture2D clickTexture;
        [SerializeField] Texture2D releaseTexture;

        private static SceneManager instance;
        public Vector3 min = new Vector3(-0.454f, 1f, -0.711f);
        public Vector3 max = new Vector3(0.934f, 1.253f, 3.554f);

        private Dictionary<int, Item> scrollItemObjectDic = new Dictionary<int, Item>();
        public List<Item> labItems = new List<Item>();

        public Lab currentLab;
        public bool IsEntryScence = false;


        private int originIndex = -1;

        public static SceneManager GetInstance()
        {
            if (null == instance)
            {
                instance = (SceneManager)GameObject.FindObjectOfType(typeof(SceneManager));
            }
            return instance;
        }

        private void OnEnable()
        {
            EventManager.OnUsingItemsEvent += OnUsingItem;
            EventManager.OnLinkNextEvent += OnLinkNext;
        }

        private void OnDisable()
        {
            EventManager.OnUsingItemsEvent -= OnUsingItem;
            EventManager.OnLinkNextEvent -= OnLinkNext;
        }

        void OnUsingItem(List<Item> items)
        {
            labItems.Clear();
            labItems.AddRange(items);
            if (currentLab.currentStep.completedState == CompletedState.Finish) currentLab.NextStep();
            UIManager.GetInstance().StepTips(currentLab.steps[currentLab.currentStepIndex]);

            ItemDB itemDB = DBManager.GetInstance().GetDB<ItemDB>();
            for (int i = 0; i < labItems.Count; i++)
            {
                Item defaultItem = itemDB.GetItemByID(labItems[i].ID);
                labItems[i].transform.position = defaultItem.transform.position;
                labItems[i].gameObject.SetActive(true);

                if (defaultItem.libraryType == LibraryType.Wires) //线缆的特殊处理
                {
                    labItems[i].transform.GetChild(0).gameObject.SetActive(true);
                    Item targetItem = GetLabItemByID(labItems[i].linkConditions[0].data.itemID);
                    Link selfPort = labItems[i].ports[0];
                    Link targetPort = targetItem.GetPortByPortsID(labItems[i].linkConditions[0].data.portsID);
                    selfPort.transform.SetParent(targetPort.transform);
                    selfPort.transform.localPosition = labItems[i].portDefaultPosition;
                    selfPort.transform.localRotation = Quaternion.Euler(labItems[i].portDefaultEuler);
                }

                BoxCollider boxCollider;
                if (labItems[i].transform.TryGetComponent(out boxCollider))
                {
                    boxCollider.enabled = true;
                }
                MeshCollider meshCollider;
                if (labItems[i].transform.TryGetComponent(out meshCollider))
                {
                    meshCollider.enabled = true;
                }

                InstrumentAction instrumentAction;
                if (labItems[i].transform.TryGetComponent(out instrumentAction))
                {
                    if (instrumentAction.voltmeterCamera) instrumentAction.voltmeterCamera.gameObject.SetActive(true);
                }
            }
            UIManager.GetInstance().SetVerifyButtonActive(true);
        }

        public void SetMouseState(bool hold = false, bool click = false)
        {
            if (hold)
            {
                Cursor.SetCursor(holdTexture, Vector2.zero, CursorMode.ForceSoftware);
            }
            else if (click)
            {
                Cursor.SetCursor(clickTexture, Vector2.zero, CursorMode.ForceSoftware);
            }
            else
            {
                Cursor.SetCursor(releaseTexture, Vector2.zero, CursorMode.ForceSoftware);
            }
        }

        public bool VerifyBasicLink()
        {
            return currentLab.VerifyBasicLink();
        }

        public void UpdateItemMoveable(bool able)
        {
            Step currentStep = currentLab.currentStep;
            if (currentStep.nextStepCanMove == true) return;
            for (int i = 0; i < currentStep.keyItems.Count; i++)
            {
                Item labItem = GetLabItemByID(currentStep.keyItems[i].ID);
                labItem.SetMoveable(able);
            }
        }

        public void SetBasicItemsLink()
        {
            List<Item> basicItems = currentLab.currentStep.keyItems;
            int origin = 10000000;
            for (int i = 0; i < basicItems.Count; i++)
            {
                Item item = GetLabItemByID(basicItems[i].ID);

                if (item.ID == 11)//波导转同轴
                {
                    origin = i;
                    originIndex = origin;
                    item.transform.position = currentLab.originPosition;
                }
                else
                {
                    if (i > origin)
                    {
                        item.transform.position = new Vector3(currentLab.originPosition.x, currentLab.originPosition.y,
                            currentLab.originPosition.z - currentLab.spacing * (i - origin));
                    }
                }
            }
            OnLinkNext();
        }

        void OnLinkNext()
        {
            originIndex++;
            List<Item> basicItems = currentLab.currentStep.keyItems;
            if (originIndex < basicItems.Count)
            {
                Item nextItem = GetLabItemByID(basicItems[originIndex].ID);
                nextItem.AutoConnect();
            }
            else
            {
                UpdateItemMoveable(false);
                currentLab.NextStep();
                UIManager.GetInstance().StepTips(currentLab.currentStep);
            }
        }

        public GameObject GetItemObject(Item item)
        {
            foreach (var targetItem in scrollItemObjectDic)
            {
                targetItem.Value.gameObject.SetActive(false);
            }

            if (scrollItemObjectDic.ContainsKey(item.ID))
            {
                return scrollItemObjectDic[item.ID].gameObject;
            }
            else
            {
                Item newItemObject = Instantiate(item) as Item;
                newItemObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.5f);
                scrollItemObjectDic.Add(item.ID, newItemObject);
                return newItemObject.gameObject;
            }
        }


        public List<Item> GetAllLabItems(List<Item> items)
        {
            List<Item> labItemObjects = new List<Item>();
            foreach (var item in scrollItemObjectDic)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (item.Key == items[i].ID)
                    {
                        labItemObjects.Add(item.Value);
                    }
                }
            }
            return labItemObjects;
        }

        public List<Item> GetUsingItems()
        {
            return labItems;
        }

        public Item GetLabItemByID(int itemID)
        {
            for (int i = 0; i < labItems.Count; i++)
            {
                if (itemID == labItems[i].ID)
                {
                    return labItems[i];
                }
            }
            return null;
        }
        public Item GetItemByName(string itemName)
        {
            for (int i = 0; i < labItems.Count; i++)
            {
                if (itemName == labItems[i].itemName)
                {
                    return labItems[i];
                }
            }
            return null;
        }

        public bool CurrentStepVerify()
        {
            switch (currentLab.labName)
            {
                case FIRST_EXPERIMENT_NAME:
                    switch (currentLab.currentStepIndex)
                    {
                        case 2:
                            return VerifyBasicLink();
                        case 3:
                            return VerifyBasicLink();
                        case 4:
                            return VerifyBasicLink();
                        case 5:
                            return VerifyBasicLink();
                        default:
                            break;
                    }
                    break;
                case SECOND_EXPERIMENT_NAME:
                    switch (currentLab.currentStepIndex)
                    {
                        case 2:
                            return VerifyBasicLink();
                        case 3:
                            return VerifyBasicLink();
                        case 4:
                            return VerifyBasicLink();
                        case 5:
                            return VerifyBasicLink();
                        case 6:
                            return VerifyBasicLink();
                        case 7:
                            return VerifyBasicLink();
                        default:
                            break;
                    }
                    break;
                case THIRD_EXPERIMENT_NAME:

                    switch (currentLab.currentStepIndex)
                    {
                        case 1:
                            return VerifyBasicLink();
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return false;
        }
        public InstrumentButton GetInstrumentButton(string deviceName, string buttonName)
        {
            Item item = GetItemByName(deviceName);
            if (item == null)
                return null;
            InstrumentAction instrumentAction = item.GetComponent<InstrumentAction>();
            if (instrumentAction == null)
                return null;
            InstrumentButton button = instrumentAction.instrumentButton.Find(x => x.instrumentButton.name == buttonName);
            return button;
        }
    }
}