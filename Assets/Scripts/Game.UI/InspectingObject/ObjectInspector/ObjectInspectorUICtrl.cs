using Game.Common;
using Reflex.Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI.InspectingObject
{
    public class ObjectInspectorUICtrl : SaiMonoBehaviour, IInstaller
    {
        [SerializeField] private UIDocument _document;

        public UIDocument UIDocument => _document;

        public void InstallBindings(ContainerBuilder builder)
        {
            builder.RegisterValue(this);
        }

        protected override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadComponentInCtrl(out _document);
        }

        private void Start()
        {
            this.Hide();
        }

        public void Show()
        {
            var root = _document.rootVisualElement;

            root.RemoveFromClassList("anim--hidden");
            root.AddToClassList("anim--visible");
        }

        public void Hide()
        {
            var root = _document.rootVisualElement;

            root.RemoveFromClassList("anim--visible");
            root.AddToClassList("anim--hidden");
        }
    }
}