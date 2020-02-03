using System.Collections.Generic;
using Google.Protobuf;
using Proto;

public class ServerPerceptionView : UPerceptionView
{

    #region views
    private readonly Dictionary<int, UElementView> _AttachElements = new Dictionary<int, UElementView>();

    internal void DeAttachView(UElementView battleElement)
    {
        AddNotify(new Notify_ElementExitState { Index = battleElement.index });
        _AttachElements.Remove(battleElement.index);
    }

    internal void AttachView(UElementView battleElement)
    {
        AddNotify(battleElement.ToInitNotify());
        AddNotify(new Notify_ElementJoinState { Index = battleElement.index });
        _AttachElements.Add(battleElement.index, battleElement);
    }
    #endregion

    #region notify
    private Queue<IMessage> _notify = new Queue<IMessage>();

    public void AddNotify(IMessage notify)
    {
        _notify.Enqueue(notify);
    }

    public IMessage[] GetAndClearNotify()
    {
        if (_notify.Count > 0)
        {
            var list = _notify.ToArray();
            _notify.Clear();
            return list;
        }
        else
            return new IMessage[0];
    }

    public IMessage[] GetInitNotify()
    {
        var list = new List<IMessage>();
        foreach (var i in _AttachElements)
        {
            var sElement = i.Value as ISerializerableElement;
            if (sElement != null)
            {
                list.Add(sElement.ToInitNotify());
                list.Add(new Notify_ElementJoinState { Index = i.Key });
            }
        }
        return list.ToArray();
    }
    #endregion

}
