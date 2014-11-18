using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
  /// <summary>
  ///   A Simple Class containing a query string and an enumerable containing the reuslts.
  /// </summary>
  ///
  /// <remarks> Tobi, 30.03.2012. </remarks>
  ///
  /// ### <typeparam name="T">  Generic type parameter. </typeparam>
  public class QueryResult : NotifyPropertyChangedBase
  {
    /// <summary> Date/Time of the creation </summary>
    private DateTime _created;
    /// <summary> Date/Time of the completion </summary>
    private DateTime _completed;
    /// <summary> Date/Time of start </summary>
    private DateTime _started;
    /// <summary> true indicates query complete </summary>
    private bool _queryComplete;
    /// <summary> true indicates query started </summary>
    private bool _queryStarted;
    /// <summary> The query string </summary>
    private string _queryString;
    /// <summary> The results </summary>
    private ICollection<object> _results;

    /// <summary> Adds a result.  </summary>
    ///
    /// <remarks> Tobi, 30.03.2012. </remarks>
    ///
    /// <param name="result"> The result. </param>
    public void AddResult(object result)
    {
      _results.Add(result);
    }

    /// <summary> Gets or sets the Date/Time of the created. </summary>
    ///
    /// <value> The created. </value>
    public DateTime Created
    {
      get
      {
        return _created;
      }
      private set
      {
        
        ChangeIfDifferent(ref _created, value);
      }
    }

    /// <summary> Gets or sets the Date/Time of the completed. </summary>
    ///
    /// <value> The completed. </value>
    public DateTime Completed
    {
      get
      {
        return _completed;
      }
      private set
      {
        ChangeIfDifferent(ref _completed, value);
      }
    }

    /// <summary> Gets or sets the Date/Time of the start. </summary>
    ///
    /// <value> The start. </value>
    public DateTime Start
    {
      get
      {
        return _started;
      }
      private set
      {
        ChangeIfDifferent(ref _started, value);
      }
    }

    /// <summary> Constructor. </summary>
    ///
    /// <remarks> Tobi, 30.03.2012. </remarks>
    ///
    /// <param name="queryString">  The query string. </param>
    /// <param name="results">      The results. </param>
    public QueryResult(string queryString)
    {
      Created = DateTime.Now;
      _results = CollectionFactory.CreateCollection<object>(true);
    }

    /// <summary> Gets the results. </summary>
    ///
    /// <value> The results. </value>
    public IEnumerable<object> Results { get { return _results; } }

    /// <summary> Gets the query. </summary>
    ///
    /// <value> The query. </value>
    public string Query { get { return _queryString; } }

    /// <summary> Gets or sets a value indicating whether the query complete. </summary>
    ///
    /// <value> true if query complete, false if not. </value>
    public bool QueryComplete { get { return _queryComplete; } private set { _queryComplete = value; RaisePropertyChanged(); } }

    /// <summary> Gets or sets a value indicating whether the query started. </summary>
    ///
    /// <value> true if query started, false if not. </value>
    public bool QueryStarted { get { return _queryStarted; } private set { ChangeIfDifferent(ref _queryStarted, value); } }

    public void SetStarted()
    {
      QueryStarted = true;
      Start = DateTime.Now;
    }

    /// <summary> Sets this query to complete. </summary>
    ///
    /// <remarks> Tobi, 30.03.2012. </remarks>
    public void SetComplete()
    {
      QueryComplete = true;
      Completed = DateTime.Now;
    }
  }
}
