

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Core.Versioning
{
  public class MergeResult
  {
    private SyncObject previous;
    private SyncObject branch;

    private SyncObject result;

    public MergeResult(SyncObject previous, SyncObject branch)
    {
      this.previous = previous;
      this.branch = branch;
      this.State = Resolve();
    }
    public void Resolve(object resolvedValue)
    {
      if (State != MergeState.Conflict) throw new InvalidOperationException("Cannot resolve an unconflicted merge");
      result = new SyncObject(previous, branch, resolvedValue);
      State = MergeState.Success;
    }
    private MergeState Resolve()
    {
      if (branch.Version == previous.Version)
      {
        // object is a branch of previous version but was not changed
        // assert previous.Value equals branch.Value
        result = new SyncObject(previous.Value, previous.Version, branch.Version);
        return MergeState.Success;
      }
      if (branch.PreviousVersion == previous.Version)
      {
        // object was branched and changed but orignal was not changed
        result = new SyncObject(branch.Value, previous.Version, branch.Version);
        return MergeState.Success;
      }

      return MergeState.Conflict;
    }
    public MergeState State { get; private set; }

    public static implicit operator bool(MergeResult result)
    {
      return result.State == MergeState.Success;
    }
    public static implicit operator SyncObject(MergeResult result)
    {
      return result.Result;
    }
    public SyncObject Result { get { return result; } }
    public SyncObject Previous { get { return previous; } }
    public SyncObject Branch { get { return branch; } }
  }
}
