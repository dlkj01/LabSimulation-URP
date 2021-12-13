using System.Runtime.InteropServices;
using UnityEngine;


namespace DLKJ {
    public class WebGLDownloadHelper
    {
        [DllImport("__Internal")]
        private static extern void WebGLDownloadFile(byte[] array, int byteLength, string fileName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="fileName">带文件格式的完整名称</param>
        public static void DownloadDocx(byte[] bytes, string fileName)
        {
            WebGLDownloadFile(bytes, bytes.Length, fileName);
        }
    }
}
