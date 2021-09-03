using UnityEngine;
using System.Collections;
using UnityEditor; 
using DLKJ;
namespace DLKJ
{
    public class MyEditorWindows : MonoBehaviour
    {
        [MenuItem("DLKJ/ItemEditor", false, 10)]
        static void OpenItemEditorWindow()
        {
            ItemWindow.Init();
        }

        [MenuItem("DLKJ/LabEditor", false, 10)]
        static void OpenLabEditorWindow()
        {
            LabWindow.Init();
        }
    }
}
