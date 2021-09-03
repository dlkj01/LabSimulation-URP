
using System;
using System.Collections.Generic;

namespace DLKJ
{
    [Serializable]
    public class ParaObject
    {
        /// <summary>
        /// 
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int score { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long startTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long endTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int timeUsed { get; set; }
        /// <summary>
        /// –Ï¿œ ¶
        /// </summary>
        public string appid { get; set; }

        public string originId { get; set; }

        public List<Stepspro> steps { get; set; }
    }

}
