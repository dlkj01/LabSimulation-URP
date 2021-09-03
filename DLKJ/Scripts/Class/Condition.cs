using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DLKJ
{
	[System.Serializable]
	public class Data
	{
		public int itemID = -1;        //Item ID
		public int portsID = -1;       //Ports ID
		public bool correct = false;   //是否是唯一的正确连接
		public float value = 0;
		public float weight = 0;      //权重

		public Data Clone()
		{
			Data newData = new Data();
			newData.itemID = itemID;
			newData.portsID = portsID;
			newData.value = value;
			newData.weight = weight;
			return newData;
		}
	}

    [System.Serializable]
    public class Condition
    {
		public Data data = new Data();

		public Condition Clone()
		{
			Condition newCondition = new Condition();
			newCondition.data = data.Clone();
			return newCondition;
		}
	}
}