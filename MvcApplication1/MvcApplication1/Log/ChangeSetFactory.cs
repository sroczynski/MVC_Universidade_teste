using FrameLog.Models;
using MvcApplication1.Log.Tabelas;
using System.Collections.Generic;

namespace MvcApplication1.Log
{
    public class ChangeSetFactory : IChangeSetFactory<ChangeSet, User>
    {
        public ChangeSet ChangeSet()
        {
            var set = new ChangeSet();
            set.ObjectChanges = new List<ObjectChange>();
            return set;
        }

        public IObjectChange<User> ObjectChange()
        {
            var obj = new ObjectChange();
            obj.PropertyChanges = new List<PropertyChange>();
            return obj;
        }

        public IPropertyChange<User> PropertyChange()
        {
            return new PropertyChange();
        }
    }
}
