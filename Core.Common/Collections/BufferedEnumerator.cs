using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Collections
{
  public interface IBufferedEnumerator<T> : IEnumerator<T>
  {
    IEnumerator<T> GetHeadEnumerator();
    int HeadPosition { get; }
    T Head { get; }
    bool MoveNextHead();
    bool MoveBack();
    T Lookup(int position, SeekOrigin origin = SeekOrigin.Current);
    int Position { get; }
    int PositionBehindHead { get; }
    T Seek(int position, SeekOrigin origin = SeekOrigin.Current);
  }
  public class BufferedEnumerator<T> : IBufferedEnumerator<T>
  {
    T[] buffer;


    int position;
    int head;

    public BufferedEnumerator(IEnumerator<T> inner, bool includeCurrentElement, int bufferSize):this(inner,bufferSize)
    {
      if (includeCurrentElement) {
        head = 0;
        position = 0;
        buffer[0] = inner.Current;
      }
      

    }
    public BufferedEnumerator(IEnumerator<T> inner, int bufferSize)
    {
      this.Inner = inner;
      buffer = new T[bufferSize];
      head = -1;
      position = -1;


    }

    public T Lookup(int pos, SeekOrigin origin = SeekOrigin.Current)
    {
      pos = GetValidAbsolutePosition(pos, origin);
      if (pos < 0) throw new InvalidOperationException("cannot go to position, end of enumerator, bellow 0 or out of buffer range");

      return GetBufferedElement(pos);
    }
    public int GetValidAbsolutePosition(int pos, SeekOrigin origin = SeekOrigin.Current)
    {
      if (origin == SeekOrigin.End)
      {
        return GetValidAbsolutePosition(head + pos, SeekOrigin.Begin);
      }
      if (origin == SeekOrigin.Current)
      {
        return GetValidAbsolutePosition(position + pos, SeekOrigin.Begin);
      }
      if (!IsValidPosition(pos)) return -1;

      while (pos > head)
      {
        if (!MoveNextHead()) return -1;
      }
      return pos;

    }
    public bool CanLookup(int pos, SeekOrigin origin = SeekOrigin.Current)
    {
      return GetValidAbsolutePosition(pos, origin) > -1;
    }
    public T this[int relativePosition]
    {
      get
      {
        return Lookup(relativePosition, SeekOrigin.Current);
      }
    }
    public T Seek(int pos, SeekOrigin origin = SeekOrigin.Current)
    {
      if (origin == SeekOrigin.End)
      {
        return Seek(head + pos, SeekOrigin.Begin);
      }
      if (origin == SeekOrigin.Current)
      {
        return Seek(position + pos, SeekOrigin.Begin);
      }

      if (pos < 0) throw new InvalidOperationException("cannot seek further back than absolute position 0");
      if (!IsValidPosition(pos)) throw new InvalidOperationException("cannot seek further back than buffer length " + buffer.Length);
      var result = Lookup(pos, SeekOrigin.Begin);
      position = pos;
      return result;

    }

    public IEnumerable<T> Buffer
    {
      get
      {
        yield break;
      }
    }


    private T GetBufferedElement(int pos)
    {
      if (!IsValidPosition(pos)) throw new InvalidOperationException("position is may not be behind head - buffer.Length");
      return buffer[GetBufferPosition(pos)];
    }


    public IEnumerator<T> Inner { get; set; }

    public T Current
    {
      get
      {
        if (position < 0) throw new InvalidOperationException("iterator was not initialized");
        return GetBufferedElement(position);
      }
    }

    public void Dispose()
    {
      Inner.Dispose();
    }

    object System.Collections.IEnumerator.Current
    {
      get { return Current; }
    }

    public bool MoveNext()
    {
      if (position < head)
      {
        position++;
        return true;
      }
      else if (position == head)
      {
        if (!MoveNextHead())
        {
          return false;
        }
        position++;
        return true;
      }
      else
      {
        throw new Exception();
      }

    }

    public void Reset()
    {
      position = -1;
      head = -1;
      Inner.Reset();
    }

    public int Position
    {
      get { return position; }
    }

    public int PositionBehindHead
    {
      get { return head - position; }
    }

    public int HeadPosition
    {
      get { return head; }
    }




    public T Head
    {
      get { return GetBufferedElement(head); }
    }

    public bool MoveNextHead()
    {
      if (!Inner.MoveNext()) return false;
      buffer[GetBufferPosition(++head)] = Inner.Current;
      return true;
    }

    public int GetBufferPosition(int pos)
    {
      return pos % buffer.Length;
    }

    bool IsValidPosition(int pos)
    {
      if (pos < 0) return false;
      return head - pos < buffer.Length;
    }

    public bool MoveBack()
    {
      if (!IsValidPosition(position - 1)) return false;
      Seek(-1, SeekOrigin.Current);
      return true;
    }



    public IEnumerator<T> GetHeadEnumerator()
    {
      while (MoveNextHead()) yield return Head;
    }
  }
}
