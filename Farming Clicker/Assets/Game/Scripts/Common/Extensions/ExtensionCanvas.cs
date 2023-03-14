using System.Linq;
using UnityEngine;

namespace Game.Scripts.Common.Extensions
{
    public static class ExtensionCanvas
    {
        public static void SetActive(this CanvasGroup canvasGroup, bool isActive)
        {
            canvasGroup.alpha = isActive ? 1 : 0;
            canvasGroup.interactable = isActive;
            canvasGroup.blocksRaycasts = isActive;
        }
 
        public static bool Interactable(this CanvasGroup[] canvasGroups)
        {
            return canvasGroups != null && canvasGroups.Length > 0 && canvasGroups.All(x => x.interactable);
        }
    }
}