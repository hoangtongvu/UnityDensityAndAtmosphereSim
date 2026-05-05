using Game.Domain;
using Game.Domain.InspectingObject;
using Game.Domain.Player;
using Game.Domain.PubSub.Messengers;
using Game.ScriptableObjects;
using Reflex.Attributes;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;
using ZBase.Foundation.PubSub;

namespace Game.UI.InspectingObject
{
    public class InspectingObjectListUICtrl : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        [SerializeField] private Sprite _placeholderSprite;
        [Inject] private InspectingObjectsSO _objectsSO;

        private VisualElement _scrollContent;
        private VisualElement _tooltip;
        private Label _tooltipName;
        private Label _tooltipDensity;
        private Label _tooltipVolume;
        private Label _tooltipWeight;

        private void Start()
        {
            var root = _document.rootVisualElement;
            root.visible = false;
        }

        private void OnEnable()
        {
            var root = _document.rootVisualElement;

            _scrollContent = root.Q<ScrollView>("items-scroll");
            _tooltip = root.Q<VisualElement>("item-tooltip");
            _tooltipName = root.Q<Label>("tooltip-name");
            _tooltipDensity = root.Q<Label>("tooltip-density");
            _tooltipVolume = root.Q<Label>("tooltip-volume");
            _tooltipWeight = root.Q<Label>("tooltip-weight");

            BuildList();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F2))
            {
                // Toggle
                var root = _document.rootVisualElement;
                root.visible = !root.visible;

                var publisher = GameplayMessenger.MessagePublisher;

                if (root.visible)
                {
                    publisher.Publish(new SetEnabledMouseLookMessage(false));
                    publisher.Publish(new SetLockCursorMessage(CursorLockMode.None));
                }
                else
                {
                    publisher.Publish(new SetEnabledMouseLookMessage(true));
                    publisher.Publish(new SetLockCursorMessage(CursorLockMode.Locked));
                }
            }
        }

        private void BuildList()
        {
            _scrollContent.Clear();

            if (_objectsSO == null) return;

            foreach (var kvp in _objectsSO.Value)
            {
                int id = kvp.Key;
                var data = kvp.Value;

                var item = CreateFluidItem(id, data);
                _scrollContent.Add(item);
            }
        }

        private VisualElement CreateFluidItem(int id, InspectingObjectData data)
        {
            // ── Container ────────────────────────────────────────────────────────
            var item = new VisualElement();
            item.AddToClassList("fluid-item");

            // ── Sprite (placeholder image element) ───────────────────────────────
            var sprite = new VisualElement();
            sprite.AddToClassList("fluid-sprite");
            if (_placeholderSprite != null)
                sprite.style.backgroundImage = new StyleBackground(_placeholderSprite);
            item.Add(sprite);

            // ── Name label ───────────────────────────────────────────────────────
            var nameLabel = new Label(data.Name);
            nameLabel.AddToClassList("fluid-name");
            item.Add(nameLabel);

            // ── Hover → show tooltip ─────────────────────────────────────────────
            item.RegisterCallback<MouseEnterEvent>(evt =>
            {
                PopulateTooltip(data);
                PositionTooltip(evt.mousePosition);
                _tooltip.RemoveFromClassList("hidden");
            });

            item.RegisterCallback<MouseMoveEvent>(evt =>
            {
                PositionTooltip(evt.mousePosition);
            });

            item.RegisterCallback<MouseLeaveEvent>(_ =>
            {
                _tooltip.AddToClassList("hidden");
            });

            // ── Click ────────────────────────────────────────────────────────────
            item.RegisterCallback<ClickEvent>(_ => OnFluidClicked(id, data));

            return item;
        }

        // ── Tooltip helpers ──────────────────────────────────────────────────────
        private void PopulateTooltip(InspectingObjectData data)
        {
            _tooltipName.text = data.Name;
            _tooltipDensity.text = $"{data.Density:F2} kg/m³";
            _tooltipVolume.text = $"{data.MeshVolume:F2} m³";
            _tooltipWeight.text = $"{data.Weight:F2} kg";
            // Add more fields here as FluidData grows, following the same pattern.
        }

        private void PositionTooltip(Vector2 mousePos)
        {
            // Offset so the tooltip doesn't sit under the cursor.
            const float offsetX = 16f;
            const float offsetY = -8f;
            _tooltip.style.left = mousePos.x + offsetX;
            _tooltip.style.top = mousePos.y + offsetY;
        }

        private void OnFluidClicked(int id, InspectingObjectData data)
        {
            UnityEngine.Debug.Log("Clicked");
            GameplayMessenger.MessagePublisher
                .Publish(new SpawnInspectingObjectMessage(id));
        }
    }
}