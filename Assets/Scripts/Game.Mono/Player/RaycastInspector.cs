using Game.Domain;
using Game.Domain.PubSub.Messengers;
using Game.Domain.WorldFluid;
using Game.Mono.HydrostaticPressure;
using Game.ScriptableObjects;
using Game.UI.InspectingObject;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UIElements;
using ZBase.Foundation.PubSub;

namespace Game.Mono.Player
{
    [SourceGeneratorInjectable]
    public partial class RaycastInspector : MonoBehaviour
    {
        public float rayLength = 10f;
        public LayerMask inspectingObjectLayer;

        private Camera _mainCamera;
        private GameObject _currentObject;
        private HydrostaticPressureCalculator _currentHydrostaticPressureCalculator;

        [Inject] private FluidsSO _fluidsSO;
        [Inject] private WorldFluidId _worldFluidId;
        [Inject] private InspectingObjectsSO _inspectingObjectsSO;
        [Inject] private ObjectInspectorUICtrl _objectInspectorUI;
        private ISubscription _changeFluidMessageSub;

        private void Awake()
        {
            _mainCamera = Camera.main;

            if (_mainCamera == null)
                Debug.LogError("[RaycastInspector] No main camera found in the scene.");

            // Auto-resolve the layer if the Inspector left the mask at Nothing.
            if (inspectingObjectLayer.value == 0)
            {
                int layerIndex = LayerMask.NameToLayer("InspectingObject");
                if (layerIndex == -1)
                    Debug.LogWarning("[RaycastInspector] Layer 'InspectingObject' not found. " +
                                     "Create it in Edit → Project Settings → Tags & Layers.");
                else
                    inspectingObjectLayer = 1 << layerIndex;
            }
        }

        private void OnEnable()
        {
            _changeFluidMessageSub = GameplayMessenger.MessageSubscriber
                .Subscribe<ChangeFluidKindMessage>(message =>
                {
                    if (!_currentObject) return;

                    var root = _objectInspectorUI.UIDocument.rootVisualElement;
                    var inspectingObjectId = _currentObject.GetComponent<InspectingObjectIdHolder>().Value;

                    var fluidData = _fluidsSO.Value[_worldFluidId.Value];
                    var objectData = _inspectingObjectsSO.Value[inspectingObjectId];

                    root.Q<Label>("field-submerged-volume").text = $"{objectData.Density / fluidData.Density:F2}";
                });
        }

        private void OnDisable()
        {
            _changeFluidMessageSub.Dispose();
        }

        private void FixedUpdate()
        {
            if (_mainCamera == null) return;

            Ray ray = new Ray(_mainCamera.transform.position, _mainCamera.transform.forward);
            bool hit = Physics.Raycast(ray, out RaycastHit hitInfo, rayLength, inspectingObjectLayer);

#if UNITY_EDITOR
            Debug.DrawRay(ray.origin, ray.direction * rayLength, hit ? Color.green : Color.red);
#endif

            GameObject hitObject = hit ? hitInfo.collider.gameObject : null;

            if (hitObject != _currentObject)
            {
                if (_currentObject != null)
                    OnDrop(_currentObject);

                _currentObject = hitObject;

                if (_currentObject != null)
                    OnGrab(_currentObject);
            }

            if (_currentObject)
            {
                var root = _objectInspectorUI.UIDocument.rootVisualElement;
                root.Q<Label>("field-depth").text = $"{_currentHydrostaticPressureCalculator.depth:F2}";
                root.Q<Label>("field-pressure").text = $"{_currentHydrostaticPressureCalculator.pressure:F2}";
            }
        }

        protected virtual void OnGrab(GameObject target)
        {
            var root = _objectInspectorUI.UIDocument.rootVisualElement;
            var inspectingObjectId = target.GetComponent<InspectingObjectIdHolder>().Value;
            _currentHydrostaticPressureCalculator = target.GetComponent<HydrostaticPressureCalculator>();

            var fluidData = _fluidsSO.Value[_worldFluidId.Value];
            var objectData = _inspectingObjectsSO.Value[inspectingObjectId];

            root.Q<Label>("field-name").text = objectData.Name;
            root.Q<Label>("field-density").text = $"{objectData.Density:F2}";
            root.Q<Label>("field-volume").text = $"{objectData.MeshVolume:F2}";
            root.Q<Label>("field-weight").text = $"{objectData.Weight:F2}";
            root.Q<Label>("field-submerged-volume").text =
                $"{Mathf.Clamp(objectData.Density / fluidData.Density * 100, 0, 100):F2}";

            _objectInspectorUI.Show();
        }

        protected virtual void OnDrop(GameObject target)
        {
            _objectInspectorUI.Hide();
        }
    }
}