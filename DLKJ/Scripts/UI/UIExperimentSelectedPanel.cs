using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Common;
namespace DLKJ
{
    public class UIExperimentSelectedPanel : MonoBehaviour
    {
        [SerializeField] Transform experimentContent;
        [SerializeField] UIExperiment uIExperimentPrefab;

        public void Initialized()
        {
            LabDB labDB = DBManager.GetInstance().GetDB<LabDB>();
            List<Lab> labs = labDB.labs;
            for (int i = 0; i < labs.Count; i++)
            {
                UIExperiment uIExperiment = Instantiate(uIExperimentPrefab, experimentContent) as UIExperiment;
                uIExperiment.Initialized(labs[i]);
                if (ProxyManager.saveProxy.map[labs[i].labName].isFinished)
                {
                    uIExperiment.SetUnInteractive();
                }
            }
        }
    }
}