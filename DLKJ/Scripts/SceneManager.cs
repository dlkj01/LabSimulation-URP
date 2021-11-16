using System.Collections.Generic;
using UnityEngine;
using static DLKJ.InstrumentAction;

namespace DLKJ
{
    public class SceneManager : MonoBehaviour
    {
        [HideInInspector] public float currentLabScore;
        public const string FIRST_EXPERIMENT_NAME = "���˿�΢�������������";
        public const string SECOND_EXPERIMENT_NAME = "�����迹�������迹ƥ��";
        public const string THIRD_EXPERIMENT_NAME = "������������ԵĲ���";
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
        public int GetCurrentStep() { return currentLab.currentStepIndex; }
        public string GetCurrentLabName() { return currentLab.labName; }
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

                if (defaultItem.libraryType == LibraryType.Wires) //���µ����⴦��
                {
                    labItems[i].transform.GetChild(0).gameObject.SetActive(true);
                    Item targetItem = GetLabItemByID(labItems[i].linkConditions[0].data.itemID);
                    Link selfPort = labItems[i].ports[0];
                    Link targetPort = targetItem.GetPortByPortsID(labItems[i].linkConditions[0].data.portsID);
                    selfPort.transform.SetParent(targetPort.transform);
                    selfPort.transform.localPosition = labItems[i].portDefaultPosition;
                    selfPort.dragAble = false;
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

        private bool connecting = false;
        public void AutoConnectCurrentStep()
        {
            if (currentLab.currentStepIndex <= 0 || connecting) return;

            Debug.Log("�Զ�����:" + currentLab.currentStepIndex);

            UIManager.GetInstance().uiMainPanle.autoConnect.Interactable(false);
            List<Item> stepItems = currentLab.currentStep.keyItems;
           
            if (currentLab.currentStepIndex > 1)
            {
                int basicLinkItemsSize = currentLab.steps[1].keyItems.Count;
               
                for (int i = 0; i < stepItems.Count; i++)
                {

                    Item item = GetLabItemByID(stepItems[i].ID);
                    if (item.libraryType == LibraryType.Wires || item.linkPort != null) continue;

                    if (stepItems[i].itemName == "����첨��")
                    {
                        if (currentLab.currentStepIndex == 2)
                        {
                            stepItems[i].RotationY();
                        }
                        else
                        {
                            int value = i - basicLinkItemsSize;
                            if (value > 1)
                            {
                                item.transform.position = new Vector3(currentLab.originPosition.x, currentLab.originPosition.y, currentLab.originPosition.z - currentLab.spacing * value);
                            }
                            else
                            {
                                item.transform.position = new Vector3(currentLab.originPosition.x, currentLab.originPosition.y, currentLab.originPosition.z - currentLab.spacing * 1.5f);
                            }
                        }
                    }
                    else
                    {
                        item.transform.position = new Vector3(currentLab.originPosition.x, currentLab.originPosition.y, currentLab.originPosition.z - currentLab.spacing * 1.5f);
                    }
                }
                OnLinkNext(currentLab.currentStep.keyItems);
            }
            else
            {
                for (int i = 0; i < stepItems.Count; i++)
                {
                    Item item = GetLabItemByID(stepItems[i].ID);
                    item.linkPort = null;
                }
                SetBasicItemsLink();
            }
        }

        public void SetBasicItemsLink()
        {
            List<Item> basicItems = currentLab.currentStep.keyItems;
            int origin = 10000000;
            for (int i = 0; i < basicItems.Count; i++)
            {
                Item item = GetLabItemByID(basicItems[i].ID);
                if (item.libraryType == LibraryType.Wires) continue;
                if (item.ID == 11)//����תͬ��
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
            OnLinkNext(basicItems);
        }

        void OnLinkNext(List<Item> needToConnect)
        {
            connecting = true;
            originIndex++;

            if (originIndex < needToConnect.Count)
            {
                Item nextItem = GetLabItemByID(needToConnect[originIndex].ID);
                if (nextItem.linkPort != null)
                {
                    OnLinkNext(currentLab.currentStep.keyItems);
                    return;
                }

                Condition toConnectCondition = null;

                if (originIndex > 0)
                {
                    Item lastItem = GetLabItemByID(needToConnect[originIndex - 1].ID);

                    for (int i = 0; i < nextItem.linkConditions.Count; i++)
                    {
                        if (nextItem.linkConditions[i].data.itemID == lastItem.ID)
                        {
                            toConnectCondition = nextItem.linkConditions[i];
                            break;
                        }
                    }
                }

                nextItem.AutoConnect(toConnectCondition);
            }
            else
            {
                Debug.Log("�������");
                originIndex = -1;
                connecting = false;
                UpdateItemMoveable(false);
                //currentLab.NextStep();
                //UIManager.GetInstance().StepTips(currentLab.currentStep);
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
                        case 3:
                        case 4:
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
                        case 3:
                        case 4:
                        case 5:
                        case 6:
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
                        case 2:
                        case 3:
                        case 4:
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