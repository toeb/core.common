using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core
{
  /// <summary>
  /// simple class for describing a file library like 
  /// jquery-1.4.min.js
  /// bootstrap.min.css
  /// libname-1.0.0.0.tag1.tag2.tag3.extension
  /// ... etc
  /// contains methods for parsing these kind of filenames
  /// </summary>
  [DebuggerDisplay("{NormalizedName}")]
  public class FileLib
  {
    /// <summary>
    /// regular expression for parsing the filename
    /// </summary>
    public static readonly string libRegex = @"((?<libname>[^\.-]*)-?)((?<major>\d+))?(\.(?<minor>\d+)(\.(?<revision>\d+)(\.(?<build>\d+))?)?)?(\.(?<tags>.+))*";
    /// <summary>
    /// parses a path as a lib
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static FileLib ParsePath(string path, bool normalize = false)
    {
      var filename = Path.GetFileName(path);
      return Parse(filename, normalize);
    }
    /// <summary>
    /// parses a filename as a file lib 
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static FileLib Parse(string filename, bool normalize = false)
    {
      filename = filename.Trim();
      var match = Regex.Match(filename, libRegex);
      match.NextMatch();
      var lib = match.Groups["libname"].Value ?? "";
      var major = match.Groups["major"].Value ?? "";
      var minor = match.Groups["minor"].Value ?? "";
      var revision = match.Groups["revision"].Value ?? "";
      var build = match.Groups["build"].Value ?? "";
      var tags = match.Groups["tags"].Value.Split('.');
      var extension = tags.LastOrDefault() ?? "";
      tags = tags.Reverse().Skip(1).Reverse().ToArray();
      var result = new FileLib()
      {
        Extension = extension,
        LibName = lib,
        Major = major,
        Minor = minor,
        Revision = revision,
        Build = build,
        Tags = tags,
        OriginalName = filename,


      };
      result.NormalizedName = NormalizeName(result);
      if (normalize) result = result.Normalize();
      return result;
    }

    private static string NormalizeName(FileLib lib)
    {
      StringBuilder result = new StringBuilder();

      if (string.IsNullOrEmpty(lib.LibName)) return result.ToString();
      result.Append(lib.LibName.ToLower());
      var version = lib.GetVersion();
      if (version != null)
      {
        result.Append('-');
        result.Append(version.ToString());
      }

      foreach (var tag in lib.Tags)
      {
        result.Append('.');
        result.Append(tag.ToLower());
      }
      if (!string.IsNullOrEmpty(lib.Extension))
      {
        result.Append('.');
        result.Append(lib.Extension.ToLower());
      }
      return result.ToString();

    }
    /// <summary>
    /// Major version number
    /// </summary>
    public string Major { get; private set; }
    /// <summary>
    /// Minor version number
    /// </summary>
    public string Minor { get; private set; }
    /// <summary>
    /// Revision Number
    /// </summary>
    public string Revision { get; private set; }

    /// <summary>
    /// Build number
    /// </summary>
    public string Build { get; private set; }

    /// <summary>
    /// Creates a version object for the Filelib. returns null if this is not possible
    /// </summary>
    /// <returns></returns>
    public Version GetVersion()
    {

      Version parsedVersion = null;
      try
      {
        var version = Major + "." + Minor + "." + Revision + "." + Build;
        while (version.EndsWith(".")) version = version.Substring(0, version.Length - 1);
        if (version == "") return parsedVersion;
        parsedVersion = new Version(version);
      }
      catch (Exception ) { }
      return parsedVersion;

    }
    /// <summary>
    /// the name of the lib
    /// </summary>
    public string LibName { get; private set; }
    /// <summary>
    /// the tags of the lib.  e.g. min, intellisense, etc
    /// </summary>
    public IEnumerable<string> Tags { get; private set; }
    /// <summary>
    /// the extension of the lib
    /// </summary>
    public string Extension { get; private set; }

    public string OriginalName { get; private set; }
    /// <summary>
    /// normalizes the name. ensures all parts are in lowercase
    /// </summary>
    public string NormalizedName { get; private set; }
    /// <summary>
    ///  returns the normalized version of this filelib object ( everything lower case)
    /// </summary>
    /// <returns></returns>
    public FileLib Normalize()
    {
      return Parse(NormalizedName);
    }
  }



}
