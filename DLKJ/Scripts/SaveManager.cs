using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DLKJ {

    public class SaveManager : MonoBehaviour {
		public static SaveManager instance;

        public Dictionary<string, ES3File> fileDic = new Dictionary<string, ES3File>();

        public static SaveManager GetInstance() {
			if (null == instance) {
				instance = (SaveManager)GameObject.FindObjectOfType (typeof(SaveManager));
			}
			return instance;
		}

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        public void RemoveFile(string name)
        {
            fileDic.Remove(name);
        }

        public ES3File GetFile(string name)
        {
            if (fileDic.ContainsKey(name))
            {
                return fileDic[name];
            }
            else
            {
                if (!ES3.FileExists(name))
                {
                    ES3.Save<string>("new", "new", name);
                }
                ES3File es3File = new ES3File(name);
                fileDic.Add(name, es3File);
                //Sync();
                return es3File;
            }
        }

        public void SetString(string key, string value, string file = "SaveKey")
        {
            string fileName = file + ".bc";
            IsKeyExists(key, fileName);
            ES3File es3File = GetFile(fileName);
            es3File.Save<string>(key, value);
            StartCoroutine(SyncFile(fileName));
        }

        public string GetString(string key, string defaultValue = "", string file = "SaveKey")
        {
            string fileName = file + ".bc";
            if (!ES3.FileExists(fileName))
            {
                return defaultValue;
            }

            bool isKeyExists = IsKeyExists(key, fileName);
            if (isKeyExists)
            {
                ES3File es3File = GetFile(fileName);
                return es3File.Load<string>(key, defaultValue);
            }
            else
            {
                return defaultValue;
            }
        }

        public void SetInt(string key, int value, string file = "SaveKey")
        {
            string fileName = file + ".bc";
            IsKeyExists(key, fileName);
            ES3File es3File = GetFile(fileName);
            es3File.Save<int>(key, value);
            StartCoroutine(SyncFile(fileName));
        }

        public int GetInt(string key, int defaultValue = 0, string file = "SaveKey")
        {
            string fileName = file + ".bc";
            if (!ES3.FileExists(fileName))
            {
                return defaultValue;
            }

            bool isKeyExists = IsKeyExists(key, fileName);
            if (isKeyExists)
            {
                ES3File es3File = GetFile(fileName);
                return es3File.Load<int>(key, defaultValue);
            }
            else
            {
                return defaultValue;
            }
        }

        public void SetFloat(string key, float value, string file = "SaveKey")
        {
            string fileName = file + ".bc";
            IsKeyExists(key, fileName);
            ES3File es3File = GetFile(fileName);
            es3File.Save<float>(key, value);
            StartCoroutine(SyncFile(fileName));
        }

        public float GetFloat(string key, float defaultValue = 0f, string file = "SaveKey")
        {
            string fileName = file + ".bc";
            if (!ES3.FileExists(fileName))
            {
                return defaultValue;
            }

            bool isKeyExists = IsKeyExists(key, fileName);
            if (isKeyExists)
            {
                ES3File es3File = GetFile(fileName);
                return es3File.Load<float>(key, defaultValue);
            }
            else
            {
                return defaultValue;
            }
        }

        public bool IsKeyExists(string key, string fileName)
        {
            bool isKeyExists = false;
            try
            {
                ES3File es3File = GetFile(fileName);
                isKeyExists = es3File.KeyExists(key);
            }
            catch
            {
                if (ES3.RestoreBackup(fileName))
                {
                    try
                    {
                        RemoveFile(fileName);
                        ES3File es3File = GetFile(fileName);
                        isKeyExists = es3File.KeyExists(key);
                    }
                    catch
                    {
                        RemoveFile(fileName);
                        ES3.DeleteFile(fileName);
                        isKeyExists = false;
                    }
                }
                else
                {
                    RemoveFile(fileName);
                    ES3.DeleteFile(fileName);
                    isKeyExists = false;
                }
            }
            return isKeyExists;
        }

        IEnumerator SyncFile(string fileName)
        {
            yield return new WaitForEndOfFrame();
            //Debug.Log("SaveManager SyncFile");
            if (fileDic.ContainsKey(fileName)){
                fileDic[fileName].Sync();
            }
        }


        public void BackupAll()
        {
            string[] files = ES3.GetFiles("");
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Contains(".bc") && !files[i].Contains(".bac"))
                {
                    IsKeyExists("", files[i]);
                    ES3.CreateBackup(files[i]);
                }
            }
        }

        public void DeleteFile(string file)
        {
            string fileName = file + ".bc";
            RemoveFile(fileName);
            ES3.DeleteFile(fileName);
        }

        public void DeleteKey(string key, string file = "SaveKey")
        {
            string fileName = file + ".bc";
            ES3File es3File = GetFile(fileName);
            es3File.DeleteKey(key);
            StartCoroutine(SyncFile(fileName));
        }
    }
}
