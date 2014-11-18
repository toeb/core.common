
using Core.TestingUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Core.Versioning.Test
{
  [TestClass]
  public class SynchronizableObjectTests : TypedTestBase<SyncObject>
  {
    protected override SyncObject CreateUut()
    {
      return new SyncObject();
    }
    [TestMethod]
    public void Create()
    {
      Assert.IsNull(uut.Value);
      Assert.IsFalse(uut.IsDirty);
      Assert.IsFalse(uut.WasComitted);
      Assert.IsFalse(uut.CanCommit);
      Assert.IsTrue(uut.IsNew);
      Assert.IsNull(uut.CleanValue);
      Assert.AreEqual(Guid.Empty, uut.CleanVersion);
      Assert.AreEqual(Guid.Empty, uut.CleanMergedVersion);
      Assert.AreEqual(Guid.Empty, uut.MergedVersion);
      Assert.AreEqual(Guid.Empty, uut.PreviousVersion);
      Assert.AreEqual(Guid.Empty, uut.Version);

    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void FailOnCommitUnchangedNewObject()
    {
      uut.Commit();
    }

    [TestMethod]
    public void ChangeNewObject()
    {
      uut.Value = "another value";
      Assert.IsTrue(uut.IsDirty);
      Assert.IsFalse(uut.WasComitted);
      Assert.IsTrue(uut.CanCommit);
      Assert.AreEqual(Guid.Empty, uut.MergedVersion);
      Assert.AreEqual(Guid.Empty, uut.PreviousVersion);
      Assert.AreEqual(Guid.Empty, uut.Version);


    }
    [TestMethod]
    public void CommitChangedNewObject()
    {
      uut.Value = "another value";
      var revision = uut.Commit();
      Assert.IsFalse(uut.CanCommit);
      Assert.IsTrue(uut.WasComitted);
      Assert.IsTrue(uut.IsClean);
      Assert.IsFalse(revision.WasComitted);
      Assert.IsFalse(revision.IsDirty);

    }
    [TestMethod]
    public void BranchNewCleanVersion()
    {
      var branch = uut.Branch();
      Assert.AreEqual(uut.Version, branch.Version);
      Assert.IsFalse(branch.IsDirty);
      Assert.IsNull(branch.Value);
      Assert.AreEqual(uut.Value, branch.Value);
      Assert.AreEqual(uut.Version, branch.PreviousVersion);
      Assert.AreEqual(Guid.Empty, branch.MergedVersion);
    }
    [TestMethod]
    public void BranchCommittedVersion()
    {
      uut.Value = 1;
      uut.Commit();
      var branch = uut.Branch();
      Assert.AreNotEqual(Guid.Empty, branch.Version);
      Assert.AreEqual(uut.Version, branch.Version);
      Assert.AreEqual(uut.Version, branch.PreviousVersion);
      Assert.AreEqual(1, branch.Value);

    }
    [TestMethod]
    public void ChangeBranchOfNewCleanVersion()
    {
      var branch = uut.Branch();
      branch.Value = "another value";
      Assert.AreEqual("another value", branch.Value);
      Assert.IsTrue(branch.IsDirty);
      Assert.AreNotEqual(uut.Value, branch.Value);
    }
    [TestMethod]
    public void ChangeBranchOfCommittedVersion()
    {
      uut.Value = 2;
      uut.Commit();
      var branch = uut.Branch();
      branch.Value = 3;
      Assert.AreNotEqual(uut.Value, branch.Value);
      Assert.IsTrue(branch.IsDirty);

    }
    [TestMethod]
    public void MergeUnchangedBranchIntoMaster()
    {
      var branch = uut.Branch();
      var result = uut.Merge(branch);
      Assert.IsTrue(result);
      var mergedBranch = (SyncObject)result;
      Assert.IsNotNull(mergedBranch);
      Assert.AreEqual(mergedBranch.Value, uut.Value);
      Assert.AreEqual(mergedBranch.Value, branch.Value);
      Assert.AreEqual(branch.Version, mergedBranch.MergedVersion);
      Assert.AreEqual(uut.Version, mergedBranch.PreviousVersion);
      Assert.AreEqual(branch.Version, mergedBranch.PreviousVersion);
    }
    [TestMethod]
    public void MergeChangedBranchIntoMaster()
    {
      uut.Value = "some value";
      uut.Commit();
      var branch = uut.Branch();
      branch.Value = "new value";
      MergeResult result = uut.Merge(branch);
      Assert.IsNotNull(result);
      Assert.IsTrue(result);
      Assert.AreEqual(uut, result.Previous);
      Assert.AreEqual(branch, result.Branch);
      var mergedBranch = (SyncObject)result;
      Assert.IsNotNull(mergedBranch);
      Assert.IsFalse(object.ReferenceEquals(branch.Value, mergedBranch.Value));
      Assert.AreNotEqual(uut.Value, mergedBranch.Value);
      Assert.AreEqual("new value", mergedBranch.Value);
    }
    [TestMethod]
    public void MergeBranchConflict()
    {
      uut.Value = "hello";
      uut = uut.Commit();
      var branch = uut.Branch();
      uut.Value = "hello 1";
      uut.Commit();
      branch.Value = "hello 2";
      branch.Commit();
      var mergeResult = uut.Merge(branch);
      Assert.IsFalse(mergeResult);
      Assert.AreEqual(MergeState.Conflict, mergeResult.State);
      Assert.IsNull(mergeResult.Result);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ExceptionOnInvalidResolve()
    {
      uut.Value = 1;
      uut.Commit();
      var branch = uut.Branch();
      branch.Value = 2;
      branch.Commit();
      var mergeResult = uut.Merge(branch);
      mergeResult.Resolve(3);
    }
    [TestMethod]
    public void ResolveConflict()
    {
      uut.Value = "hello";
      uut = uut.Commit();
      var branch = uut.Branch();
      uut.Value = "hello 1";
      uut.Commit();
      branch.Value = "hello 2";
      branch.Commit();
      var mergeResult = uut.Merge(branch);
      mergeResult.Resolve("hello 3");
      Assert.IsTrue(mergeResult);
      Assert.IsNotNull(mergeResult);
      var mergedBranch = mergeResult.Result;
      Assert.IsTrue(mergedBranch.IsDirty);
      Assert.AreEqual("hello 3", mergedBranch.Value);
      Assert.AreEqual("hello 3", mergedBranch.CleanValue);
      Assert.AreEqual(uut.Version, mergedBranch.PreviousVersion);
      Assert.AreEqual(branch.Version, mergedBranch.MergedVersion);
    }

  }
}
