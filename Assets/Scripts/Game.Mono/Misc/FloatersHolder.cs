using Game.Common;
using System.Collections.Generic;

namespace Game.Mono
{
    public class FloatersHolder : SaiMonoBehaviour
    {
        public List<Floater> Value = new();

        protected override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadFloaterList();
        }

        private void LoadFloaterList()
        {
            this.Value = new(GetComponentsInChildren<Floater>());
        }

        public void SetSubmergedHeight(float value)
        {
            foreach (var floater in this.Value)
            {
                floater.depthBefSub = value;
            }
        }

        public void SetDisplacementAmt(float value)
        {
            foreach (var floater in this.Value)
            {
                floater.displacementAmt = value;
            }
        }
    }
}