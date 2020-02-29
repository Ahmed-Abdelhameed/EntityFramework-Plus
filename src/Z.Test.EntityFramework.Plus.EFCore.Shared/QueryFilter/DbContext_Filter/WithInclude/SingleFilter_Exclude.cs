﻿// Description: Entity Framework Bulk Operations & Utilities (EF Bulk SaveChanges, Insert, Update, Delete, Merge | LINQ Query Cache, Deferred, Filter, IncludeFilter, IncludeOptimize | Audit)
// Website & Documentation: https://github.com/zzzprojects/Entity-Framework-Plus
// Forum & Issues: https://github.com/zzzprojects/EntityFramework-Plus/issues
// License: https://github.com/zzzprojects/EntityFramework-Plus/blob/master/LICENSE
// More projects: http://www.zzzprojects.com/
// Copyright © ZZZ Projects Inc. 2014 - 2016. All rights reserved.
#if !EFCORE
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Z.Test.EntityFramework.Plus
{
    public partial class QueryFilter_DbContext_Filter
    {
        [TestMethod]
        public void WithInclude_SingleFilter_Exclude()
        {
            TestContext.DeleteAll(x => x.Inheritance_Interface_Entities);
            TestContext.DeleteAll(x => x.Inheritance_Interface_Entities_LazyLoading);

            using (var ctx = new TestContext())
            {
                var list = TestContext.Insert(ctx, x => x.Inheritance_Interface_Entities, 10);

                var left = ctx.Inheritance_Interface_Entities_LazyLoading.Add(new Inheritance_Interface_Entity_LazyLoading());
                left.Rights = new Collection<Inheritance_Interface_Entity>();

                list.ForEach(x => left.Rights.Add(x));
                ctx.SaveChanges();
            }

            using (var ctx = new TestContext(false, enableFilter1: true, excludeClass: true))
            {
                var rights = ctx.Inheritance_Interface_Entities_LazyLoading.Include(x => x.Rights).ToList().SelectMany(x => x.Rights);
                Assert.AreEqual(45, rights.Sum(x => x.ColumnInt));
            }
        }
    }
}
#endif