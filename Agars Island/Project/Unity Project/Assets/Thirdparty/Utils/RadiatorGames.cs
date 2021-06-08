using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Radiator Games Library
///
/// Created by: Lewis Arnold
/// Edited by:
/// </summary>
namespace RadiatorGames
{
    namespace UI
    {
        namespace SwapCanvasGroup
        {
            public class GroupSwapper
            {
                public static void ShowCanvasGroup(CanvasGroup canvasGroup)
                {
                    canvasGroup.alpha = 1.0f;
                    canvasGroup.interactable = true;
                    canvasGroup.blocksRaycasts = true;
                }

                public static void HideCanvasGroup(CanvasGroup canvasGroup)
                {
                    canvasGroup.alpha = 0.0f;
                    canvasGroup.interactable = false;
                    canvasGroup.blocksRaycasts = false;
                }
            }
        }
    }

    namespace Sanity
    {
        public class SanityTime
        {
            public static void SetTimeScale(float timeScale)
            {
                Time.timeScale = timeScale;
            }
        }
    }

    namespace Math
    {
        public class Mapping
        {
            /// <summary>
            /// Maps value X between A and B, to a value (return value) between C and D
            /// </summary>
            /// <param name="A"></param> Start of 1st range
            /// <param name="B"></param> End of 1st range
            /// <param name="C"></param> Start of 2nd range
            /// <param name="D"></param> End of 2nd range
            /// <param name="X"></param> Value in 1st range
            /// <returns></returns>
            public static float Map(float A, float B, float C, float D, float X)
            {
                return (X - A) / (B - A) * (D - C) + C;
            }
        }
    }

    namespace util
    {
        public class componentUtil : MonoBehaviour
        {
            private T GetComponentInChildOnly<T>(GameObject parent) where T : Component
            {
                T[] comps = parent.GetComponentsInChildren<T>();

                if (comps.Length > 1)
                {
                    for (int i = 1; i < comps.Length; i++)
                    {
                        T comp = comps[i];
                        if (comp.gameObject.GetInstanceID() != GetInstanceID())
                        {
                            return comp;
                        }
                    }
                }

                return null;
            }
        }
    }
}