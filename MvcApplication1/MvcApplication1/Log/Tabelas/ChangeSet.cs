using System;
using System.Collections.Generic;
using FrameLog.Models;


namespace MvcApplication1.Log.Tabelas
{
    public class ChangeSet : IChangeSet<User>
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public virtual User Author { get; set; }
        public virtual List<ObjectChange> ObjectChanges { get; set; }

        IEnumerable<IObjectChange<User>> IChangeSet<User>.ObjectChanges
        {
            get { return ObjectChanges; }
        }

        void IChangeSet<User>.Add(IObjectChange<User> objectChange)
        {
            ObjectChanges.Add((ObjectChange)objectChange);
        }

        public override string ToString()
        {
            return string.Format("Por {0} em {1}, com {2} ObjectChanges",
                Author, Timestamp, ObjectChanges.Count);
        }
    }
}
