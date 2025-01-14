﻿using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

using Microsoft.VisualStudio.Shell;

namespace Sawczyn.EFDesigner.EFModel.DslPackage
{
   public class OptionsPage : DialogPage
   {
      private string dotExePath;
      private bool saveDiagramsCompressed;
      private bool restrictPropertyTypes = true;

      [Category("Designer")]
      [DisplayName("Restrict property types")]
      [Description("If true, restrict property types to built-in types or types defined in the model. If false, any type can be used but there will be no validation that it will work.")]
      public bool RestrictPropertyTypes
      {
         get
         {
            return restrictPropertyTypes;
         }
         set
         {
            OptionsEventArgs args = new OptionsEventArgs("RestrictPorpertyTypes", restrictPropertyTypes, value);
            restrictPropertyTypes = value;
            OnOptionsChanged(args);
         }
      }

      [Category("Display")]
      [DisplayName("GraphViz dot.exe path")]
      [Description("Path to the GraphViz dot.exe file (including 'dot.exe'), if present")]
      [Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
      public string DotExePath
      {
         get
         {
            return dotExePath;
         }
         set
         {
            OptionsEventArgs args = new OptionsEventArgs("DotExePath", dotExePath, value);
            dotExePath = value;
            OnOptionsChanged(args);
         }
      }

      [Category("File")]
      [DisplayName("Save diagram using legacy binary format")]
      [Description("If true, .diagramx files will be saved in compressed format. If false, they will not.")]
      public bool SaveDiagramsCompressed
      {
         get
         {
            return saveDiagramsCompressed;
         }
         set
         {
            OptionsEventArgs args = new OptionsEventArgs("SaveDiagramsCompressed", saveDiagramsCompressed, value);
            saveDiagramsCompressed = value;
            OnOptionsChanged(args);
         }
      }

      protected virtual void OnOptionsChanged(OptionsEventArgs args)
      {
         OptionChanged?.Invoke(this, args);
      }

      public event EventHandler<OptionsEventArgs> OptionChanged;
   }

   public class OptionsEventArgs : EventArgs
   {
      public OptionsEventArgs(string option, object oldValue, object newValue)
      {
         Option = option;
         OldValue = oldValue;
         NewValue = newValue;
      }

      public string Option { get; }
      public object OldValue { get; }
      public object NewValue { get; }
   }
}