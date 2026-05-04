using Game.Domain;
using Game.Domain.PubSub.Messengers;
using Game.Domain.WorldFluid;
using Game.ScriptableObjects;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UIElements;
using ZBase.Foundation.PubSub;

namespace MyNamespace
{
    public class FluidsListUICtrl : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        [SerializeField] private Sprite _placeholderSprite;
        [Inject] private FluidsSO _fluidsSO;

        private VisualElement _scrollContent;
        private VisualElement _tooltip;
        private Label _tooltipName;
        private Label _tooltipDensity;

        private void Start()
        {
            var root = _document.rootVisualElement;
            root.visible = false;
        }

        private void OnEnable()
        {
            var root = _document.rootVisualElement;

            _scrollContent = root.Q<ScrollView>("fluids-scroll");
            _tooltip = root.Q<VisualElement>("fluid-tooltip");
            _tooltipName = root.Q<Label>("tooltip-name");
            _tooltipDensity = root.Q<Label>("tooltip-density");

            BuildList();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                // Toggle
                var root = _document.rootVisualElement;
                root.visible = !root.visible;

                UnityEngine.Cursor.lockState = root.visible ? CursorLockMode.None : CursorLockMode.Locked;
            }
        }

        private void BuildList()
        {
            _scrollContent.Clear();

            if (_fluidsSO == null) return;

            foreach (var kvp in _fluidsSO.Value)
            {
                int id = kvp.Key;
                FluidData data = kvp.Value;

                var item = CreateFluidItem(id, data);
                _scrollContent.Add(item);
            }
        }

        private VisualElement CreateFluidItem(int id, FluidData data)
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
        private void PopulateTooltip(FluidData data)
        {
            _tooltipName.text = data.Name;
            _tooltipDensity.text = $"{data.Density:F2} kg/m³";
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

        private void OnFluidClicked(int id, FluidData data)
        {
            GameplayMessenger.MessagePublisher.Publish(new ChangeFluidKindMessage(new()
            {
                Value = id,
            }));
        }
    }
}