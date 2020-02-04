using System.Collections.Generic;
using Google.Protobuf;
using Proto;

public class ServerPerceptionView : UPerceptionView
{


    #region views

    private readonly Queue<IMessage> _notify = new Queue<IMessage>();

    private readonly Dictionary<int, UElementView> AttachElements = new Dictionary<int, UElementView>();

    public override void DeAttachView(UElementView battleElement)
    {
        AddNotify(new Notify_ElementExitState { Index = battleElement.Index });
        AttachElements.Remove(battleElement.Index);
    }

    public override void AttachView(UElementView battleElement)
    {
        AddNotify(battleElement.ToInitNotify());
        AddNotify(new Notify_ElementJoinState { Index = battleElement.Index });
        AttachElements.Add(battleElement.Index, battleElement);
    }

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
        foreach (var i in AttachElements)
        {
            if (i.Value is ISerializerableElement sElement)
            {
                list.Add(sElement.ToInitNotify());
                list.Add(new Notify_ElementJoinState { Index = i.Key });
            }
        }
        return list.ToArray();
    }

    #endregion

}
