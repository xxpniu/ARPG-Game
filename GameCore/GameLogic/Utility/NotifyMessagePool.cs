using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Proto;
#pragma warning disable XS0001 // Find usages of mono todo items
namespace GameLogic.Utility
{
    
    public class NotifyMessagePool
    {
        public class Frame
        {
            public int Index;
            public float time;

            private List<ISerializerable> notifys = new List<ISerializerable>();

            public void SetNotify(ISerializerable[] notifys)
            {
                this.notifys = notifys.ToList();
            }

            public void LoadFormBytes(BinaryReader br)
            {
                notifys.Clear();
                time = br.ReadSingle();
                var count = br.ReadInt32();
                while (count-- > 0)
                {
                    var typeIndex = br.ReadInt32();
                    var type = Proto.MessageHandleTypes.GetTypeByIndex(typeIndex);
                    var t = Activator.CreateInstance(type) as Proto.ISerializerable;
                    t.ParseFormBinary(br);
                    notifys.Add(t);
                }
            }

            public void ToBytes(BinaryWriter bw)
            {
                bw.Write(time);
                bw.Write(notifys.Count);
                foreach (var i in notifys)
                {
                    int index = 0;
                    if (MessageHandleTypes.GetTypeIndex(i.GetType(), out index))
                    {
                        bw.Write(index);
                        i.ToBinary(bw);
                    }
                }

            }

            public Proto.ISerializerable[] GetNotify()
            {
                return notifys.ToArray();
            }

       }

        public NotifyMessagePool()
        {
        }

        private int frame;

        public int TotalFrame { get { return frames.Count; }}

        public Queue<Frame> frames = new Queue<Frame>();

        public void AddFrame(Proto.ISerializerable[] notify,float time)
        {
            var f = new Frame { Index = this.frame++,time =time};
            f.SetNotify(notify);
            frames.Enqueue(f);
        }

        public void LoadFormBytes(byte[] bytes)
        {
            frame = 0;
            frames = new Queue<Frame>();
            using (var mem = new MemoryStream(bytes))
            {
                using (var br = new BinaryReader(mem))
                {
                    var count = br.ReadInt32();
                    while (count-- > 0)
                    {
                        var f = new Frame() { Index = frame++ };
                        f.LoadFormBytes(br);
                        frames.Enqueue(f);
                    }
                }
            }
        }

        public byte[] ToBytes()
        {
            using (var mem = new MemoryStream())
            {
                using (var bw = new BinaryWriter(mem))
                {
                    bw.Write(frames.Count);
                    foreach (var i in frames)
                    {
                        i.ToBytes(bw);
                    }
                }
                return mem.ToArray();
            }
        }

        public bool NextFrame(out Frame frame)
        {
            frame = null;
            if (frames.Count > 0)
            {
                frame = frames.Dequeue();
                return true;
            }
            return false;
        }
    }
}

