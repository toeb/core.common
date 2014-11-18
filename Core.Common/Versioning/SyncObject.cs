using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Core.Versioning
{
  [DebuggerDisplay("Rev {value} {original} IsDirty {IsDirty} WasComitted {WasComitted} , new {IsNew}")]
  public class SyncObject 
  {
    //
    private object value;
    private object original;
    //48 byte
    public Guid Version { get; private set; }
    public Guid PreviousVersion { get; private set; }
    public Guid MergedVersion { get; private set; }

    //constructs a new sync object
    public SyncObject()
    {
      this.original = null;
      this.value = null;
    }

    /// <summary>
    /// returns true iff this is new and unchanged
    /// </summary>
    public bool IsNew { get { return Version.IsEmpty() && PreviousVersion.IsEmpty(); } }

    /// <summary>
    /// returns true if clean  meaning it is either unchanged or comitted
    /// </summary>
    public bool IsClean { get { if (IsNew && value != null)return false; if (PreviousVersion.IsEmpty() && !Version.IsEmpty())return true; return Version == PreviousVersion; } }

    /// <summary>
    /// alias for !IsClean
    /// </summary>
    public bool IsDirty { get { return !IsClean; } }

    /// <summary>
    /// only possible if object has not been comitted yet
    /// </summary>
    public void Revert()
    {
      CommitCheck();
      if (IsClean) return;
      value = original.DeepClone();
      Version = PreviousVersion;
    }

    private void CommitCheck()
    {
      if (WasComitted) throw new InvalidOperationException("cannot change value after it was already committed. try using the SyncObject returned by Commit");
    }
    /// <summary>
    ///  returns a deep clone of the clean value
    ///  the clean value is the original value (the value obtained when Revert is called) if the object is not yet committed
    ///  otherwise the clean value is the original value
    ///
    /// </summary>
    public object CleanValue
    {
      get
      {
        if (WasComitted) return value.DeepClone();
        return original.DeepClone();
      }
    }
    /// <summary>
    /// returns either the comitted version or if the object was not committed yet
    /// </summary>
    public Guid CleanVersion
    {
      get
      {
        if (WasComitted) return Version;
        return PreviousVersion;
      }
    }

    public Guid CleanMergedVersion
    {
      get
      {
        if (WasComitted) return Guid.Empty;
        return MergedVersion;
      }
    }
    /// <summary>
    /// returns the value
    /// if the value is a reference type and the object was already committed a deep copy is returned
    /// 
    /// </summary>
    public object Value
    {
      get
      {
        // return a copy of the value if syncobject was already comitted
        if (WasComitted) return value.DeepClone();
        return value;
      }
      set
      {
        CommitCheck();
        this.value = value;
        NotifyValueChanged();
      }
    }
    /// <summary>
    /// call if object has reference has changed causing the syncobjects Dirty logic to kick in
    /// </summary>
    public void NotifyValueChanged()
    {
      if (IsDirty) return;
      Version = Guid.Empty;
    }
    /// <summary>
    /// returns true if commit was called successfully
    /// </summary>
    public bool WasComitted
    {
      get
      {

        return !Version.IsEmpty() && Version != PreviousVersion;
      }
    }
    /// <summary>
    /// commit  the changes. closing off this syncobject to further modification
    /// </summary>
    /// <returns></returns>
    public SyncObject Commit()
    {
      if (!IsDirty) throw new InvalidOperationException("No Changes were made. Cannot commit");
      Version = Guid.NewGuid();
      // deepclone value in case it is still referenced outside of this object
      value = value.DeepClone();
      return Branch();
    }

    /// <summary>
    /// depending on wether previous version is committed or not
    /// not committed: create a branch from previousVersion's previousVersion but keep changes made (resulting in a new dirty version)
    /// comitted: create a branch based on previous versions value. the object will be clean
    /// </summary>
    /// <param name="previousVersion"></param>
    internal SyncObject(SyncObject previousVersion)
    {
      if (!previousVersion.WasComitted)
      {
        // just branch the previous version of the previous version
        Value = previousVersion.Value.DeepClone();
        original = previousVersion.CleanValue;
        PreviousVersion = previousVersion.CleanVersion;
        MergedVersion = previousVersion.MergedVersion;
        Version = Guid.Empty;
      }
      else
      {
        PreviousVersion = previousVersion.Version;
        MergedVersion = Guid.Empty;
        value = previousVersion.value.DeepClone();
        original = previousVersion.value.DeepClone();
        Version = PreviousVersion;
      }

    }
    internal SyncObject(SyncObject previous, SyncObject mergedBranch, object mergedValue)
    {
      if (!previous.WasComitted) throw new InvalidOperationException("cannot create a merged sync object if previous version was not comitted");
      if (!previous.WasComitted) throw new InvalidOperationException("cannot create a merged sync object if branch to be mergedwas not comitted");

      this.Value = mergedValue.DeepClone();
      this.original = mergedValue.DeepClone();
      MergedVersion = mergedBranch.Version;
      PreviousVersion = previous.Version;
      Version = Guid.Empty;
    }


    internal SyncObject(object value, Guid originalVersion, Guid mergedVersion)
    {
      PreviousVersion = originalVersion;
      MergedVersion = mergedVersion;
      this.value = value.DeepClone();
    }
    /// <summary>
    /// creates a branch of this syncobject
    /// </summary>
    /// <returns></returns>
    public SyncObject Branch()
    {
      return new SyncObject(this);
    }
    /// <summary>
    /// tries to merge this the specified branch into this object
    /// </summary>
    /// <param name="branch"></param>
    /// <returns></returns>
    public MergeResult Merge(SyncObject branch)
    {
      MergeResult result = new MergeResult(this, branch);
      return result;
    }
    /// <summary>
    /// returns true if object can commit (isdirty && wascomitted)
    /// </summary>
    public bool CanCommit { get { return IsDirty && !WasComitted; } }
  }
}
