using UnityEngine;

namespace Game.Common;

public abstract class SaiMonoBehaviour : MonoBehaviour
{
#if (UNITY_EDITOR)
    [ContextMenu("Load Components")]
    private void LoadComponentButton()
    {
        this.LoadComponents();
        Debug.Log($"Loaded Components in {gameObject.name}.", this);
    }
#endif

    protected virtual void Reset()
    {
        this.LoadComponents();
        this.ResetValues();
    }

    /// <summary>
    /// For overriding
    /// </summary>
    protected virtual void LoadComponents() { }

    /// <summary>
    /// Use to Reset Value (Reset button).
    /// For overriding
    /// </summary>
    protected virtual void ResetValues() { }

    protected void LoadComponentInChildren<TComponent>(out TComponent component)
        where TComponent : Component
    {
        component = GetComponentInChildren<TComponent>();
        if (component != null) return;

        Debug.LogError($"Can't load component of type {nameof(TComponent)}", gameObject);
    }

    protected void LoadComponentInCtrl<TComponent>(out TComponent component)
        where TComponent : Component
    {
        component = GetComponent<TComponent>();
        if (component != null) return;

        Debug.LogError($"Can't load component of type {nameof(TComponent)}", gameObject);
    }

    protected void LoadCtrl<TComponent>(out TComponent component)
        where TComponent : Component
    {
        component = GetComponentInParent<TComponent>();
        if (component != null) return;

        Debug.LogError($"Can't load component of type {nameof(TComponent)}", gameObject);
    }

    protected void LoadTransformInChildrenByName(out Transform t, string name)
    {
        t = transform.Find(name);
        if (t != null) return;

        Debug.LogError($"Can't find child Transform with name {name}", gameObject);
    }

    protected void FindFirstObjectByType<T>(out T t)
        where T : UnityEngine.Object
    {
        t = UnityEngine.Object.FindFirstObjectByType<T>();
    }

    protected void FindAnyObjectByType<T>(out T t)
        where T : UnityEngine.Object
    {
        t = UnityEngine.Object.FindAnyObjectByType<T>();
    }

    protected void FindObjectsByType<T>(out T[] tArray, FindObjectsSortMode findObjectsSortMode = FindObjectsSortMode.None)
        where T : UnityEngine.Object
    {
        tArray = UnityEngine.Object.FindObjectsByType<T>(findObjectsSortMode);
    }
}