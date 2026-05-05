using ZBase.Foundation.PubSub;

namespace Game.Domain.Player;

public record struct SetEnabledMouseLookMessage(bool Value) : IMessage;