using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

namespace DLKJ
{
    public class LabWindow : BaseWindow
    {
        private static LabWindow window;

        private Vector2 scrollPos;
        private Vector2 listScroll;
        protected int selectIndex = 0;

        public static void Init()
        {
            window = (LabWindow)EditorWindow.GetWindow(typeof(LabWindow), false, "Lab Editor");
            window.minSize = new Vector2(750, 500);

            window.LoadDB();
        }

        public void OnGUI()
        {
            if (window == null) Init();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            if (GUILayout.Button("Creat New", new GUILayoutOption[] { GUILayout.Width(100), GUILayout.Height(30) }))
            {
                NewLab();
            }
            if (labDB.labs.Count > 0)
            {
                GUILayout.Space(10);
                if (GUILayout.Button("Remove", new GUILayoutOption[] { GUILayout.Width(100), GUILayout.Height(30) }))
                {
                    DeleteLab();
                }
            }
            GUILayout.Space(30);
            GUILayout.EndHorizontal();

            if (labDB.labs.Count == 0)
            {
                GUILayout.Label("There is no Lab:", EditorStyles.boldLabel, new GUILayoutOption[] { GUILayout.Width(150) });
                return;
            }

            GUILayout.BeginHorizontal();
            DrowLabList();

            GUILayout.BeginVertical();

            DrowLabConfigurator();

            GUILayout.EndVertical();


            GUILayout.EndHorizontal();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(labDB);
            }
        }

        void DrowLabList()
        {
            GUILayout.BeginVertical();
            listScroll = EditorGUILayout.BeginScrollView(listScroll, GUILayout.Width(250), GUILayout.Height(position.height - 40f));

            for (int i = 0; i < labDB.labs.Count; i++)
            {
                GUILayout.BeginHorizontal();
                if (selectIndex == i)
                {
                    GUI.color = Color.gray;
                }
                else
                {
                    GUI.color = Color.white;
                }

                if (GUILayout.Button(labDB.labs[i].labName, new GUILayoutOption[] {
                        GUILayout.Width (150),
                        GUILayout.Height (30)
                    }))
                {
                    selectIndex = i;
                }

                GUILayout.EndHorizontal();
            }
            GUI.color = Color.white;

            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            selectIndex = Mathf.Clamp(selectIndex, 0, labDB.labs.Count - 1);
        }

        bool componentsFoldout = true;
        bool stepsFoldout = true;
        void DrowLabConfigurator()
        {
            Lab selectedLab = labDB.labs[selectIndex];
            if (selectedLab == null) return;
            GUILayout.Space(30);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(window.position.width - 250), GUILayout.Height(window.position.height - 80));
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            Texture2D texture = null;
            if (selectedLab.icon)
            {
                texture = selectedLab.icon.texture;
            }
            else
            {
                //if (selectedLab._prefab)
                //{
                //	texture = AssetPreview.GetAssetPreview(selectUnit._prefab) as Texture2D;
                //}
            }

            if (GUILayout.Button(texture, new GUILayoutOption[] {GUILayout.Width (120), GUILayout.Height (120)
            }))
            {

            }

            GUILayout.Space(10);

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label("ID:" + selectedLab.ID, EditorStyles.boldLabel, new GUILayoutOption[] { GUILayout.Width(70) });
            GUILayout.EndHorizontal();

            //Name
            GUILayout.BeginHorizontal();
            GUILayout.Label("Name: " + Localization(selectedLab.labName), EditorStyles.label, new GUILayoutOption[] { GUILayout.Width(60) });
            selectedLab.labName = EditorGUILayout.TextField(selectedLab.labName, new GUILayoutOption[] { GUILayout.Width(100) });
            GUILayout.EndHorizontal();


            //Desc
            GUILayout.BeginHorizontal();
            GUILayout.Label("Desc: " + Localization(selectedLab.labDesc), EditorStyles.label, new GUILayoutOption[] { GUILayout.Width(60) });
            selectedLab.labDesc = EditorGUILayout.TextField(selectedLab.labDesc, new GUILayoutOption[] { GUILayout.Width(100) });
            GUILayout.EndHorizontal();


            if (texture == null)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Icon:", EditorStyles.label, new GUILayoutOption[] { GUILayout.Width(60) });
                selectedLab.icon = (Sprite)EditorGUILayout.ObjectField(selectedLab.icon, typeof(Sprite), false, new GUILayoutOption[] { GUILayout.Width(120) });
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("Audio:", EditorStyles.label, new GUILayoutOption[] { GUILayout.Width(60) });
            selectedLab.audioClip = (AudioClip)EditorGUILayout.ObjectField(selectedLab.audioClip, typeof(AudioClip), false, new GUILayoutOption[] { GUILayout.Width(150) });
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Score:", EditorStyles.label, new GUILayoutOption[] { GUILayout.Width(60) });
            selectedLab.score = EditorGUILayout.FloatField(selectedLab.score, new GUILayoutOption[] { GUILayout.Width(100) });
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("AutoMoveSpeed:", EditorStyles.label, new GUILayoutOption[] { GUILayout.Width(100) });
            selectedLab.autoMoveSpeed = EditorGUILayout.FloatField(selectedLab.autoMoveSpeed, new GUILayoutOption[] { GUILayout.Width(60) });
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("OriginPos:", EditorStyles.label, new GUILayoutOption[] { GUILayout.Width(60) });
            selectedLab.originPosition = EditorGUILayout.Vector3Field("", selectedLab.originPosition, new GUILayoutOption[] { GUILayout.Width(180) });
            GUILayout.Space(5);
            selectedLab.spacing = EditorGUILayout.FloatField(selectedLab.spacing, new GUILayoutOption[] { GUILayout.Width(40) });
            GUILayout.EndHorizontal();


