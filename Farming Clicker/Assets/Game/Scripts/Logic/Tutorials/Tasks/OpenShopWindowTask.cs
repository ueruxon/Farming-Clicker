using UnityEngine;

namespace Game.Scripts.Logic.Tutorials.Tasks
{
    [CreateAssetMenu(menuName = "Tutorial/Tasks/OpenShopWindow", fileName = "New Task")]
    public class OpenShopWindowTask : TutorialTask
    {
        public override void OnStart()
        {
            UIFactory.ShopOpened += OnShopWindowOpened;
            base.OnStart();
        }
        
        private void OnShopWindowOpened() => 
            OnComplete();

        public override void OnComplete()
        {
            base.OnComplete();
            UIFactory.ShopOpened -= OnShopWindowOpened;
        }
    }
}