using UnityEngine;

namespace CMR
{
    public class ConvexDecompositionSettings : ScriptableObject
    {
        [SerializeField]
        [Tooltip("Create an asset containing all of the convex hulls for use in prefabs, other objects, or exporting")]
        bool m_CreateAsset;
        [SerializeField]
        [Tooltip("Create MeshCollider components (required for physics)")]
        bool m_CreateColliders;
        [SerializeField]
        [Tooltip("Create MeshFilter and MeshRenderer components with random colors (useful for debugging)")]
        bool m_CreateMeshRenderers;

        [SerializeField]
        [Range(1, 1600)]
        [Tooltip("Maximum number of voxels generated during the voxelization stage (in units of 10,000)")]
        int m_Resolution;
        [SerializeField]
        [Range(1, 512)]
        [Tooltip("Maximum number of convex hulls to produce")]
        int m_MaxHulls;
        [SerializeField]
        [Range(0.0f, 1.0f)]
        [Tooltip("Maximum allowed concavity")]
        float m_MaxConcavity;
        [SerializeField]
        [Range(1, 16)]
        [Tooltip("Controls the granularity of the search for the \"best\" clipping plane")]
        int m_PlaneDownsampling;
        [SerializeField]
        [Range(1, 16)]
        [Tooltip("Controls the precision of the convex hulls during the clipping plane selection stage")]
        int m_HullDownsampling;
        [SerializeField]
        [Range(0.0f, 1.0f)]
        [Tooltip("Controls the bias toward clipping along symmetry planes")]
        float m_SymmetryPlaneBias;
        [SerializeField]
        [Range(0.0f, 1.0f)]
        [Tooltip("Controls the bias toward clipping along revolution axes")]
        float m_RevolutionAxesBias;
        [SerializeField]
        [Tooltip("Normalize the mesh before applying the convex decomposition")]
        bool m_MeshNormalization;
        [SerializeField]
        [Tooltip("Use tetrahedron-based approximate convex decomposition. Otherwise, use a voxel-based method")]
        bool m_TetrahedronMode;
        [SerializeField]
        [Range(3, 255)]
        [Tooltip("Maximum number of triangles per convex hull")]
        int m_MaxVertices;
        [SerializeField]
        [Range(0.0f, 0.01f)]
        [Tooltip("Controls the adaptive sampling of the generated convex hulls")]
        float m_MinVolume;
        [SerializeField]
        [Tooltip("Enable approximation when computing convex hulls")]
        bool m_ApproximateHulls;
        [SerializeField]
        [Tooltip("Project convex hull vertices onto the original source mesh to increase floating point accuracy")]
        bool m_ProjectVertices;
        [SerializeField]
        [Tooltip("Enable OpenCL acceleration")]
        bool m_EnableOpenCL;
        [SerializeField]
        int m_OpenCLPlatformID;
        [SerializeField]
        int m_OpenCLDeviceID;

        public void Init(ConvexDecompositionEditorWindow window)
        {
            m_CreateAsset = window.createAsset;
            m_CreateColliders = window.createColliders;
            m_CreateMeshRenderers = window.createMeshRenderers;
            m_Resolution = window.resolution;
            m_MaxHulls = window.maxHulls;
            m_MaxConcavity = window.maxConcavity;
            m_PlaneDownsampling = window.planeDownsampling;
            m_HullDownsampling = window.hullDownsampling;
            m_SymmetryPlaneBias = window.symmetryPlaneBias;
            m_RevolutionAxesBias = window.revolutionAxesBias;
            m_MeshNormalization = window.meshNormalization;
            m_TetrahedronMode = window.tetrahedronMode;
            m_MaxVertices = window.maxVertices;
            m_MinVolume = window.minVolume;
            m_ApproximateHulls = window.approximateHulls;
            m_ProjectVertices = window.projectVertices;
            m_EnableOpenCL = window.enableOpenCL;
            m_OpenCLPlatformID = window.openCLPlatformID;
            m_OpenCLDeviceID = window.openCLDeviceID;
        }

        public void ApplySettings(ConvexDecompositionEditorWindow window)
        {
            window.createAsset = m_CreateAsset;
            window.createColliders = m_CreateColliders;
            window.createMeshRenderers = m_CreateMeshRenderers;
            window.resolution = m_Resolution;
            window.maxHulls = m_MaxHulls;
            window.maxConcavity = m_MaxConcavity;
            window.planeDownsampling = m_PlaneDownsampling;
            window.hullDownsampling = m_HullDownsampling;
            window.symmetryPlaneBias = m_SymmetryPlaneBias;
            window.revolutionAxesBias = m_RevolutionAxesBias;
            window.meshNormalization = m_MeshNormalization;
            window.tetrahedronMode = m_TetrahedronMode;
            window.maxVertices = m_MaxVertices;
            window.minVolume = m_MinVolume;
            window.approximateHulls = m_ApproximateHulls;
            window.projectVertices = m_ProjectVertices;
            window.enableOpenCL = m_EnableOpenCL;
            window.openCLPlatformID = m_OpenCLPlatformID;
            window.openCLDeviceID = m_OpenCLDeviceID;

            window.Repaint();
        }
    }
}
