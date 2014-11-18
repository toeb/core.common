using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FileFormats
{
  public class FileFormat
  {
    protected FileFormat()
    {

    }
    public IEnumerable<string> Extensions { get; set; }
    public string DisplayName { get; set; }
    public string Name { get; set; }
    public string Encoding { get; set; }
  }

  [Export]
  public class CsvFileFormat : FileFormat
  {
    public CsvFileFormat()
    {
      Extensions = new[] { "csv", "tsv" };
      DisplayName = "Comma Separated Values";
    }

  }




  public interface IFileFormatReader
  {

    object Read(Stream stream);
  }

  public interface IFileFormatWriter
  {
    void Write(Stream stream, object model);
  }
}
