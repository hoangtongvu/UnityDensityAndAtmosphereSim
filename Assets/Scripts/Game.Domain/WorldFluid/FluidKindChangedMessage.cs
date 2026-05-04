using ZBase.Foundation.PubSub;

namespace Game.Domain.WorldFluid;

public record struct FluidKindChangedMessage(WorldFluidId FluidId) : IMessage;