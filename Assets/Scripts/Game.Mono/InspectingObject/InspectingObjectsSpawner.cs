using Game.Common;
using Game.Domain;
using Game.Domain.InspectingObject;
using Game.Domain.PubSub.Messengers;
using Game.Mono.Player;
using Game.ScriptableObjects;
using Reflex.Attributes;
using Reflex.Extensions;
using Reflex.Injectors;
using UnityEngine;
using ZBase.Foundation.PubSub;

namespace Game.Mono.InspectingObject
{
    public class InspectingObjectsSpawner : SaiMonoBehaviour
    {
        [Inject] private InspectingObjectsSO inspectingObjectsSO;
        [Inject] private PlayerCtrl playerCtrl;
        private ISubscription spawnObjectMessageSub;

        [SerializeField] private Vector3 spawnOffset = new(0f, 5f, 5f);

        private void OnEnable()
        {
            this.spawnObjectMessageSub = GameplayMessenger.MessageSubscriber
                .Subscribe<SpawnInspectingObjectMessage>(this.SpawnObject);
        }

        private void OnDisable()
        {
            this.spawnObjectMessageSub.Dispose();
        }

        private void SpawnObject(SpawnInspectingObjectMessage message)
        {
            var objectData = this.inspectingObjectsSO.Value[message.ObjectId];

            var spawnPos = this.playerCtrl.transform.position
                + this.playerCtrl.transform.TransformDirection(this.spawnOffset);
            var newObject = Instantiate(objectData.Prefab, spawnPos, Quaternion.identity, transform);
            newObject.GetComponent<InspectingObjectIdHolder>().Value = message.ObjectId;

            GameObjectInjector.InjectRecursive(newObject, gameObject.scene.GetSceneContainer());
        }
    }
}