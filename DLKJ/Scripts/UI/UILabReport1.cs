using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DLKJ
{
    public class UILabReport1 : MonoBehaviour
    {
        [SerializeField] InputField nameInputField;
        [SerializeField] InputField classInputField;
        [SerializeField] InputField timeInputField;
        [SerializeField] InputField idInputField;
        [SerializeField] InputField teacherInputField;

        // Start is called before the first frame update
        void Start()
        {

        }

        public void OnSureCallBack()
        {
            if (nameInputField.text.Length>0)
            {

            }
            else
            {
                EventManager.OnTips(TipsType.Toast,"«Î ‰»Î–’√˚");
            }
        }

    }
}