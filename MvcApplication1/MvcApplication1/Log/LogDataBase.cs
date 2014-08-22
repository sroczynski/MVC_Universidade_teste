//using FrameLog;
//using FrameLog.Contexts;
//using FrameLog.History;
//using FrameLog.Models;
//using MvcApplication1.Log;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Web;

//namespace MvcApplication1.FrameLog
//{
//    public class LogDataBase : DbContext
//    {
//        public readonly FrameLogModule<ChangeSet, User> Logger;

//        public LogDataBase()
//        {
//            Logger = new FrameLogModule<ChangeSet, User>(new ChangeSetFactory(), new LogDatabaseAdapter(this));
//        }

//        public DbSet<ChangeSet> ChangeSets { get; set; }
//        public DbSet<ObjectChange> ObjectChanges { get; set; }
//        public DbSet<PropertyChange> PropertyChanges { get; set; }

//        public IFrameLogContext<ChangeSet, User> FrameLogContext
//        {
//            get { return new LogDatabaseAdapter(this); }
//        }
//        public HistoryExplorer<ChangeSet, User> HistoryExplorer
//        {
//            get { return new HistoryExplorer<ChangeSet, User>(FrameLogContext); }
//        }
        
//        public void Save(User autor)
//        {
//            try
//            {
//                this.Database.Create();
//            }
//            finally
//            {

//                Logger.SaveChanges(autor);
//            }
//        }

//        [Obsolete]
//        public override int SaveChanges()
//        {
//            return base.SaveChanges();
//        }
//    }

//    public class LogDatabaseAdapter : DbContextAdapter<ChangeSet, User>
//    {
//        private LogDataBase context;

//        public LogDatabaseAdapter(LogDataBase context)
//            : base(context)
//        {
//            this.context = context;
//        }

//        public override IQueryable<IChangeSet<User>> ChangeSets
//        {
//            get { return context.ChangeSets; }
//        }

//        public override IQueryable<IObjectChange<User>> ObjectChanges
//        {
//            get { return context.ObjectChanges; }
//        }

//        public override IQueryable<IPropertyChange<User>> PropertyChanges
//        {
//            get { return context.PropertyChanges; }
//        }

//        public override void AddChangeSet(ChangeSet changeSet)
//        {
//            context.ChangeSets.Add(changeSet);
//        }

//        public override Type UnderlyingContextType
//        {
//            get { return typeof(LogDataBase); }
//        }
//    }
//}