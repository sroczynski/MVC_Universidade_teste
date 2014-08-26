using FrameLog.Contexts;
using MvcApplication1.Log;
using MvcApplication1.Log.Tabelas;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MvcApplication1.Tabelas;
using FrameLog.Models;
using FrameLog;
using System.Data.Entity.ModelConfiguration.Conventions;
using FrameLog.History;
using FrameLog.Filter;

namespace MvcApplication1
{
    public class DBEntities : DbContext
    {
        public DBEntities()
        {
            Logger = new FrameLogModule<ChangeSet, User>(new ChangeSetFactory(), FrameLogContext);
        }

        public DbSet<Alunos> Alunos { get; set; }
        public DbSet<Cursos> Cursos { get; set; }
        public DbSet<Disciplinas> Disciplinas { get; set; }
        public DbSet<Notas> Notas { get; set; }
        public DbSet<Universidades> Universidades { get; set; }

        public DbSet<User> User { get; set; }

        //Log
        #region logging
        public DbSet<ChangeSet> ChangeSets { get; set; }
        public DbSet<ObjectChange> ObjectChanges { get; set; }
        public DbSet<PropertyChange> PropertyChanges { get; set; }

        public readonly FrameLogModule<ChangeSet, User> Logger;

        public IFrameLogContext<ChangeSet, User> FrameLogContext
        {
            get { return new DBEntitiesAdapter(this); }
        }

        public HistoryExplorer<ChangeSet, User> HistoryExplorer
        {
            get { return new HistoryExplorer<ChangeSet, User>(FrameLogContext); }
        }

        public void Save(User author)
        {
            Logger.SaveChanges(author);
        }
        #endregion

        [Obsolete]
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        } 

    }

    public class DBEntitiesAdapter : DbContextAdapter<ChangeSet, User>
    {
        private DBEntities context;

        public DBEntitiesAdapter(DBEntities context)
            : base(context)
        {
            this.context = context;
        }

        public override IQueryable<IChangeSet<User>> ChangeSets
        {
            get { return context.ChangeSets; }
        }

        public override IQueryable<IObjectChange<User>> ObjectChanges
        {
            get { return context.ObjectChanges; }
        }

        public override IQueryable<IPropertyChange<User>> PropertyChanges
        {
            get { return context.PropertyChanges; }
        }

        public override void AddChangeSet(ChangeSet changeSet)
        {
            context.ChangeSets.Add(changeSet);
        }

        public override Type UnderlyingContextType
        {
            get { return typeof(DBEntities); }
        }
    }
}