﻿using System;

namespace Sawczyn.EFDesigner
{
   /// <summary>
   ///    This helps keep UI interaction out of our DSL project proper. DslPackage calls RegisterDisplayHandler with a method that shows the MessageBox
   ///    (or other UI-related method) properly using the Visual Studio service provider.
   /// </summary>
   public static class BooleanQuestionDisplay
   {
      private static QuestionVisualizer QuestionVisualizerMethod;

      public delegate bool QuestionVisualizer(IServiceProvider serviceProvider, string message);

      public static void RegisterDisplayHandler(QuestionVisualizer method)
      {
         QuestionVisualizerMethod = method;
      }

      public static bool? Show(IServiceProvider serviceProvider, string message)
      {
         if (QuestionVisualizerMethod != null)
         {
            try
            {
               return QuestionVisualizerMethod(serviceProvider, message);
            }
            catch
            {
               return null;
            }
         }

         return null;
      }
   }
}