using ZBase.Foundation.PubSub;

namespace Game.Domain.WorldFluid;

public record struct ChangeFluidKindMessage(WorldFluidId FluidId) : IMessage;