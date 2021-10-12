using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DLKJ {

    public enum CompletedState
    {
        Unfinished,
        Finish,
    }

    [System.Serializable]
    public enum ValueType
    {
        Value = 100,
        ValuePercentAdd = 200,
        ValuePercentMultiply = 300,
        ValueFixed = 400,
        SpecialValue = 500,
        SpecialValuePercentAdd = 600,
        SpecialValuePercentMultiply = 700,
        SpecialValueFixed = 800,
    }

    [System.Serializable]
    public class Lab
    {
        public int ID = -1;
        public Sprite icon;
        public string labName = "";
        public string labDesc = "";
        public float score = 100;
        public AudioClip audioClip;
        public float spacing = 1;
        public float autoMoveSpeed = 1f;
        public Vector3 originPosition = Vector3.zero;


        public List<Item> defaultComponents = new List<Item>();
        private List<Item> selectedComponents = new List<Item>();

        public List<Step> steps = new List<Step>();

        public int currentStepIndex = 0;
        public Step currentStep = null;

        public Lab Clone()
        {
            Lab newLab = new Lab();
            newLab.ID = ID;
            newLab.icon = icon;
            newLab.labName = labName;
            newLab.labDesc = labDesc;
            newLab.score = score;
            newLab.audioClip = audioClip;
            newLab.spacing = spacing;
            newLab.autoMoveSpeed = autoMoveSpeed;
            newLab.originPosition = originPosition;
            newLab.currentStep = currentStep;
            newLab.currentStepIndex = currentStepIndex;
            newLab.defaultComponents.AddRange(defaultComponents);
            newLab.selectedComponents.AddRange(selectedComponents);
            for (int i = 0; i < steps.Count; i++)
            {
                newLab.steps.Add(steps[i].Clone());
            }
            return newLab;
        }

        public void Initialized()
        {
            currentStepIndex = 0;
            currentStep = steps[currentStepIndex];
            currentStep.Initialized();
        }

        public void NextStep()
        {
            currentStep.completedState = CompletedState.Finish;
            currentStepIndex++;
            if (currentStepIndex > steps.Count - 1) return;
            currentStep = steps[currentStepIndex];
            currentStep.Initialized();
            UIManager.GetInstance().StepTips(currentStep);
        }

        public Step GetStepByID(int stepID)
        {
            for (int i = 0; i < steps.Count; i++)
            {
                if (steps[i].ID == stepID)
                {
                    return steps[i];
                }
            }
            return null;
        }

        public void AddComponents(List<Item> components)
        {
            selectedComponents.Clear();
            selectedComponents.AddRange(components);
        }

        public List<Item> GetSelectedItems() { return selectedComponents; }

        public bool VerifyEnableStart()
        {
            bool pass = false;
            LabDB labDB = DBManager.GetInstance().GetDB<LabDB>();

            var listKey = defaultComponents.Select(c => c.ID).ToList();
            if (listKey.All(c => selectedComponents.Any(s => s.ID == c)))
            {
                pass = true;
                //����С���ϵ����������������������һ�����������ڴ󼯺��У��Ϳ����ж��󼯺�ȫ����С����
            }
            Debug.Log("It's Right?"+pass);
            return pass;
        }

        public bool VerifyBasicLink()
        {
            return currentStep.BasicCompleted();
        }

        public void TriggerScore()
        {
           currentStep.TriggerScore();
        }
    }

    [System.Serializable]
    public class Step
    {
        public int ID = -1;
        public float points = 0;        //�ܷ�ֵ
        public float dropPoints = -0;   //ÿ�δ���ķ�ֵ
        public string stepName;
        public ValueType valueType = ValueType.Value;
        public CompletedState completedState = CompletedState.Unfinished;
        public List<Item> keyItems = new List<Item>();

        private float score = 0;

        public Step Clone()
        {
            Step newStep = new Step();
            newStep.ID = ID;
            newStep.points = points;
            newStep.dropPoints = dropPoints;
            newStep.stepName = stepName;
            newStep.valueType = valueType;
            newStep.completedState = completedState;
            newStep.keyItems.AddRange(keyItems);

            return newStep;
        }

        public void Initialized()
        {
            score = points;
        }

        public void SetState(bool completed)
        {
            completedState = (CompletedState)Convert.ToInt32(completed);
        }

        public void TriggerScore()
        {
            score += dropPoints;
        }

        public float GetScore()
        {
            return score;  //ÿ������ķ�ֵ��Ҫ����ʵ�ʽ��м���
        }

        public bool BasicCompleted()
        {
            bool completed = true;
            for (int i = 0; i < keyItems.Count; i++)
            {
                Item basic = SceneManager.GetInstance().GetLabItemByID(keyItems[i].ID);
                if (basic!=null)
                {
                    if (basic.linkPort == null)
                    {
                        completed = false;
                        return completed;
                    }
                }
            }
            return completed;
        }

    }
}