using UnityEngine;
namespace T_Common
{
    public static class TransformHelper
    {
        public static Transform FindChildByName(this Transform currentTF, string childName)
        {
            Transform childTF = currentTF.Find(childName);
            if (childTF != null)
            {
                return childTF;
            }
            for (int i = 0; i < currentTF.childCount; i++)
            {
                childTF = FindChildByName(currentTF.GetChild(i), childName);
                if (childTF != null)
                {
                    return childTF;
                }
            }
            return null;
        }
    }
}