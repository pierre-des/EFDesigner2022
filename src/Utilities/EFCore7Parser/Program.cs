﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

using Microsoft.Extensions.DependencyModel;

using ParsingModels;

namespace EFCore7Parser
{
   internal class Program
   {
      public const int SUCCESS = 0;
      public const int BAD_ARGUMENT_COUNT = 1;
      public const int CANNOT_LOAD_ASSEMBLY = 2;
      public const int CANNOT_WRITE_OUTPUTFILE = 3;
      public const int CANNOT_CREATE_DBCONTEXT = 4;
      public const int CANNOT_FIND_APPROPRIATE_CONSTRUCTOR = 5;
      public const int AMBIGUOUS_REQUEST = 6;

      private static Logger log = Logger.GetLogger("");

      private static List<string> Usage
      {
         get
         {
            return new List<string>(new[]
                                    {
                                       $"Usage: {typeof(Program).Assembly.GetName().Name} InputFileName OutputFileName [FullyQualifiedClassName]",
                                       "where",
                                       "   (required) InputFileName           - path of assembly containing EFCore7 DbContext to parse",
                                       "   (required) OutputFileName          - path to create JSON file of results",
                                       "   (optional) FullyQualifiedClassName - fully-qualified name of DbContext class to process, if more than one available.",
                                       "                                        DbContext class must have a constructor that accepts one parameter of type DbContextOptions<>",
                                       "Result codes:",
                                       "   0   Success",
                                       "   1   Bad argument count",
                                       "   2   Cannot load assembly",
                                       "   3   Cannot write output file",
                                       "   4   Cannot create DbContext",
                                       "   5   Cannot find appropriate constructor",
                                       "   6   Ambiguous request",
                                       ""
                                    });
         }
      }

      private static Assembly Context_Resolving(AssemblyLoadContext context, AssemblyName assemblyName)
      {
         // avoid loading *.resources dlls, because of: https://github.com/dotnet/coreclr/issues/8416
         if (assemblyName.Name.EndsWith("resources"))
            return null;

         // check cached assemblies first
         RuntimeLibrary library = DependencyContext.Default.RuntimeLibraries.FirstOrDefault(runtimeLibrary => runtimeLibrary.Name == assemblyName.Name);

         if (library != null)
            return context.LoadFromAssemblyName(new AssemblyName(library.Name));

         // try known directories

         string pathInAppDirectory = Path.Combine(AppContext.BaseDirectory, $"{assemblyName.Name}.dll");

         if (File.Exists(pathInAppDirectory))
            return context.LoadFromAssemblyPath(pathInAppDirectory);

         // try the current directory
         string pathInCurrentDirectory = Path.Combine(Directory.GetCurrentDirectory(), $"{assemblyName.Name}.dll");

         if (File.Exists(pathInCurrentDirectory))
            return context.LoadFromAssemblyPath(pathInCurrentDirectory);

         // try gac
         string found = Directory.GetFileSystemEntries(Environment.ExpandEnvironmentVariables("%windir%\\Microsoft.NET\\assembly"),
                                                       $"{assemblyName.Name}.dll",
                                                       SearchOption.AllDirectories)
                                 .FirstOrDefault();

         return found == null
                   ? null
                   : context.LoadFromAssemblyPath(found);
      }

      private static void Exit(int returnCode, Exception ex = null)
      {
         if (returnCode != 0)
         {
            Usage.ForEach(x => log.Error(x));

            if (ex != null)
               log.Fatal($"Caught {ex.GetType().Name} - {ex.Message}");

            log.Fatal($"Exiting with return code {returnCode}");
         }

         log.Dispose();
         Environment.Exit(returnCode);
      }

      private static int Main(string[] args)
      {
         if ((args.Length < 2) || (args.Length > 3))
         {
            Usage.ForEach(x => Console.Error.WriteLine(x));
            log.Error($"Expecting 2 or 3 arguments - found {args.Length}");
            Exit(BAD_ARGUMENT_COUNT);
         }

         try
         {
            string exePath = Environment.GetCommandLineArgs()[0];
            string inputPath = args[0].Replace("\n", @"\n");
            string outputPath = args[1].Replace("\n", @"\n");
            string logPath = Path.ChangeExtension(outputPath, "log");

            log = Logger.GetLogger(logPath);

            log.Info($"Starting {exePath}");
            log.Info($"Log file at {logPath}");

            string contextClassName = args.Length == 3
                                         ? args[2]
                                         : null;

            using (StreamWriter output = new StreamWriter(outputPath))
            {
               try
               {
                  if (!File.Exists(inputPath))
                     throw new FileNotFoundException($"Can't find {inputPath}", inputPath);

                  log.Info($"Loading {inputPath}");
                  Environment.CurrentDirectory = Path.GetDirectoryName(inputPath);
                  Assembly assembly = TryLoadFrom(inputPath);
                  Parser parser = null;

                  try
                  {
                     parser = new Parser(assembly, log, contextClassName);
                  }

                  // ReSharper disable once UncatchableException
                  catch (MissingMethodException ex)
                  {
                     log.Error(ex.Message);
                     Exit(CANNOT_FIND_APPROPRIATE_CONSTRUCTOR, ex);
                  }
                  catch (AmbiguousMatchException ex)
                  {
                     log.Error(ex.Message);
                     Exit(AMBIGUOUS_REQUEST, ex);
                  }
                  catch (Exception ex)
                  {
                     Exception e = ex;

                     do
                     {
                        Debug.WriteLine(e.Message);
                        log.Error(e.Message);
                        e = e.InnerException;
                     } while (e != null);

                     Exit(CANNOT_CREATE_DBCONTEXT, ex);
                  }

                  output.Write(parser?.Process());
                  output.Flush();
                  output.Close();
               }
               catch (Exception ex)
               {
                  log.Error(ex.Message);
                  Exit(CANNOT_LOAD_ASSEMBLY, ex);
               }
            }

            log.Info("Success");
         }
         catch (Exception ex)
         {
            log.Error(ex.Message);
            Exit(CANNOT_WRITE_OUTPUTFILE, ex);
         }
         finally
         {
            log.Dispose();
         }

         return SUCCESS;
      }

      private static Assembly TryLoadFrom(string inputPath)
      {
         AssemblyLoadContext context = new AssemblyLoadContext("EFCore7Parser");
         context.Resolving += Context_Resolving;

         try
         {
            return context.LoadFromAssemblyPath(inputPath);
         }
         catch
         {
            string altPath = Path.ChangeExtension(inputPath, "dll");

            return context.LoadFromAssemblyPath(altPath);
         }
      }
   }
}