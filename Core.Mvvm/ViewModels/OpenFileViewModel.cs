using Core.Common.MVVM;
using System.ComponentModel.Composition;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Core.Common.Injection;
using System;

namespace Core.Common.MVVM
{

    public class FileType
    {

        public FileType(string ext, string name) { Extension = ext.ToLower(); if (Extension.StartsWith(".")) Extension = Extension.Substring(1); Name = name; }


        public string Extension { get; set; }
        public string Name { get; set; }
    }
    public static class FileTypes
    {

        public static readonly FileType Stl = new FileType("stl", "Stereo Lithography");
        public static readonly FileType Bmp = new FileType("bmp", "Bitmap Image");
        public static readonly FileType Png = new FileType("png", "Portable Network Graphics");
        public static readonly FileType Jpg = new FileType("jpg", "JPEG encoded Image");

        public static readonly FileType[] Cad = new FileType[] { Stl };
        public static readonly FileType[] Images = new FileType[] { Bmp, Png, Jpg };

    }

    public class OpenFileViewModel : NotifyPropertyChangedBase
    {
        [ImportingConstructor]
        private OpenFileViewModel([Import] IInjectionService ijs)
        {
            DisplayName = "Select File...";

        }
        public static implicit operator bool(OpenFileViewModel self)
        {
            if (self == null)
            {
                return false;
            }
            return self.Success.HasValue && self.Success.Value;
        }
        public bool? Success { get; set; }

        public string FileName
        {
            get
            {
                return FileNames.Single();
            }
        }


        private string[] _fileNames;
        public string[] FileNames { get => _fileNames; set => this.ChangeProperty(ref _fileNames, value); }

        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set => this.ChangeProperty(ref _displayName, value);
        }



        public void SelectFile(string fileName)
        {
            Path.GetFileName(fileName);
            DisplayName = fileName;
            FileNames = new string[] { fileName };
            Success = true;
        }

        private bool _multiSelect;
        public bool Multiselect
        {
            get => _multiSelect;
            set => this.ChangeProperty(ref _multiSelect, value);
        }
        private bool _openDirectory;
        public bool OpenDirectory
        {
            get => _openDirectory;
            set => this.Change(ref _openDirectory, value);
        }



        public ICollection<FileType> Filter { get; } = new List<FileType>();
    }


}
