﻿

namespace PlayFab.Editor {
    using System;
    using UnityEngine;
    using UnityEditor;
    using System.Collections.Generic;
    using PlayFab.Editor.EditorModels;

    public class TitleInternalDataViewer : Editor {
        public List<KvpItem> items;
        public static TitleDataEditor tdEditor;
        public string displayTitle = "";
        public Vector2 scrollPos = Vector2.zero;
        private bool showSave = false;

        // this gets called after the Base draw loop
        public void Draw()
        {
            EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if(GUILayout.Button("REFRESH",  PlayFabEditorHelper.uiStyle.GetStyle("Button")))
                {
                     RefreshRecords();

                }

            if(GUILayout.Button("+",  PlayFabEditorHelper.uiStyle.GetStyle("Button"), GUILayout.MaxWidth(25)))
                {
                    AddRecord();
                }

            EditorGUILayout.EndHorizontal();


            if(items.Count > 0)
            {
                    scrollPos = GUILayout.BeginScrollView(scrollPos, PlayFabEditorHelper.uiStyle.GetStyle("gpStyleGray1"));
                    float keyInputBoxWidth = EditorGUIUtility.currentViewWidth > 200 ? 170 : (EditorGUIUtility.currentViewWidth - 100) / 2;
                    float valueInputBoxWidth = EditorGUIUtility.currentViewWidth > 200 ? EditorGUIUtility.currentViewWidth - 290 : (EditorGUIUtility.currentViewWidth - 100) / 2; 

                      for(var z = 0; z < this.items.Count; z++)
                    {
                        this.items[z].DataEditedCheck();
                        if(items[z].isDirty)
                        {
                            showSave = true;
                        }

                        if(items[z].Value != null)
                        {

                        var keyStyle = this.items[z].isDirty ?  PlayFabEditorHelper.uiStyle.GetStyle("listKey_dirty") :PlayFabEditorHelper.uiStyle.GetStyle("listKey");
                        var valStyle = this.items[z].isDirty ?  PlayFabEditorHelper.uiStyle.GetStyle("listValue_dirty") : PlayFabEditorHelper.uiStyle.GetStyle("listValue");


                        EditorGUILayout.BeginHorizontal(PlayFabEditorHelper.uiStyle.GetStyle("gpStyleClear"));




                        items[z].Key = GUILayout.TextField(items[z].Key, keyStyle, GUILayout.Width(keyInputBoxWidth));

                            EditorGUILayout.LabelField(":", GUILayout.MaxWidth(10));
                            GUILayout.Label(""+items[z].Value, valStyle, GUILayout.MaxWidth(valueInputBoxWidth), GUILayout.MaxHeight(25));  

                        if(GUILayout.Button("Edit",  PlayFabEditorHelper.uiStyle.GetStyle("Button"), GUILayout.MaxHeight(19), GUILayout.MinWidth(35)))
                            {
                                tdEditor.LoadData(items[z].Key, items[z].Value);
                                TitleDataEditor.ShowWindow(tdEditor);
                            } 
                        if(GUILayout.Button("X",  PlayFabEditorHelper.uiStyle.GetStyle("Button"), GUILayout.MaxHeight(19), GUILayout.MinWidth(20)))
                            {
                                items[z].isDirty = true;
                                items[z].Value = null;
                            } 
                          
                            EditorGUILayout.EndHorizontal();
                        }
                    }


                GUILayout.EndScrollView();

                if(showSave)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if(GUILayout.Button("SAVE", PlayFabEditorHelper.uiStyle.GetStyle("Button"), GUILayout.MaxWidth(200)))
                        {
                            SaveRecords();
                        }
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();
                }
            }
        }


        public void AddRecord()
        {
            this.items.Add(new KvpItem("","NewValue"){isDirty = true});
        }

        public void RefreshRecords()
        {
            Action<PlayFab.Editor.EditorModels.GetTitleDataResult> cb = (result) => {
                
                items.Clear();
                showSave = false;
                foreach(var kvp in result.Data)
                {
                    items.Add(new KvpItem(kvp.Key, kvp.Value));
                }

                PlayFabEditorDataService.envDetails.titleInternalData = result.Data;
                PlayFabEditorDataService.SaveEnvDetails();

            };

            PlayFabEditorApi.GetTitleInternalData(cb, PlayFabEditorHelper.SharedErrorCallback); 
        }

        public void SaveRecords()
        {
            //reset dirty status.
            showSave = false;
            Dictionary<string, string> dirtyItems = new Dictionary<string, string>();
            foreach(var item in items)
            {
                if(item.isDirty)
                {
                    dirtyItems.Add(item.Key, item.Value);
                }

            }

            if(dirtyItems.Count > 0)
            {
                PlayFabEditorApi.SetTitleInternalData(dirtyItems, (result) => 
                {
                    foreach(var item in items)
                    {
                        item.CleanItem();
                    }
                }, PlayFabEditorHelper.SharedErrorCallback);
            } 
        }



        public TitleInternalDataViewer(List<KvpItem> i = null)
        {
            this.items = i ?? new List<KvpItem>();
        }

        public TitleInternalDataViewer()
        {
            this.items = new List<KvpItem>();
        }

        public void OnEnable()
        {
            if(tdEditor == null)
            {
                tdEditor = ScriptableObject.CreateInstance<TitleDataEditor>();
            }
        }

    }
   
}

