using ZBase.Foundation.PubSub;

namespace Game.Domain;

public record struct FluidKindChangedMessage(int FluidId) : IMessage;