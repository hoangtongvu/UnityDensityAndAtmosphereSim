using ZBase.Foundation.PubSub;

namespace Game.Domain.InspectingObject;

public record struct SpawnInspectingObjectMessage(int ObjectId) : IMessage;