using UnityEngine;
using ZBase.Foundation.PubSub;

namespace Game.Domain.Player;

public record struct SetLockCursorMessage(CursorLockMode LockMode) : IMessage;