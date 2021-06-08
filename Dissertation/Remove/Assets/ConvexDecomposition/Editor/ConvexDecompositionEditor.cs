using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CMR
{
    [CustomEditor(typeof(ConvexDecompositionSettings))]
    public class ConvexDecompositionEditor : Editor
    {
        private static class Styles
        {
            public static GUIContent presetIcon = EditorGUIUtility.IconContent("Preset.Context");
            public static GUIStyle iconButton = new GUIStyle("IconButton");
        }

        public GameObject selectedObject;
        public Mesh inputMesh;
        public PhysicMaterial physicMaterial;

        bool m_StylesInitialized;
        GUIContent m_EnableOpenCLLabel;
        GUIStyle m_FoldoutStyle;
        GUIStyle m_HorizontalLine;
        Color m_HorizontalLineColor;

        ConvexDecompositionSettings m_Settings;
        bool m_HasOpenCL;
        List<string> m_OpenCLPlatforms;
        List<string> m_OpenCLDevices;

        bool m_HullFoldout = true;
        bool m_AdvancedFoldout;
        bool m_OpenCLFoldout = true;
        bool m_SettingsFoldout;

        void OnEnable()
        {
            m_Settings = (ConvexDecompositionSettings)target;

            if (selectedObject == null)
            {
                selectedObject = Selection.activeGameObject;
            }

            if (selectedObject != null && physicMaterial == null)
            {
                physicMaterial = GetPhysicMaterial(selectedObject);
            }

            m_OpenCLPlatforms = VHCDAPI.GetPlatforms();
            m_OpenCLDevices = m_OpenCLPlatforms.Count > 0 ? VHCDAPI.GetDevices(0) : new List<string>();

            var enableOpenCL = serializedObject.FindProperty("m_EnableOpenCL");
            var openCLPlatformID = serializedObject.FindProperty("m_OpenCLPlatformID");
            var openCLDeviceID = serializedObject.FindProperty("m_OpenCLDeviceID");

            // Check if OpenCL is supported
            m_HasOpenCL = m_OpenCLPlatforms.Count > 0 && m_OpenCLDevices.Count > 0;
            if (enableOpenCL.boolValue && !m_HasOpenCL)
            {
                enableOpenCL.boolValue = false;
            }

            openCLPlatformID.intValue = Math.Max(0, Math.Min(openCLPlatformID.intValue, m_OpenCLPlatforms.Count - 1));
            openCLDeviceID.intValue = Math.Max(0, Math.Min(openCLDeviceID.intValue, m_OpenCLDevices.Count - 1));
        }

        public override void OnInspectorGUI()
        {
            #region serializedObject handling

            if (serializedObject == null || serializedObject.targetObject == null)
            {
                EditorGUILayout.HelpBox("No target object", MessageType.Info);
                return;
            }

            // Refresh the serialized object from its backing store
            serializedObject.Update();

            var propCreateAsset = serializedObject.FindProperty("m_CreateAsset");
            var propCreateColliders = serializedObject.FindProperty("m_CreateColliders");
            var propCreateMeshRenderers = serializedObject.FindProperty("m_CreateMeshRenderers");
            var propResolution = serializedObject.FindProperty("m_Resolution");
            var propMaxHulls = serializedObject.FindProperty("m_MaxHulls");
            var propMaxConcavity = serializedObject.FindProperty("m_MaxConcavity");
            var propPlaneDownsampling = serializedObject.FindProperty("m_PlaneDownsampling");
            var propHullDownsampling = serializedObject.FindProperty("m_HullDownsampling");
            var propSymmetryPlaneBias = serializedObject.FindProperty("m_SymmetryPlaneBias");
            var propRevolutionAxesBias = serializedObject.FindProperty("m_RevolutionAxesBias");
            var propMeshNormalization = serializedObject.FindProperty("m_MeshNormalization");
            var propTetrahedronMode = serializedObject.FindProperty("m_TetrahedronMode");
            var propMaxVertices = serializedObject.FindProperty("m_MaxVertices");
            var propMinVolume = serializedObject.FindProperty("m_MinVolume");
            var propApproximateHulls = serializedObject.FindProperty("m_ApproximateHulls");
            var propProjectVertices = serializedObject.FindProperty("m_ProjectVertices");
            var propEnableOpenCL = serializedObject.FindProperty("m_EnableOpenCL");
            var propOpenCLPlatformID = serializedObject.FindProperty("m_OpenCLPlatformID");
            var propOpenCLDeviceID = serializedObject.FindProperty("m_OpenCLDeviceID");

            #endregion serializedObject handling

            // Handle Inspector selection changes
            if (Selection.activeGameObject != null)
            {
                selectedObject = Selection.activeGameObject;

                var newPhysicMaterial = GetPhysicMaterial(selectedObject);
                if (newPhysicMaterial != null)
                {
                    physicMaterial = newPhysicMaterial;
                }
            }

            #region GUI rendering

            if (!m_StylesInitialized)
            {
                m_StylesInitialized = true;

                m_EnableOpenCLLabel = new GUIContent("Enabled");

                m_FoldoutStyle = new GUIStyle(EditorStyles.foldout);
                m_FoldoutStyle.fontStyle = FontStyle.Bold;

                m_HorizontalLine = new GUIStyle();
                m_HorizontalLine.normal.background = EditorGUIUtility.whiteTexture;
                m_HorizontalLine.margin = new RectOffset(0, 0, 4, 4);
                m_HorizontalLine.fixedHeight = 1;

                m_HorizontalLineColor = new Color(0.4f, 0.4f, 0.4f);
            }

            EditorGUILayout.Separator();

            selectedObject = (GameObject)EditorGUILayout.ObjectField("Input GameObject",
                selectedObject, typeof(GameObject), true);

            inputMesh = null;
            if (selectedObject != null)
            {
                MeshFilter meshFilter = selectedObject.GetComponent<MeshFilter>();
                if (meshFilter != null)
                {
                    inputMesh = meshFilter.sharedMesh;
                }
            }

            if (selectedObject == null)
            {
                EditorGUILayout.HelpBox("Select an input GameObject with a mesh", MessageType.Info);
            }
            else if (inputMesh == null)
            {
                EditorGUILayout.HelpBox("Selected GameObject does not have a mesh", MessageType.Warning);
            }

            EditorGUILayout.Separator();

            EditorGUILayout.PropertyField(propCreateColliders);

            if (propCreateColliders.boolValue)
            {
                physicMaterial = (PhysicMaterial)EditorGUILayout.ObjectField("Physic Material",
                    physicMaterial, typeof(PhysicMaterial), true);
            }

            // Disable MeshFilter/MeshRenderer creation if we are saving as an
            // asset so we don't have to deal with Material creation. This
            // could be revisited in the future if there's demand
            if (propCreateAsset.boolValue && propCreateMeshRenderers.boolValue)
            {
                propCreateMeshRenderers.boolValue = false;
            }

            EditorGUI.BeginDisabledGroup(propCreateAsset.boolValue);
            EditorGUILayout.PropertyField(propCreateMeshRenderers);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.PropertyField(propCreateAsset);

            EditorGUILayout.Separator();
            HorizontalRule();
            m_HullFoldout = EditorGUILayout.Foldout(m_HullFoldout, "Convex Hulls", m_FoldoutStyle);
            if (m_HullFoldout)
            {
                EditorGUILayout.PropertyField(propMaxHulls);
                EditorGUILayout.PropertyField(propMaxVertices);
            }

            HorizontalRule();
            m_AdvancedFoldout = EditorGUILayout.Foldout(m_AdvancedFoldout, "Advanced", m_FoldoutStyle);
            if (m_AdvancedFoldout)
            {
                EditorGUILayout.PropertyField(propResolution);
                EditorGUILayout.PropertyField(propMaxConcavity);
                EditorGUILayout.PropertyField(propPlaneDownsampling);
                EditorGUILayout.PropertyField(propHullDownsampling);
                EditorGUILayout.PropertyField(propSymmetryPlaneBias);
                EditorGUILayout.PropertyField(propRevolutionAxesBias);
                EditorGUILayout.PropertyField(propMeshNormalization);
                EditorGUILayout.PropertyField(propTetrahedronMode);
                EditorGUILayout.PropertyField(propMinVolume);
                EditorGUILayout.PropertyField(propApproximateHulls);
                EditorGUILayout.PropertyField(propProjectVertices);
            }

            if (m_HasOpenCL)
            {
                HorizontalRule();
                m_OpenCLFoldout = EditorGUILayout.Foldout(m_OpenCLFoldout, "OpenCL", m_FoldoutStyle);
                if (m_OpenCLFoldout)
                {
                    EditorGUILayout.PropertyField(propEnableOpenCL, m_EnableOpenCLLabel);

                    int platformID = EditorGUILayout.Popup("Platform",
                        propOpenCLPlatformID.intValue, m_OpenCLPlatforms.ToArray());
                    if (platformID != propOpenCLPlatformID.intValue)
                    {
                        m_OpenCLDevices = m_OpenCLPlatforms.Count > 0 ? VHCDAPI.GetDevices(0) : new List<string>();
                        propOpenCLDeviceID.intValue = 0;
                    }

                    propOpenCLDeviceID.intValue = EditorGUILayout.Popup("Device",
                            propOpenCLDeviceID.intValue, m_OpenCLDevices.ToArray());
                }
            }

            HorizontalRule();
            m_SettingsFoldout = EditorGUILayout.Foldout(m_SettingsFoldout, "Settings", m_FoldoutStyle);
            if (m_SettingsFoldout)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Clear Settings");
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Reset"))
                {
                    var window = EditorWindow.GetWindow<ConvexDecompositionEditorWindow>();
                    window.ResetPreferences(m_Settings);
                }
                GUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Save/Load Settings");
                GUILayout.FlexibleSpace();
                var buttonPosition = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight, Styles.iconButton);
                if (EditorGUI.DropdownButton(buttonPosition, Styles.presetIcon, FocusType.Passive, Styles.iconButton))
                {
                    var window = EditorWindow.GetWindow<ConvexDecompositionEditorWindow>();
                    window.ShowPresets();
                }
                EditorGUILayout.EndHorizontal();
            }

            #endregion GUI rendering

            // Store any updated settings
            serializedObject.ApplyModifiedProperties();
        }

        void HorizontalRule()
        {
            Color origColor = GUI.color;
            GUI.color = m_HorizontalLineColor;
            GUILayout.Box(GUIContent.none, m_HorizontalLine);
            GUI.color = origColor;
        }

        PhysicMaterial GetPhysicMaterial(GameObject go)
        {
            var collider = go.GetComponent<Collider>();
            return collider ? collider.sharedMaterial : null;
        }
    }
}