            GUILayout.EndVertical();

            GUILayout.EndHorizontal();


            componentsFoldout = EditorGUILayout.Foldout(componentsFoldout, "Lab Components");
            //GUILayout.Space(10);
            //GUILayout.Label("Others:", EditorStyles.boldLabel, new GUILayoutOption[] { GUILayout.Width(55) });
            //GUILayout.Space(-15);
            //GUILayout.Label("____________________________________________", EditorStyles.label, new GUILayoutOption[] { GUILayout.Width(200) });
            if (componentsFoldout)
            {
                DrawConditions("", selectedLab.defaultComponents);
            }
            GUILayout.Space(10);

            stepsFoldout = EditorGUILayout.Foldout(stepsFoldout, "Lab Steps");
            if (stepsFoldout)
            {
                DrawLabSteps("", selectedLab);
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        void DrawConditions(string title, List<Item> components)
        {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();

            GUILayout.Label(title, EditorStyles.boldLabel, new GUILayoutOption[] { GUILayout.Width(title.Length * 7) });
            if (GUILayout.Button("+", new GUILayoutOption[] { GUILayout.Width(20), GUILayout.Height(20) }))
            {
                components.Add(itemDB.GetItemByIndex(0));
            }
            GUILayout.EndHorizontal();

            for (int i = 0; i < components.Count; i++)
            {
                GUILayout.BeginHorizontal();
                components[i] = DrawItemPopup(components[i].ID);
                if (GUILayout.Button("-", new GUILayoutOption[] { GUILayout.Width(20), GUILayout.Height(20) }))
                {
                    components.RemoveAt(i);
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
        }

        void DrawLabSteps(string title, Lab selected)
        {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();

            GUILayout.Label(title, EditorStyles.boldLabel, new GUILayoutOption[] { GUILayout.Width(title.Length * 7) });
            if (GUILayout.Button("+", new GUILayoutOption[] { GUILayout.Width(20), GUILayout.Height(20) }))
            {
                Step newStep = new Step();
                newStep.ID = GererateStepID(selected);
                selected.steps.Add(newStep);
            }
            GUILayout.EndHorizontal();

            for (int i = 0; i < selected.steps.Count; i++)
            {
                GUILayout.BeginHorizontal();
                DrawStepConfig(selected, i, selected.steps[i]);
                GUILayout.EndHorizontal();
            }


            GUILayout.EndVertical();
        }

        void DrawStepConfig(Lab selectedLab, int index, Step step)
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();

            GUILayout.Label("Step" + index + ":", EditorStyles.label, new GUILayoutOption[] { GUILayout.Width(60) });
            if (GUILayout.Button("-", new GUILayoutOption[] { GUILayout.Width(20), GUILayout.Height(20) }))
            {
                selectedLab.steps.Remove(step);
            }
            step.stepName = EditorGUILayout.TextField(step.stepName, new GUILayoutOption[] { GUILayout.Width(100) });
            step.points = EditorGUILayout.FloatField(step.points, new GUILayoutOption[] { GUILayout.Width(50) });
            step.valueType = (ValueType)EditorGUILayout.EnumPopup(step.valueType, new GUILayoutOption[] { GUILayout.Width(120) });
            step.dropPoints = EditorGUILayout.FloatField(step.dropPoints, new GUILayoutOption[] { GUILayout.Width(20) });
            step.completedState = (CompletedState)EditorGUILayout.EnumPopup(step.completedState, new GUILayoutOption[] { GUILayout.Width(80) });
            step.nextStepCanMove = EditorGUILayout.Toggle(step.nextStepCanMove, new GUILayoutOption[] { GUILayout.Width(20) });
            if (GUILayout.Button("+", new GUILayoutOption[] { GUILayout.Width(20), GUILayout.Height(20) }))
            {
                step.keyItems.Add(itemDB.GetItemByID(0));
            }

            for (int i = 0; i < step.keyItems.Count; i++)
            {
                step.keyItems[i] = DrawItemPopup(step.keyItems[i].ID);
                if (GUILayout.Button("-", new GUILayoutOption[] { GUILayout.Width(20), GUILayout.Height(20) }))
                {
                    step.keyItems.RemoveAt(i);
                }
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

        }

        void NewLab()
        {
            Lab lab = new Lab();
            lab.ID = GenerateNewID();
            lab.labName = "LabName" + lab.ID;
            lab.labDesc = "LabDesc" + lab.ID;

            if (labDB.labs.Count == 0)
            {
                labDB.labs.Add(lab);
                selectIndex = 0;
            }
            else
            {
                labDB.labs.Insert(selectIndex + 1, lab);
                selectIndex = selectIndex + 1;
            }
        }

        void DeleteLab()
        {
            labDB.labs.RemoveAt(selectIndex);
            if (selectIndex >= labDB.labs.Count)
            {
                selectIndex = labDB.labs.Count - 1;
            }
        }
        private int GenerateNewID()
        {
            int ID = 0;

            for (int i = 0; i < labDB.labs.Count; i++)
            {
                if (labDB.labs[i].ID > ID)
                {
                    ID = labDB.labs[i].ID;
                }
            }
            ID += 1;

            return ID;
        }

        int GererateStepID(Lab lab) {
            int ID = 0;
            for (int i = 0; i < lab.steps.Count; i++)
            {
                if (lab.steps[i].ID>ID)
                {
                    ID = lab.steps[i].ID;
                }
            }
            ID += 1;
            return ID;
        }


    }
}