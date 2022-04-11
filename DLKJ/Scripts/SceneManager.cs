using System.Collections.Generic;
using UnityEngine;
using static DLKJ.InstrumentAction;
using T_Common;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;

namespace DLKJ
{
    public class SceneManager : MonoBehaviour
    {
        public string md5Key = Int64.MaxValue.ToString();
        public float currentLabScore;
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

        public float GetScoreRate
        {
            get
            {
                float rate = 0;
                switch (currentLab.labName)
                {
                    case FIRST_EXPERIMENT_NAME:
                        rate = 0.4f;
                        break;
                    case SECOND_EXPERIMENT_NAME:
                        rate = 0.5f;
                        break;
                    case THIRD_EXPERIMENT_NAME:
                        rate = 0.1f;
                        break;
                }
                return rate;
            }
        }
        private int originIndex = -1;

        private void Awake()
        {
#if UNITY_WEBGL
            UserData data = new UserData() { accountNumber = "student1", password = "password1", userType = UserType.Student };
            loginUserData = data;
#endif

            string filePath = Application.streamingAssetsPath + "/Authorization.txt";
           
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            string lastString = GetLastLine(fs);
           
            int licenses = 0;
            if (lastString.Length > 0)
            {
                string lastStr = MD5Decryption(lastString);
                int.TryParse(lastStr, out licenses);
            }
            Debug.Log("licenses:" + licenses);
            int n = SaveManager.GetInstance().GetInt("n_l", 0, "User");
            int maxL = SaveManager.GetInstance().GetInt("max_l", 1, "User");
            if (licenses != maxL)
            {
                maxL = licenses;
                SaveManager.GetInstance().SetInt("max_l", licenses, "User");
                SaveManager.GetInstance().SetInt("n_l", 0, "User");
            }

            if (n >= maxL)
            {
                UIManager.GetInstance().uITips.OnTips(TipsType.Toast, "ʹ�ô��������꣡", null, OnTipsSure);
                UIManager.GetInstance().loginPanel.SetEnableLogin(false);
            }
            else
            {

                UIManager.GetInstance().loginPanel.SetEnableLogin(true);
            }
        }

        string GetMD5Hash(int number)
        {
            // ����һ��MD5CryptoServiceProvider�������ʵ����
            MD5 md5Hasher = MD5.Create();

            // ��������ַ���ת��Ϊһ���ֽ����鲢�����ϣֵ��

            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(number.ToString()));

            //����һ��StringBuilder���������ռ��ֽ������е�ÿһ���ֽڣ�Ȼ�󴴽�һ���ַ�����

            StringBuilder sBuilder = new StringBuilder();

            // �����ֽ����飬��ÿһ���ֽ�ת��Ϊʮ�������ַ�����׷�ӵ�StringBuilderʵ���Ľ�β

            for (int i = 0; i < data.Length; i++)

            {

                sBuilder.Append(data[i].ToString("x2"));

            }

            // ����һ��ʮ�������ַ���
            return sBuilder.ToString();
        }

        //MD5 - Encription 
        public string MD5Encryption(string inputData)
        {
            string hasKey = md5Key; 
            byte[] bData = UTF8Encoding.UTF8.GetBytes(inputData);

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            TripleDESCryptoServiceProvider tripalDES = new TripleDESCryptoServiceProvider();

            tripalDES.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hasKey));
            tripalDES.Mode = CipherMode.ECB;

            ICryptoTransform trnsfrm = tripalDES.CreateEncryptor();
            byte[] result = trnsfrm.TransformFinalBlock(bData, 0, bData.Length);

            return Convert.ToBase64String(result);
        }

        //MD5 -  Decryption
        public string MD5Decryption(string inputData)
        {
            string hasKey = md5Key; 
            byte[] bData = Convert.FromBase64String(inputData);

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            TripleDESCryptoServiceProvider tripalDES = new TripleDESCryptoServiceProvider();

            tripalDES.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hasKey));
            tripalDES.Mode = CipherMode.ECB;

            ICryptoTransform trnsfrm = tripalDES.CreateDecryptor();
            byte[] result = trnsfrm.TransformFinalBlock(bData, 0, bData.Length);

            return UTF8Encoding.UTF8.GetString(result);
        }

        /// <summary>
        /// ��ȡ�ı����һ������
        /// </summary>
        /// <param name="fs">�ļ���</param>
        /// <returns>���һ������</returns>
        private string GetLastLine(FileStream fs)
        {
            StreamReader streamReader = new StreamReader(fs);
            string lastLine = "";
            while (!streamReader.EndOfStream)
            {
                lastLine = streamReader.ReadLine();
            }
            //Debug.Log("fs.Length:" + fs.Length);
            //int seekLength = (int)(fs.Length < 1024 ? fs.Length : 1024);  // ������Ҫ�����Լ������ݳ��Ƚ��е�����Ҳ��д�ɶ�̬��ȡ�����Լ�ʵ��
            //byte[] buffer = new byte[seekLength];
            //fs.Seek(-buffer.Length, SeekOrigin.End);
            //fs.Read(buffer, 0, buffer.Length);
            //string multLine = System.Text.Encoding.UTF8.GetString(buffer);
            //string[] lines = multLine.Split(new string[] { "\\n" }, StringSplitOptions.RemoveEmptyEntries);
            //string line = lines[lines.Length - 1];

            return lastLine;
        }

        void OnTipsSure()
        {
            Application.Quit();
        }

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
#if UNITY_WEBGL || UNITY_EDITOR
                    WebLine[] webLines = FindObjectsOfType<WebLine>(true);
                    for (int j = 0; j < webLines.Length; j++)
                    {
                        Transform EndPos = null;
                        Transform startPos = null;
                        if (webLines[j].Name == "Ƶѡ-��������Line")
                        {
                            EndPos = webLines[j].transform.FindChildByName("ѡƵEndPos");
                            startPos = GetItemByName("ѡƵ�Ŵ���").transform.FindChildByName("Inputλ��");
                        }
                        if (webLines[j].Name == "΢��-����Line")
                        {
                            EndPos = webLines[j].transform.FindChildByName("End����");
                            startPos = GetItemByName("΢���ź�Դ").transform.FindChildByName("΢��StartPos");
                        }
                        EndPos.parent.SetParent(webLines[j].transform);
                        webLines[j].SetLineRender(startPos, EndPos);
                    }
#else
                     labItems[i].transform.GetChild(0).gameObject.SetActive(true);
#endif

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
                    if (item.libraryType == LibraryType.Wires) continue;

                    if (stepItems[i].itemName == "����첨��")
                    {
                        if (currentLab.currentStepIndex == 4)
                        {
                            item.RotationY();
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
                        if (item.linkPort != null) continue;
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
            bool state = false;
            switch (currentLab.labName)
            {
                case FIRST_EXPERIMENT_NAME:
                    if (currentLab.currentStepIndex>1)
                    {
                        state = VerifyBasicLink();
                    }
                    break;
                case SECOND_EXPERIMENT_NAME:
                    if (currentLab.currentStepIndex > 1)
                    {
                        state = VerifyBasicLink();
                    }
                    break;
                case THIRD_EXPERIMENT_NAME:

                    if (currentLab.currentStepIndex > 0)
                    {
                        state = VerifyBasicLink();
                    }
                    break;
                default:
                    break;
            }
            return state;
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