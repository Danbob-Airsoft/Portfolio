using UnityEngine;
using UnityEditor;
using UnityEditor.Presets;

namespace CMR
{
    public class ConvexDecompositionEditorWindow : EditorWindow

    {
        ConvexDecompositionEditor m_SettingsEditor;
        ConvexDecompositionSettings m_SerializedSettings;

        #region Editor Preferences

        public bool createAsset
        {
            get { return EditorPrefs.GetBool("ConvexDecomposition_createAsset", false); }
            set { EditorPrefs.SetBool("ConvexDecomposition_createAsset", value); }
        }
        public bool createColliders
        {
            get { return EditorPrefs.GetBool("ConvexDecomposition_createColliders", true); }
            set { EditorPrefs.SetBool("ConvexDecomposition_createColliders", value); }
        }
        public bool createMeshRenderers
        {
            get { return EditorPrefs.GetBool("ConvexDecomposition_createMeshRenderers", false); }
            set { EditorPrefs.SetBool("ConvexDecomposition_createMeshRenderers", value); }
        }
        public int resolution
        {
            get { return EditorPrefs.GetInt("ConvexDecomposition_resolution", 50); }
            set { EditorPrefs.SetInt("ConvexDecomposition_resolution", value); }
        }
        public int maxHulls
        {
            get { return EditorPrefs.GetInt("ConvexDecomposition_maxHulls", 16); }
            set { EditorPrefs.SetInt("ConvexDecomposition_maxHulls", value); }
        }
        public float maxConcavity
        {
            get { return EditorPrefs.GetFloat("ConvexDecomposition_maxConcavity", 0.001f); }
            set { EditorPrefs.SetFloat("ConvexDecomposition_maxConcavity", value); }
        }
        public int planeDownsampling
        {
            get { return EditorPrefs.GetInt("ConvexDecomposition_planeDownsampling", 4); }
            set { EditorPrefs.SetInt("ConvexDecomposition_planeDownsampling", value); }
        }
        public int hullDownsampling
        {
            get { return EditorPrefs.GetInt("ConvexDecomposition_hullDownsampling", 4); }
            set { EditorPrefs.SetInt("ConvexDecomposition_hullDownsampling", value); }
        }
        public float symmetryPlaneBias
        {
            get { return EditorPrefs.GetFloat("ConvexDecomposition_symmetryPlaneBias", 0.05f); }
            set { EditorPrefs.SetFloat("ConvexDecomposition_symmetryPlaneBias", value); }
        }
        public float revolutionAxesBias
        {
            get { return EditorPrefs.GetFloat("ConvexDecomposition_revolutionAxesBias", 0.05f); }
            set { EditorPrefs.SetFloat("ConvexDecomposition_revolutionAxesBias", value); }
        }
        public bool meshNormalization
        {
            get { return EditorPrefs.GetBool("ConvexDecomposition_meshNormalization", true); }
            set { EditorPrefs.SetBool("ConvexDecomposition_meshNormalization", value); }
        }
        public bool tetrahedronMode
        {
            get { return EditorPrefs.GetBool("ConvexDecomposition_tetrahedronMode", false); }
            set { EditorPrefs.SetBool("ConvexDecomposition_tetrahedronMode", value); }
        }
        public int maxVertices
        {
            get { return EditorPrefs.GetInt("ConvexDecomposition_maxVertices", 64); }
            set { EditorPrefs.SetInt("ConvexDecomposition_maxVertices", value); }
        }
        public float minVolume
        {
            get { return EditorPrefs.GetFloat("ConvexDecomposition_minVolume", 0.0001f); }
            set { EditorPrefs.SetFloat("ConvexDecomposition_minVolume", value); }
        }
        public bool approximateHulls
        {
            get { return EditorPrefs.GetBool("ConvexDecomposition_approximateHulls", true); }
            set { EditorPrefs.SetBool("ConvexDecomposition_approximateHulls", value); }
        }
        public bool projectVertices
        {
            get { return EditorPrefs.GetBool("ConvexDecomposition_projectVertices", true); }
            set { EditorPrefs.SetBool("ConvexDecomposition_projectVertices", value); }
        }
        public bool enableOpenCL
        {
            get { return EditorPrefs.GetBool("ConvexDecomposition_enableOpenCL", true); }
            set { EditorPrefs.SetBool("ConvexDecomposition_enableOpenCL", value); }
        }
        public int openCLPlatformID
        {
            get { return EditorPrefs.GetInt("ConvexDecomposition_openCLPlatformID", 0); }
            set { EditorPrefs.SetInt("ConvexDecomposition_openCLPlatformID", value); }
        }
        public int openCLDeviceID
        {
            get { return EditorPrefs.GetInt("ConvexDecomposition_openCLDeviceID", 0); }
            set { EditorPrefs.SetInt("ConvexDecomposition_openCLDeviceID", value); }
        }

        #endregion

