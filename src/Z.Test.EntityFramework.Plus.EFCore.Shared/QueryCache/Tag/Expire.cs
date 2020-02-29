﻿// Description: Entity Framework Bulk Operations & Utilities (EF Bulk SaveChanges, Insert, Update, Delete, Merge | LINQ Query Cache, Deferred, Filter, IncludeFilter, IncludeOptimize | Audit)
// Website & Documentation: https://github.com/zzzprojects/Entity-Framework-Plus
// Forum & Issues: https://github.com/zzzprojects/EntityFramework-Plus/issues
// License: https://github.com/zzzprojects/EntityFramework-Plus/blob/master/LICENSE
// More projects: http://www.zzzprojects.com/
// Copyright © ZZZ Projects Inc. 2014 - 2016. All rights reserved.

using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.EntityFramework.Plus;

namespace Z.Test.EntityFramework.Plus
{
    public partial class QueryCache_Tag
    {
        [TestMethod]
        public void Tag_Expire()
        {
            var testCacheKey = Guid.NewGuid().ToString();

            TestContext.DeleteAll(x => x.Entity_Basics);
            TestContext.Insert(x => x.Entity_Basics, 1);

            using (var ctx = new TestContext())
            {
                // BEFORE
                var itemCountBefore = ctx.Entity_Basics.FromCache(testCacheKey).Count();
                var cacheCountBefore = QueryCacheHelper.GetCacheCount();

                TestContext.DeleteAll(x => x.Entity_Basics);

                QueryCacheManager.ExpireTag(testCacheKey);
                var cacheCountExpired = QueryCacheHelper.GetCacheCount();

                // TEST: The cache count are NOT equal (The cache key has been removed)
                Assert.AreEqual(cacheCountBefore - 1, cacheCountExpired);

                // AFTER
                var itemCountAfter = ctx.Entity_Basics.FromCache(testCacheKey).Count();
                var cacheCountAfter = QueryCacheHelper.GetCacheCount();

                // TEST: The item count are NOT equal (The query has been expired)
                Assert.AreNotEqual(itemCountBefore, itemCountAfter);
                Assert.AreEqual(0, itemCountAfter);

                // TEST: The cache count are NOT equal (The expired cache key is added)
                Assert.AreEqual(cacheCountExpired + 1, cacheCountAfter);
                Assert.AreEqual(cacheCountBefore, cacheCountAfter);
            }
        }
    }
}