        // Method to open the window
        [MenuItem("Window/Convex Hull Generator")]
        static void OpenWindow()
        {
            GetWindow<ConvexDecompositionEditorWindow>(false, "Convex Hull Generator");
        }

        public void ResetPreferences(ConvexDecompositionSettings settings)
        {
            EditorPrefs.DeleteKey("ConvexDecomposition_createAsset");
            EditorPrefs.DeleteKey("ConvexDecomposition_createColliders");
            EditorPrefs.DeleteKey("ConvexDecomposition_createMeshRenderers");
            EditorPrefs.DeleteKey("ConvexDecomposition_resolution");
            EditorPrefs.DeleteKey("ConvexDecomposition_maxHulls");
            EditorPrefs.DeleteKey("ConvexDecomposition_maxConcavity");
            EditorPrefs.DeleteKey("ConvexDecomposition_planeDownsampling");
            EditorPrefs.DeleteKey("ConvexDecomposition_hullDownsampling");
            EditorPrefs.DeleteKey("ConvexDecomposition_symmetryPlaneBias");
            EditorPrefs.DeleteKey("ConvexDecomposition_revolutionAxesBias");
            EditorPrefs.DeleteKey("ConvexDecomposition_meshNormalization");
            EditorPrefs.DeleteKey("ConvexDecomposition_tetrahedronMode");
            EditorPrefs.DeleteKey("ConvexDecomposition_maxVertices");
            EditorPrefs.DeleteKey("ConvexDecomposition_minVolume");
            EditorPrefs.DeleteKey("ConvexDecomposition_approximateHulls");
            EditorPrefs.DeleteKey("ConvexDecomposition_projectVertices");
            EditorPrefs.DeleteKey("ConvexDecomposition_enableOpenCL");
            EditorPrefs.DeleteKey("ConvexDecomposition_openCLPlatformID");
            EditorPrefs.DeleteKey("ConvexDecomposition_openCLDeviceID");

            settings.Init(this);
        }

        public void ShowPresets()
        {
            var presetReceiver = ScriptableObject.CreateInstance<ConvexDecompositionSettingsReceiver>();
            presetReceiver.Init(m_SerializedSettings, this);
            PresetSelector.ShowSelector(m_SerializedSettings, null, true, presetReceiver);
        }

        void OnEnable()
        {
            // Create your settings now and its associated Inspector
            // that allows to create only one custom Inspector for the settings in the window and the Preset.
            m_SerializedSettings = ScriptableObject.CreateInstance<ConvexDecompositionSettings>();
            m_SerializedSettings.Init(this);
            m_SettingsEditor = (ConvexDecompositionEditor)Editor.CreateEditor(m_SerializedSettings, typeof(ConvexDecompositionEditor));

            Selection.selectionChanged += SelectionChangedHandler;
        }

        void OnDisable()
        {
            Selection.selectionChanged -= SelectionChangedHandler;

            Object.DestroyImmediate(m_SerializedSettings);
            Object.DestroyImmediate(m_SettingsEditor);
        }

        void OnGUI()
        {
            // Main window UI. The actual rendering is done in the ConvexDecompositionEditor class
            EditorGUI.BeginChangeCheck();
            m_SettingsEditor.OnInspectorGUI();
            if (EditorGUI.EndChangeCheck())
            {
                // Apply changes made in the settings editor to our instance.
                m_SerializedSettings.ApplySettings(this);
            }

            EditorGUILayout.Separator();
            if (m_SettingsEditor.inputMesh != null && (createAsset || createColliders || createMeshRenderers))
            {
                if (GUILayout.Button("Bake"))
                {
                    VHACDSession session = new VHACDSession();
                    session.maxConvexHulls = maxHulls;
                    session.resolution = resolution * 10000;
                    session.concavity = maxConcavity;
                    session.planeDownsampling = planeDownsampling;
                    session.convexhullDownsampling = hullDownsampling;
                    session.alpha = symmetryPlaneBias;
                    session.beta = revolutionAxesBias;
                    session.pca = meshNormalization ? 1 : 0;
                    session.mode = tetrahedronMode ? 1 : 0;
                    session.maxNumVerticesPerCH = maxVertices;
                    session.minVolumePerCH = minVolume;
                    session.convexHullApproximation = approximateHulls ? 1 : 0;
                    session.projectHullVertices = projectVertices ? 1 : 0;
                    session.oclAcceleration = enableOpenCL ? 1 : 0;
                    session.oclPlatformID = openCLPlatformID;
                    session.oclDeviceID = openCLDeviceID;

                    ConvexDecomposition.Bake(m_SettingsEditor.selectedObject, session,
                        m_SettingsEditor.physicMaterial, createAsset, createColliders, createMeshRenderers);
                }
            }
            else
            {
                GUI.enabled = false;
                GUILayout.Button("Bake");
                GUI.enabled = true;
            }
        }

        void SelectionChangedHandler()
        {
            Repaint();
        }
    }
}
