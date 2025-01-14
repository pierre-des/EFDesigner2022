using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security;

// ReSharper disable RedundantNameQualifier

namespace Sawczyn.EFDesigner.EFModel.EditingOnly
{
   // ReSharper disable once UnusedMember.Global
   public partial class GeneratedTextTransformation
   {
      #region Template

      // EFDesigner v4.2.3.2
      // Copyright (c) 2017-2022 Michael Sawczyn
      // https://github.com/msawczyn/EFDesigner

      protected void NL()
      {
         WriteLine(string.Empty);
      }

      protected void Output(List<string> segments)
      {
         if (ModelRoot.ChopMethodChains)
            OutputChopped(segments);
         else
            Output(string.Join(".", segments) + ";");

         segments.Clear();
      }

      protected void OutputNoTerminator(List<string> segments)
      {
         if (ModelRoot.ChopMethodChains)
            OutputChoppedNoTerminator(segments);
         else
            Output(string.Join(".", segments));

         segments.Clear();
      }

      protected void Output(string text)
      {
         if (text == "}")
            PopIndent();

         WriteLine(text);

         if (text == "{")
         {
            PushIndent(ModelRoot.UseTabs
                          ? "\t"
                          : "   ");
         }
      }

      protected void Output(string template, params object[] items)
      {
         string text = string.Format(template, items);
         Output(text);
      }

      protected void OutputChopped(List<string> segments)
      {
         string[] segmentArray = segments?.ToArray() ?? Array.Empty<string>();

         if (!segmentArray.Any())
            return;

         int indent = segmentArray[0].IndexOf('.');

         if (indent == -1)
         {
            if (segmentArray.Length > 1)
            {
               segmentArray[0] = $"{segmentArray[0]}.{segmentArray[1]}";
               indent = segmentArray[0].IndexOf('.');
               segmentArray = segmentArray.Where((source, index) => index != 1).ToArray();
            }
         }

         for (int index = 1; index < segmentArray.Length; ++index)
            segmentArray[index] = $"{new string(' ', indent)}.{segmentArray[index]}";

         if (!segmentArray[segmentArray.Length - 1].Trim().EndsWith(";"))
            segmentArray[segmentArray.Length - 1] = segmentArray[segmentArray.Length - 1] + ";";

         foreach (string segment in segmentArray)
            Output(segment);

         segments.Clear();
      }

      protected void OutputChoppedNoTerminator(List<string> segments)
      {
         string[] segmentArray = segments?.ToArray() ?? Array.Empty<string>();

         if (!segmentArray.Any())
            return;

         int indent = segmentArray[0].IndexOf('.');

         if (indent == -1)
         {
            if (segmentArray.Length > 1)
            {
               segmentArray[0] = $"{segmentArray[0]}.{segmentArray[1]}";
               indent = segmentArray[0].IndexOf('.');
               segmentArray = segmentArray.Where((source, index) => index != 1).ToArray();
            }
         }

         for (int index = 1; index < segmentArray.Length; ++index)
            segmentArray[index] = $"{new string(' ', indent)}.{segmentArray[index]}";

         foreach (string segment in segmentArray)
            Output(segment);

         segments.Clear();
      }

      public abstract class EFModelGenerator
      {
         protected static string[] xmlDocTags =
         {
            @"<([\s]*c[\s]*[/]?[\s]*)>",
            @"<(/[\s]*c[\s]*)>",
            @"<([\s]*code[\s]*[/]?[\s]*)>",
            @"<(/[\s]*code[\s]*)>",
            @"<([\s]*description[\s]*[/]?[\s]*)>",
            @"<(/[\s]*description[\s]*)>",
            @"<([\s]*example[\s]*[/]?[\s]*)>",
            @"<(/[\s]*example[\s]*)>",
            @"<([\s]*exception cref=""[^""]+""[\s]*[/]?[\s]*)>",
            @"<(/[\s]*exception[\s]*)>",
            @"<([\s]*include file='[^']+' path='tagpath\[@name=""[^""]+""\]'[\s]*/)>",
            @"<([\s]*inheritdoc/[\s]*)>",
            @"<([\s]*item[\s]*[/]?[\s]*)>",
            @"<(/[\s]*item[\s]*)>",
            @"<([\s]*list type=""[^""]+""[\s]*[/]?[\s]*)>",
            @"<(/[\s]*list[\s]*)>",
            @"<([\s]*listheader[\s]*[/]?[\s]*)>",
            @"<(/[\s]*listheader[\s]*)>",
            @"<([\s]*para[\s]*[/]?[\s]*)>",
            @"<(/[\s]*para[\s]*)>",
            @"<([\s]*param name=""[^""]+""[\s]*[/]?[\s]*)>",
            @"<(/[\s]*param[\s]*)>",
            @"<([\s]*paramref name=""[^""]+""/[\s]*)>",
            @"<([\s]*permission cref=""[^""]+""[\s]*[/]?[\s]*)>",
            @"<(/[\s]*permission[\s]*)>",
            @"<([\s]*remarks[\s]*[/]?[\s]*)>",
            @"<(/[\s]*remarks[\s]*)>",
            @"<([\s]*returns[\s]*[/]?[\s]*)>",
            @"<(/[\s]*returns[\s]*)>",
            @"<([\s]*see cref=""[^""]+""/[\s]*)>",
            @"<([\s]*seealso cref=""[^""]+""/[\s]*)>",
            @"<([\s]*summary[\s]*[/]?[\s]*)>",
            @"<(/[\s]*summary[\s]*)>",
            @"<([\s]*term[\s]*[/]?[\s]*)>",
            @"<(/[\s]*term[\s]*)>",
            @"<([\s]*typeparam name=""[^""]+""[\s]*[/]?[\s]*)>",
            @"<(/[\s]*typeparam[\s]*)>",
            @"<([\s]*typeparamref name=""[^""]+""/[\s]*)>",
            @"<([\s]*value[\s]*[/]?[\s]*)>",
            @"<(/[\s]*value[\s]*)>"
         };

         protected EFModelGenerator(GeneratedTextTransformation host)
         {
            this.host = host;
            modelRoot = host.ModelRoot;
         }

         public static string[] NonNullableTypes
         {
            get
            {
               return new[]
                      {
                         "Binary",
                         "Geography",
                         "GeographyCollection",
                         "GeographyLineString",
                         "GeographyMultiLineString",
                         "GeographyMultiPoint",
                         "GeographyMultiPolygon",
                         "GeographyPoint",
                         "GeographyPolygon",
                         "Geometry",
                         "GeometryCollection",
                         "GeometryLineString",
                         "GeometryMultiLineString",
                         "GeometryMultiPoint",
                         "GeometryMultiPolygon",
                         "GeometryPoint",
                         "GeometryPolygon",
                         "String"
                      };
            }
         }

         protected void BeginNamespace(string ns)
         {
            if (!string.IsNullOrEmpty(ns))
            {
               Output($"namespace {ns}");
               Output("{");
            }
         }

         protected void ClearIndent() { host.ClearIndent(); }

         protected static string CreateShadowPropertyName(Association association, List<string> foreignKeyColumns, ModelAttribute identityAttribute)
         {
            string separator = identityAttribute.ModelClass.ModelRoot.ShadowKeyNamePattern == ShadowKeyPattern.TableColumn
                                  ? string.Empty
                                  : "_";

            string GetShadowPropertyName(string nameBase)
            {
               return $"{nameBase}{separator}{identityAttribute.Name}";
            }

            string GetShadowPropertyNameBase()
            {
               if (association.SourceRole == EndpointRole.Dependent)
                  return association.TargetPropertyName;

               if (association is BidirectionalAssociation b)
                  return b.SourcePropertyName;

               return $"{association.Source.Name}{separator}{association.TargetPropertyName}";
            }

            string shadowNameBase = GetShadowPropertyNameBase();
            string shadowPropertyName = GetShadowPropertyName(shadowNameBase);

            int index = 0;

            while (foreignKeyColumns.Contains(shadowPropertyName))
               shadowPropertyName = GetShadowPropertyName($"{shadowNameBase}{++index}");

            return shadowPropertyName;
         }

         protected void EndNamespace(string ns)
         {
            if (!string.IsNullOrEmpty(ns))
               Output("}");
         }

         protected string FullyQualified(string typeName)
         {
            string[] parts = typeName.Split('.');

            if (parts.Length == 1)
               return typeName;

            string simpleName = parts[0];
            ModelEnum modelEnum = modelRoot.Store.ElementDirectory.AllElements.OfType<ModelEnum>().FirstOrDefault(e => e.Name == simpleName);

            return modelEnum != null
                      ? $"{modelEnum.FullName}.{parts.Last()}"
                      : typeName;
         }

         public abstract void Generate(Manager efModelFileManager);

         protected string[] GenerateCommentBody(string comment)
         {
            List<string> result = new List<string>();

            if (!string.IsNullOrEmpty(comment))
            {
               int chunkSize = 80;
               string[] parts = comment.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

               foreach (string value in parts)
               {
                  string text = value;

                  while (text.Length > 0)
                  {
                     string outputText = text;

                     if (outputText.Length > chunkSize)
                     {
                        outputText = (text.IndexOf(' ', chunkSize) > 0
                                         ? text.Substring(0, text.IndexOf(' ', chunkSize))
                                         : text).Trim();

                        text = text.Substring(outputText.Length).Trim();
                     }
                     else
                        text = string.Empty;

                     result.Add(SecurityElement.Escape(outputText));
                  }
               }
            }

            return result.ToArray();
         }

         protected void GeneratePropertyAnnotations(ModelAttribute modelAttribute)
         {
            string customAttributes = modelAttribute.CustomAttributes ?? string.Empty;

            if (!modelAttribute.Persistent && modelAttribute.ModelClass.Persistent)
            {
               if (!customAttributes.Contains("NotMapped"))
                  Output("[NotMapped]");
            }
            else
            {
               if (modelAttribute.IsIdentity)
                  Output("[Key]");

               if (modelAttribute.Required)
                  Output("[Required]");

               if (modelAttribute.FQPrimitiveType == "string")
               {
                  if (modelAttribute.MinLength > 0)
                     Output($"[MinLength({modelAttribute.MinLength})]");

                  if (modelAttribute.MaxLength > 0)
                  {
                     Output($"[MaxLength({modelAttribute.MaxLength})]");
                     Output($"[StringLength({modelAttribute.MaxLength})]");
                  }
               }
            }

            if (!string.IsNullOrWhiteSpace(modelAttribute.DisplayText))
               Output($"[System.ComponentModel.DataAnnotations.Display(Name=\"{modelAttribute.DisplayText.Replace("\"", "\\\"")}\")]");

            if (!string.IsNullOrWhiteSpace(modelAttribute.Summary))
               Output($"[System.ComponentModel.Description(\"{modelAttribute.Summary.Trim('\r', '\n').Replace("\"", "\\\"")}\")]");
         }

         protected abstract List<string> GetAdditionalUsingStatements();

         protected string GetDefaultConstructorVisibility(ModelClass modelClass)
         {
            if (modelClass.DefaultConstructorVisibility == TypeAccessModifierExt.Default)
            {
               bool hasRequiredParameters = GetAllRequiredParameters(modelClass, false, true).Any();

               string visibility = (hasRequiredParameters || modelClass.IsAbstract) && !modelClass.IsDependentType
                                      ? "protected"
                                      : "public";

               return visibility;
            }

            return modelClass.DefaultConstructorVisibility.ToString().ToLowerInvariant();
         }

         protected string GetFullContainerName(string containerType, string payloadType)
         {
            string result;

            switch (containerType)
            {
               case "HashSet":
                  result = "System.Collections.Generic.HashSet<T>";

                  break;

               case "LinkedList":
                  result = "System.Collections.Generic.LinkedList<T>";

                  break;

               case "List":
                  result = "System.Collections.Generic.List<T>";

                  break;

               case "SortedSet":
                  result = "System.Collections.Generic.SortedSet<T>";

                  break;

               case "Collection":
                  result = "System.Collections.ObjectModel.Collection<T>";

                  break;

               case "ObservableCollection":
                  result = "System.Collections.ObjectModel.ObservableCollection<T>";

                  break;

               case "BindingList":
                  result = "System.ComponentModel.BindingList<T>";

                  break;

               default:
                  result = containerType;

                  break;
            }

            if (result.EndsWith("<T>"))
               result = result.Replace("<T>", $"<{payloadType}>");

            return result;
         }

         protected string GetMigrationNamespace()
         {
            List<string> nsParts = modelRoot.Namespace.Split('.').ToList();
            nsParts = nsParts.Take(nsParts.Count - 1).ToList();
            nsParts.Add("Migrations");

            return string.Join(".", nsParts);
         }

         private List<List<(ModelAttribute Property, NavigationProperty Navigation)>> GetAllRequiredParametersMix(ModelClass modelClass, bool? haveDefaults, bool publicOnly = false)
         {
            List<List<(ModelAttribute Property, NavigationProperty Navigation)>> result =
               new List<List<(ModelAttribute Property, NavigationProperty Navigation)>>();

            List<ModelAttribute> modelAttributes = GetAllRequiredParameterAttributes(modelClass, publicOnly)
                                                  .Where(x => (haveDefaults != true && string.IsNullOrEmpty(x.InitialValue))
                                                           || (haveDefaults != false && !string.IsNullOrEmpty(x.InitialValue)))
                                                  .ToList();

            List<(ModelAttribute Property, NavigationProperty Navigation)> resultBase =
               modelAttributes.Where(x => !x.IsForeignKeyProperty)
                              .Select(modelAttribute => ((ModelAttribute modelAttribute, NavigationProperty navigation))(modelAttribute, null))
                              .ToList();

            NavigationProperty[] navigationProperties = GetAllRequiredParameterNavigations(modelClass).ToArray();

            if (navigationProperties.Length == 0)
            {
               if (resultBase.Any())
                  result.Add(resultBase);
            }
            else
            {
               int permutationCount = (int)Math.Pow(2, navigationProperties.Length);

               for (int i = 0; i < permutationCount; ++i)
               {
                  List<(ModelAttribute Property, NavigationProperty Navigation)> permutation =
                     new List<(ModelAttribute Property, NavigationProperty Navigation)>(resultBase);

                  for (int j = 0; j < navigationProperties.Length; j++)
                  {
                     if ((i & (int)Math.Pow(2, j)) > 0)
                        permutation.Add((null, navigationProperties[j]));
                     else
                        permutation.Add((modelAttributes.FirstOrDefault(a => a.IsForeignKeyFor == navigationProperties[j].AssociationObject.Id), null));
                  }

                  permutation.RemoveAll(x => x.Property == null && x.Navigation == null);
                  result.Add(permutation);
               }
            }

            return result.Where(x => x.Any()).ToList();
         }

         /// <summary>Gets the local required properties for the ModelClass in formal parameter format</summary>
         /// <param name="modelClass">Source</param>
         /// <param name="haveDefaults">If true, only return those with default values. If false, only return those without default values. If null, return both.</param>
         /// <param name="publicOnly">If true, only return those with public setters. If false, only return those without public setters. If null, return both.</param>
         private List<string> GetAllRequiredParameters(ModelClass modelClass, bool? haveDefaults, bool publicOnly = false)
         {
            List<string> requiredParameters = new List<string>();

            if (haveDefaults != true)
            {
               // false or null - get those without default values 
               requiredParameters.AddRange(GetAllRequiredParameterAttributes(modelClass, publicOnly)
                                                     .Where(x => string.IsNullOrEmpty(x.InitialValue))
                                                     .Select(x => $"{x.FQPrimitiveType} {x.Name.ToLower()}"));

               requiredParameters.AddRange(GetAllRequiredParameterNavigations(modelClass).Select(x => $"{x.ClassType.FullName} {x.PropertyName.ToLower()}"));
            }

            if (haveDefaults != false)
            {
               // true or null - get those with default values
               requiredParameters.AddRange(GetAllRequiredParameterAttributes(modelClass, publicOnly)
                                                     .Where(x => !string.IsNullOrEmpty(x.InitialValue))
                                                     .Select(x =>
                                                             {
                                                                string quote = x.PrimitiveType == "string"
                                                                                  ? "\""
                                                                                  : x.PrimitiveType == "char"
                                                                                     ? "'"
                                                                                     : string.Empty;

                                                                string value = FullyQualified(quote.Length > 0
                                                                                                 ? x.InitialValue.Trim(quote[0])
                                                                                                 : x.InitialValue);

                                                                if (x.FQPrimitiveType == "decimal")
                                                                   value += "m";

                                                                return $"{x.FQPrimitiveType} {x.Name.ToLower()} = {quote}{value}{quote}";
                                                             }));
            }

            return requiredParameters;
         }

         private static IEnumerable<NavigationProperty> GetAllRequiredParameterNavigations(ModelClass modelClass)
         {
            // don't use 1..1 associations in constructor parameters. Becomes a Catch-22 scenario.
            return modelClass.AllRequiredNavigationProperties()
                             .Where(np => (np.AssociationObject.SourceMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.One)
                                       || (np.AssociationObject.TargetMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.One));
         }

         private static IEnumerable<ModelAttribute> GetAllRequiredParameterAttributes(ModelClass modelClass, bool publicOnly)
         {
            return modelClass.AllRequiredAttributes
                             .Where(x => (!x.IsIdentity || (x.IdentityType == IdentityType.Manual))
                                      && !x.IsConcurrencyToken
                                      && ((x.SetterVisibility == SetterAccessModifier.Public) || !publicOnly));
         }

         public static bool IsNullable(ModelAttribute modelAttribute)
         {
            return !modelAttribute.Required && !modelAttribute.IsIdentity && !modelAttribute.IsConcurrencyToken && !NonNullableTypes.Contains(modelAttribute.Type);
         }

         // implementations delegated to the surrounding GeneratedTextTransformation for backward compatability
         protected void NL() { host.NL(); }

         protected void Output(List<string> segments) { host.Output(segments); }

         protected void Output(string text) { host.Output(text); }

         protected void Output(string template, params object[] items) { host.Output(template, items); }

         protected void OutputNoTerminator(List<string> segments) { host.OutputNoTerminator(segments); }

         protected void PopIndent() { host.PopIndent(); }

         protected void PushIndent(string indent) { host.PushIndent(indent); }

         protected virtual void WriteClass(ModelClass modelClass)
         {
            Output("using System;");
            Output("using System.Collections.Generic;");
            Output("using System.Collections.ObjectModel;");
            Output("using System.ComponentModel;");
            Output("using System.ComponentModel.DataAnnotations;");
            Output("using System.ComponentModel.DataAnnotations.Schema;");
            Output("using System.Linq;");
            Output("using System.Runtime.CompilerServices;");

            List<string> additionalUsings = GetAdditionalUsingStatements();

            if (additionalUsings.Any())
               Output(string.Join("\n", additionalUsings));

            NL();

            BeginNamespace(modelClass.EffectiveNamespace);

            string isAbstract = modelClass.IsAbstract
                                   ? "abstract "
                                   : string.Empty;

            List<string> bases = new List<string>();

            if (modelClass.Superclass != null)
               bases.Add(modelClass.Superclass.FullName);

            if (!string.IsNullOrEmpty(modelClass.CustomInterfaces))
               bases.AddRange(modelClass.CustomInterfaces.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

            string baseClass = string.Join(", ", bases.Select(x => x.Trim()));

            if (!string.IsNullOrEmpty(modelClass.Summary))
            {
               Output("/// <summary>");
               WriteCommentBody(modelClass.Summary);
               Output("/// </summary>");
            }

            if (!string.IsNullOrEmpty(modelClass.Description))
            {
               Output("/// <remarks>");
               WriteCommentBody(modelClass.Description);
               Output("/// </remarks>");
            }

            string customAttributes = modelClass.CustomAttributes ?? string.Empty;

            if (!modelClass.Persistent && !customAttributes.Contains("NotMapped"))
               Output("[NotMapped]");

            if (!string.IsNullOrWhiteSpace(customAttributes))
               Output($"[{customAttributes.Trim('[', ']')}]");

            if (!string.IsNullOrWhiteSpace(modelClass.Summary))
               Output($"[System.ComponentModel.Description(\"{modelClass.Summary.Trim('\r', '\n').Replace("\"", "\\\"")}\")]");

            Output(baseClass.Length > 0
                      ? $"public {isAbstract}partial class {modelClass.Name}: {baseClass}"
                      : $"public {isAbstract}partial class {modelClass.Name}");

            Output("{");

            WriteConstructors(modelClass);
            WriteProperties(modelClass);
            WriteNavigationProperties(modelClass);

            Output("}");

            EndNamespace(modelClass.EffectiveNamespace);
            NL();
         }

         protected void WriteCommentBody(string comment)
         {
            foreach (string s in GenerateCommentBody(comment))
               Output($"/// {s}");
         }

         protected void WriteConstructors(ModelClass modelClass)
         {
            Output("partial void Init();");
            NL();

            WriteDefaultConstructor(modelClass);

            if (GetDefaultConstructorVisibility(modelClass) != "public" && !modelClass.IsAbstract)
            {
               Output("/// <summary>");
               Output("/// Replaces default constructor, since it's protected. Caller assumes responsibility for setting all required values before saving.");
               Output("/// </summary>");
               Output($"public static {modelClass.Name} Create{modelClass.Name}Unsafe()");
               Output("{");
               Output($"return new {modelClass.Name}();");
               Output("}");
               NL();
            }

            /***********************************************************************/
            // Constructors with required parameters (if necessary)
            /***********************************************************************/

            List<List<(ModelAttribute Property, NavigationProperty Navigation)>> requiredParametersMix = GetAllRequiredParametersMix(modelClass, false, true);

            if (requiredParametersMix.Any())
            {
               foreach (List<(ModelAttribute Property, NavigationProperty Navigation)> parameters in requiredParametersMix)
                  WriteConstructorWithRequiredProperties(modelClass, GetConstructorNote(modelClass), parameters);

               if (!modelClass.IsAbstract)
               {
                  foreach (List<(ModelAttribute Property, NavigationProperty Navigation)> parameters in requiredParametersMix)
                  {
                     Output("/// <summary>");
                     Output("/// Static create function (for use in LINQ queries, etc.)");
                     Output("/// </summary>");
                     WriteConstructorParameterComments(parameters);

                     List<string> parameterDeclarations = parameters.Select(GetParameterDeclaration).ToList();

                     //string newToken = string.Empty;
                     //if (!AllSuperclassesAreNullOrAbstract(modelClass))
                     //{
                     //   List<string> superclassRequiredParameters = GetAllRequiredParameters(modelClass.Superclass, null, true);

                     //   if (!requiredParameters.Except(superclassRequiredParameters).Any())
                     //      newToken = "new ";
                     //}

                     List<string> parameterNames = parameters.Select(p => p.Property != null
                                                                             ? p.Property.Name.ToLowerInvariant()
                                                                             : p.Navigation.PropertyName.ToLowerInvariant())
                                                             .ToList();

                     Output($"public static {modelClass.Name} Create({string.Join(", ", parameterDeclarations)})");
                     Output("{");
                     Output($"return new {modelClass.Name}({string.Join(", ", parameterNames)});");
                     Output("}");
                     NL();
                  }
               }
            }         }

         private void WriteDefaultConstructor(ModelClass modelClass)
         {
            bool hasRequiredParameters = GetAllRequiredParametersMix(modelClass, false, true).Any();
            string visibility = GetDefaultConstructorVisibility(modelClass);

            if (visibility == "public")
            {
               Output("/// <summary>");
               Output("/// Default constructor");
               Output("/// </summary>");
            }
            else if (modelClass.IsAbstract)
            {
               Output("/// <summary>");
               Output("/// Default constructor. Protected due to being abstract.");
               Output("/// </summary>");
            }
            else if (hasRequiredParameters)
            {
               Output("/// <summary>");
               Output("/// Default constructor. Protected due to required properties, but present because EF needs it.");
               Output("/// </summary>");
            }

            Output(modelClass.Superclass != null
                      ? $"{visibility} {modelClass.Name}(): base()"
                      : $"{visibility} {modelClass.Name}()");

            Output("{");

            List<string> constructorNote = GetConstructorNote(modelClass);

            if (constructorNote.Count > 0)
            {
               foreach (string comment in constructorNote)
                  Output(comment);

               NL();
            }

            WriteDefaultConstructorBody(modelClass);

            Output("}");
            NL();
         }

         private static List<string> GetConstructorNote(ModelClass modelClass)
         {
            List<string> constructorNote = new List<string>();

            bool hasOneToOneAssociations = modelClass.AllRequiredNavigationProperties()
                                                     .Any(np => (np.AssociationObject.SourceMultiplicity == Sawczyn.EFDesigner.EFModel.Multiplicity.One)
                                                             && (np.AssociationObject.TargetMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.One));

            if (hasOneToOneAssociations)
            {
               List<Association> oneToOneAssociations = modelClass.AllRequiredNavigationProperties()
                                                                  .Where(np => (np.AssociationObject.SourceMultiplicity == Sawczyn.EFDesigner.EFModel.Multiplicity.One)
                                                                            && (np.AssociationObject.TargetMultiplicity == Sawczyn.EFDesigner.EFModel.Multiplicity.One))
                                                                  .Select(np => np.AssociationObject)
                                                                  .ToList();

               List<ModelClass> otherEndsOneToOne = oneToOneAssociations.Where(a => a.Source != modelClass)
                                                                        .Select(a => a.Target)
                                                                        .Union(oneToOneAssociations.Where(a => a.Target != modelClass).Select(a => a.Source))
                                                                        .ToList();

               if (oneToOneAssociations.Any(a => (a.Source.Name == modelClass.Name) && (a.Target.Name == modelClass.Name)))
                  otherEndsOneToOne.Add(modelClass);

               if (otherEndsOneToOne.Any())
               {
                  string nameList = otherEndsOneToOne.Count == 1
                                       ? otherEndsOneToOne.First().Name
                                       : string.Join(", ", otherEndsOneToOne.Take(otherEndsOneToOne.Count - 1).Select(c => c.Name))
                                       + " and "
                                       + (otherEndsOneToOne.Last().Name != modelClass.Name
                                             ? otherEndsOneToOne.Last().Name
                                             : "itself");

                  constructorNote.Add($"// NOTE: This class has one-to-one associations with {nameList}.");
                  constructorNote.Add("// One-to-one associations are not validated in constructors since this causes a scenario where each one must be constructed before the other.");
               }
            }

            return constructorNote;
         }

         protected void WriteConstructorParameterComments(List<(ModelAttribute Property, NavigationProperty Navigation)> parameters)
         {
            foreach ((ModelAttribute Property, NavigationProperty Navigation) parameter in parameters)
            {
               Output(parameter.Property != null
                         ? $@"/// <param name=""{parameter.Property.Name.ToLower()}"">{string.Join(" ", GenerateCommentBody(parameter.Property.Summary))}</param>"
                         : $@"/// <param name=""{parameter.Navigation.PropertyName.ToLower()}"">{string.Join(" ", GenerateCommentBody(parameter.Navigation.Summary))}</param>");
            }
         }

         private void WriteConstructorWithRequiredProperties(ModelClass modelClass, List<string> constructorNotes, List<(ModelAttribute Property, NavigationProperty Navigation)> parameters)
         {
            string visibility = modelClass.IsAbstract
                                   ? "protected"
                                   : "public";

            Output("/// <summary>");
            Output("/// Public constructor with required data");
            Output("/// </summary>");

            WriteConstructorParameterComments(parameters);
            Output($"{visibility} {modelClass.Name}({string.Join(", ", parameters.Select(GetParameterDeclaration))}) : this()");
            Output("{");

            if (constructorNotes.Count > 0)
            {
               foreach (string remark in constructorNotes)
                  Output(remark);

               NL();
            }

            foreach ((ModelAttribute Property, NavigationProperty Navigation) parameter in parameters)
            {
               if (parameter.Property != null)
               {
                  ModelAttribute requiredProperty = parameter.Property;

                  if ((!requiredProperty.IsIdentity || requiredProperty.IdentityType == IdentityType.Manual)
                   && !requiredProperty.IsConcurrencyToken
                   && requiredProperty.SetterVisibility == SetterAccessModifier.Public)
                  {
                     if (requiredProperty.Type == "String")
                        Output($"if (string.IsNullOrEmpty({requiredProperty.Name.ToLower()})) throw new ArgumentNullException(nameof({requiredProperty.Name.ToLower()}));");
                     else if (requiredProperty.Type.StartsWith("Geo"))
                        Output($"if ({requiredProperty.Name.ToLower()} == null) throw new ArgumentNullException(nameof({requiredProperty.Name.ToLower()}));");

                     string lhs = requiredProperty.AutoProperty || string.IsNullOrEmpty(requiredProperty.BackingFieldName)
                                     ? requiredProperty.Name
                                     : requiredProperty.BackingFieldName;

                     Output($"this.{lhs} = {requiredProperty.Name.ToLower()};");
                     NL();
                  }
                  else if (requiredProperty.SetterVisibility == SetterAccessModifier.Public
                        && !requiredProperty.Required
                        && !string.IsNullOrEmpty(requiredProperty.InitialValue)
                        && requiredProperty.InitialValue != "null")
                  {
                     string quote = requiredProperty.Type == "String"
                                       ? "\""
                                       : requiredProperty.Type == "Char"
                                          ? "'"
                                          : string.Empty;

                     string initialValue = requiredProperty.InitialValue;

                     if (requiredProperty.Type == "decimal")
                        initialValue += "m";

                     string lhs = requiredProperty.AutoProperty || string.IsNullOrEmpty(requiredProperty.BackingFieldName)
                                     ? requiredProperty.Name
                                     : requiredProperty.BackingFieldName;

                     Output(quote.Length > 0
                               ? $"this.{lhs} = {quote}{FullyQualified(initialValue.Trim(quote[0]))}{quote};"
                               : $"this.{lhs} = {quote}{FullyQualified(initialValue)}{quote};");
                  }
               }
               else if (parameter.Navigation != null)
               {
                  NavigationProperty requiredNavigationProperty = parameter.Navigation;
                  NavigationProperty otherSide = requiredNavigationProperty.OtherSide;
                  string parameterName = requiredNavigationProperty.PropertyName.ToLower();
                  Output($"if ({parameterName} == null) throw new ArgumentNullException(nameof({parameterName}));");

                  string targetObjectName = requiredNavigationProperty.IsAutoProperty
                                               ? requiredNavigationProperty.PropertyName
                                               : requiredNavigationProperty.BackingFieldName;

                  if (!requiredNavigationProperty.ConstructorParameterOnly)
                  {
                     Output(requiredNavigationProperty.IsCollection
                               ? $"this.{targetObjectName}.Add({parameterName});"
                               : $"this.{targetObjectName} = {parameterName};");
                  }

                  if (!string.IsNullOrEmpty(otherSide.PropertyName))
                  {
                     Output(otherSide.IsCollection
                               ? $"{parameterName}.{otherSide.PropertyName}.Add(this);"
                               : $"{parameterName}.{otherSide.PropertyName} = this;");
                  }

                  NL();
               }
            }

            Output("}");
            NL();
         }

         private string GetParameterDeclaration((ModelAttribute modelAttribute, NavigationProperty navigation) parameter)
         {
            return parameter.modelAttribute != null
                      ? $"{parameter.modelAttribute.FQPrimitiveType} {parameter.modelAttribute.Name.ToLower()}"
                      : $"{parameter.navigation.ClassType.FullName} {parameter.navigation.PropertyName.ToLower()}";
         }

         protected void WriteDbContextComments()
         {
            if (!string.IsNullOrEmpty(modelRoot.Summary))
            {
               Output("/// <summary>");
               WriteCommentBody(modelRoot.Summary);
               Output("/// </summary>");

               if (!string.IsNullOrEmpty(modelRoot.Description))
               {
                  Output("/// <remarks>");
                  WriteCommentBody(modelRoot.Description);
                  Output("/// </remarks>");
               }
            }
            else
               Output("/// <inheritdoc/>");
         }

         protected void WriteDefaultConstructorBody(ModelClass modelClass)
         {
            int lineCount = 0;

            foreach (ModelAttribute modelAttribute in modelClass.Attributes.Where(x => (x.SetterVisibility == SetterAccessModifier.Public)
                                                                                    && !string.IsNullOrEmpty(x.InitialValue)
                                                                                    && (x.InitialValue.Trim('"') != "null")))
            {
               string quote = modelAttribute.Type == "String"
                                 ? "\""
                                 : modelAttribute.Type == "Char"
                                    ? "'"
                                    : string.Empty;

               string initialValue = modelAttribute.InitialValue;

               if (modelAttribute.Type == "decimal")
                  initialValue += "m";

               string lhs = modelAttribute.AutoProperty || string.IsNullOrEmpty(modelAttribute.BackingFieldName)
                               ? modelAttribute.Name
                               : modelAttribute.BackingFieldName;

               Output(quote.Length == 1
                         ? $"{lhs} = {quote}{FullyQualified(initialValue.Trim(quote[0]))}{quote};"
                         : $"{lhs} = {quote}{FullyQualified(initialValue)}{quote};");

               ++lineCount;
            }

            lineCount += WriteNavigationInitializersForConstructors(modelClass);

            if (lineCount > 0)
               NL();

            Output("Init();");
         }

         protected void WriteEnum(ModelEnum modelEnum)
         {
            Output("using System;");
            NL();

            BeginNamespace(modelEnum.EffectiveNamespace);

            if (!string.IsNullOrEmpty(modelEnum.Summary))
            {
               Output("/// <summary>");
               WriteCommentBody(modelEnum.Summary);
               Output("/// </summary>");
            }

            if (!string.IsNullOrEmpty(modelEnum.Description))
            {
               Output("/// <remarks>");
               WriteCommentBody(modelEnum.Description);
               Output("/// </remarks>");
            }

            if (modelEnum.IsFlags)
               Output("[Flags]");

            if (!string.IsNullOrWhiteSpace(modelEnum.CustomAttributes))
               Output($"[{modelEnum.CustomAttributes.Trim('[', ']')}]");

            if (!string.IsNullOrWhiteSpace(modelEnum.Summary))
               Output($"[System.ComponentModel.Description(\"{modelEnum.Summary.Trim('\r', '\n').Replace("\"", "\\\"")}\")]");

            Output($"public enum {modelEnum.Name} : {modelEnum.ValueType}");
            Output("{");

            ModelEnumValue[] values = modelEnum.Values.ToArray();

            for (int index = 0; index < values.Length; ++index)
            {
               if (!string.IsNullOrEmpty(values[index].Summary))
               {
                  Output("/// <summary>");
                  WriteCommentBody(values[index].Summary);
                  Output("/// </summary>");
               }

               if (!string.IsNullOrEmpty(values[index].Description))
               {
                  Output("/// <remarks>");
                  WriteCommentBody(values[index].Description);
                  Output("/// </remarks>");
               }

               if (!string.IsNullOrWhiteSpace(values[index].CustomAttributes))
                  Output($"[{values[index].CustomAttributes.Trim('[', ']')}]");

               if (!string.IsNullOrWhiteSpace(values[index].Summary))
                  Output($"[System.ComponentModel.Description(\"{values[index].Summary.Trim('\r', '\n').Replace("\"", "\\\"")}\")]");

               if (!string.IsNullOrWhiteSpace(values[index].DisplayText))
                  Output($"[System.ComponentModel.DataAnnotations.Display(Name=\"{values[index].DisplayText.Trim('\r', '\n').Replace("\"", "\\\"")}\")]");

               Output(string.IsNullOrEmpty(values[index].Value)
                         ? $"{values[index].Name}{(index < values.Length - 1 ? "," : string.Empty)}"
                         : $"{values[index].Name} = {values[index].Value}{(index < values.Length - 1 ? "," : string.Empty)}");
            }

            Output("}");

            EndNamespace(modelEnum.EffectiveNamespace);
         }

         private int WriteNavigationInitializersForConstructors(ModelClass modelClass)
         {
            int lineCount = 0;

            foreach (NavigationProperty navigationProperty in modelClass.LocalNavigationProperties()
                                                                        .Where(x => x.AssociationObject.Persistent
                                                                                 && x.IsCollection
                                                                                 && !x.ConstructorParameterOnly))
            {
               string collectionType = GetFullContainerName(navigationProperty.AssociationObject.CollectionClass, navigationProperty.ClassType.FullName);

               Output(navigationProperty.IsAutoProperty || string.IsNullOrEmpty(navigationProperty.BackingFieldName)
                         ? $"{navigationProperty.PropertyName} = new {collectionType}();"
                         : $"{navigationProperty.BackingFieldName} = new {collectionType}();");

               ++lineCount;
            }

            foreach (NavigationProperty navigationProperty in modelClass.LocalNavigationProperties()
                                                                        .Where(x => x.AssociationObject.Persistent
                                                                                 && !x.IsCollection
                                                                                 && !x.ConstructorParameterOnly
                                                                                 && x.Required
                                                                                 && x.OtherSide.ClassType.IsDependentType))
            {
               Output(navigationProperty.IsAutoProperty || string.IsNullOrEmpty(navigationProperty.BackingFieldName)
                         ? $"{navigationProperty.PropertyName} = new {navigationProperty.OtherSide.ClassType.Namespace}.{navigationProperty.OtherSide.ClassType.Name}();"
                         : $"{navigationProperty.BackingFieldName} = new {navigationProperty.OtherSide.ClassType.Namespace}.{navigationProperty.OtherSide.ClassType.Name}();");

               ++lineCount;
            }

            return lineCount;
         }

         [SuppressMessage("ReSharper", "ConvertIfStatementToConditionalTernaryExpression")]
         protected void WriteNavigationProperties(ModelClass modelClass)
         {
            if (!modelClass.LocalNavigationProperties().Any())
               return;

            Output("/*************************************************************************");
            Output(" * Navigation properties");
            Output(" *************************************************************************/");
            NL();

            foreach (NavigationProperty navigationProperty in modelClass.LocalNavigationProperties()
                                                                        .Where(x => !x.ConstructorParameterOnly)
                                                                        .OrderBy(x => x.PropertyName))
            {
               string type = navigationProperty.IsCollection
                                ? $"ICollection<{navigationProperty.ClassType.FullName}>"
                                : navigationProperty.ClassType.FullName;

               if (!navigationProperty.IsAutoProperty)
               {
                  Output("/// <summary>");
                  Output($"/// Backing field for {navigationProperty.PropertyName}");
                  Output("/// </summary>");
                  Output($"protected {type} {navigationProperty.BackingFieldName};");
                  NL();

                  if (!navigationProperty.IsCollection)
                  {
                     Output("/// <summary>");
                     Output($"/// When provided in a partial class, allows value of {navigationProperty.PropertyName} to be changed before setting.");
                     Output("/// </summary>");
                     Output($"partial void Set{navigationProperty.PropertyName}({type} oldValue, ref {type} newValue);");
                     NL();

                     Output("/// <summary>");
                     Output($"/// When provided in a partial class, allows value of {navigationProperty.PropertyName} to be changed before returning.");
                     Output("/// </summary>");
                     Output($"partial void Get{navigationProperty.PropertyName}(ref {type} result);");

                     NL();
                  }
               }

               List<string> comments = new List<string>();

               if (navigationProperty.Required)
                  comments.Add("Required");

               string comment = comments.Count > 0
                                   ? string.Join(", ", comments)
                                   : string.Empty;

               if (!string.IsNullOrEmpty(navigationProperty.Summary) || !string.IsNullOrEmpty(comment))
               {
                  Output("/// <summary>");

                  if (!string.IsNullOrEmpty(comment) && !string.IsNullOrEmpty(navigationProperty.Summary))
                     comment += "<br/>";

                  if (!string.IsNullOrEmpty(comment))
                     WriteCommentBody(comment);

                  if (!string.IsNullOrEmpty(navigationProperty.Summary))
                     WriteCommentBody(navigationProperty.Summary);

                  Output("/// </summary>");
               }

               if (!string.IsNullOrEmpty(navigationProperty.Description))
               {
                  Output("/// <remarks>");
                  WriteCommentBody(navigationProperty.Description);
                  Output("/// </remarks>");
               }

               string customAttributes = navigationProperty.CustomAttributes ?? string.Empty;

               if (!string.IsNullOrWhiteSpace(customAttributes))
                  Output($"[{customAttributes.Trim('[', ']')}]");

               if (!string.IsNullOrWhiteSpace(navigationProperty.Summary))
                  Output($"[System.ComponentModel.Description(\"{navigationProperty.Summary.Replace("\"", "\\\"")}\")]");

               if (!string.IsNullOrWhiteSpace(navigationProperty.DisplayText))
                  Output($"[System.ComponentModel.DataAnnotations.Display(Name=\"{navigationProperty.DisplayText.Replace("\"", "\\\"")}\")]");

               if (!navigationProperty.AssociationObject.Persistent && modelClass.Persistent)
               {
                  if (!customAttributes.Contains("NotMapped"))
                     Output("[NotMapped]");
               }

               if (navigationProperty.IsAutoProperty)
               {
                  if (navigationProperty.IsCollection)
                     Output($"public virtual {type} {navigationProperty.PropertyName} {{ get; private set; }}");
                  else
                     Output($"public virtual {type} {navigationProperty.PropertyName} {{ get; set; }}");
               }
               else if (navigationProperty.IsCollection)
               {
                  Output($"public virtual {type} {navigationProperty.PropertyName}");
                  Output("{");
                  Output("get");
                  Output("{");
                  Output($"return {navigationProperty.BackingFieldName};");
                  Output("}");
                  Output("private set");
                  Output("{");
                  Output($"{navigationProperty.BackingFieldName} = value;");
                  Output("}");
                  Output("}");
               }
               else
               {
                  Output($"public virtual {type} {navigationProperty.PropertyName}");
                  Output("{");
                  Output("get");
                  Output("{");
                  Output($"{type} value = {navigationProperty.BackingFieldName};");
                  Output($"Get{navigationProperty.PropertyName}(ref value);");
                  Output($"return ({navigationProperty.BackingFieldName} = value);");
                  Output("}");
                  Output("set");
                  Output("{");
                  Output($"{type} oldValue = {navigationProperty.PropertyName};");
                  Output($"Set{navigationProperty.PropertyName}(oldValue, ref value);");
                  Output("if (oldValue != value)");
                  Output("{");
                  Output($"{navigationProperty.BackingFieldName} = value;");

                  if (navigationProperty.ImplementNotify)
                     Output("OnPropertyChanged();");

                  Output("}");
                  Output("}");
                  Output("}");
               }

               NL();
            }
         }

         protected void WriteProperties(ModelClass modelClass)
         {
            if (!modelClass.Attributes.Any())
               return;

            Output("/*************************************************************************");
            Output(" * Properties");
            Output(" *************************************************************************/");
            NL();

            List<string> segments = new List<string>();

            foreach (ModelAttribute modelAttribute in modelClass.Attributes.OrderBy(x => x.Name))
            {
               segments.Clear();

               if (modelAttribute.IsIdentity)
                  segments.Add("Identity");

               if (modelAttribute.Indexed)
                  segments.Add("Indexed");

               if (modelAttribute.Required || modelAttribute.IsIdentity)
                  segments.Add("Required");

               if (modelAttribute.MinLength > 0)
                  segments.Add($"Min length = {modelAttribute.MinLength}");

               if (modelAttribute.MaxLength > 0)
                  segments.Add($"Max length = {modelAttribute.MaxLength}");

               if (!string.IsNullOrEmpty(modelAttribute.InitialValue))
               {
                  string quote = modelAttribute.PrimitiveType == "string"
                                    ? "\""
                                    : modelAttribute.PrimitiveType == "char"
                                       ? "'"
                                       : string.Empty;

                  string initialValue = modelAttribute.InitialValue;

                  if (modelAttribute.Type == "decimal")
                     initialValue += "m";

                  segments.Add($"Default value = {quote}{FullyQualified(initialValue.Trim('"'))}{quote}");
               }

               string nullable = IsNullable(modelAttribute)
                                    ? "?"
                                    : string.Empty;

               string @virtual = modelAttribute.Virtual && !modelAttribute.IsConcurrencyToken
                                    ? "virtual "
                                    : string.Empty;

               if (!modelAttribute.IsConcurrencyToken && !modelAttribute.AutoProperty)
               {
                  string visibility = modelAttribute.Indexed
                                         ? "internal"
                                         : "protected";

                  Output("/// <summary>");
                  Output($"/// Backing field for {modelAttribute.Name}");
                  Output("/// </summary>");
                  Output($"{visibility} {modelAttribute.FQPrimitiveType}{nullable} {modelAttribute.BackingFieldName};");
                  Output("/// <summary>");
                  Output($"/// When provided in a partial class, allows value of {modelAttribute.Name} to be changed before setting.");
                  Output("/// </summary>");
                  Output($"partial void Set{modelAttribute.Name}({modelAttribute.FQPrimitiveType}{nullable} oldValue, ref {modelAttribute.FQPrimitiveType}{nullable} newValue);");
                  Output("/// <summary>");
                  Output($"/// When provided in a partial class, allows value of {modelAttribute.Name} to be changed before returning.");
                  Output("/// </summary>");
                  Output($"partial void Get{modelAttribute.Name}(ref {modelAttribute.FQPrimitiveType}{nullable} result);");

                  NL();
               }

               if (!string.IsNullOrEmpty(modelAttribute.Summary) || segments.Any())
               {
                  Output("/// <summary>");

                  if (segments.Any())
                     WriteCommentBody($"{string.Join(", ", segments)}");

                  if (!string.IsNullOrEmpty(modelAttribute.Summary))
                     WriteCommentBody(modelAttribute.Summary);

                  Output("/// </summary>");
               }

               if (!string.IsNullOrEmpty(modelAttribute.Description))
               {
                  Output("/// <remarks>");
                  WriteCommentBody(modelAttribute.Description);
                  Output("/// </remarks>");
               }

               string setterVisibility = modelAttribute.SetterVisibility == SetterAccessModifier.Protected
                                            ? "protected "
                                            : modelAttribute.SetterVisibility == SetterAccessModifier.Internal
                                               ? "internal "
                                               : string.Empty;

               GeneratePropertyAnnotations(modelAttribute);

               if (!string.IsNullOrWhiteSpace(modelAttribute.CustomAttributes))
                  Output($"[{modelAttribute.CustomAttributes.Trim('[', ']')}]");

               if (modelAttribute.IsAbstract)
                  Output($"public abstract {modelAttribute.FQPrimitiveType}{nullable} {modelAttribute.Name} {{ get; {setterVisibility}set; }}");
               else if (modelAttribute.IsConcurrencyToken || modelAttribute.AutoProperty)
                  Output($"public {@virtual}{modelAttribute.FQPrimitiveType}{nullable} {modelAttribute.Name} {{ get; {setterVisibility}set; }}");
               else
               {
                  Output($"public {@virtual}{modelAttribute.FQPrimitiveType}{nullable} {modelAttribute.Name}");
                  Output("{");
                  Output("get");
                  Output("{");
                  Output($"{modelAttribute.FQPrimitiveType}{nullable} value = {modelAttribute.BackingFieldName};");
                  Output($"Get{modelAttribute.Name}(ref value);");
                  Output($"return ({modelAttribute.BackingFieldName} = value);");
                  Output("}");
                  Output($"{setterVisibility}set");
                  Output("{");
                  Output($"{modelAttribute.FQPrimitiveType}{nullable} oldValue = {modelAttribute.Name};");
                  Output($"Set{modelAttribute.Name}(oldValue, ref value);");
                  Output("if (oldValue != value)");
                  Output("{");
                  Output($"{modelAttribute.BackingFieldName} = value;");

                  if (modelAttribute.ImplementNotify)
                     Output("OnPropertyChanged();");

                  Output("}");
                  Output("}");
                  Output("}");
               }

               NL();
            }

            if (!modelClass.AllAttributes.Any(x => x.IsConcurrencyToken)
             && ((modelClass.Concurrency == ConcurrencyOverride.Optimistic) || (modelRoot.ConcurrencyDefault == Concurrency.Optimistic)))
            {
               Output("/// <summary>");
               Output("/// Concurrency token");
               Output("/// </summary>");
               Output("[System.ComponentModel.DataAnnotations.Timestamp]");
               Output("public Byte[] Timestamp { get; set; }");
               NL();
            }
         }

#pragma warning disable IDE1006 // Naming Styles
         protected ModelRoot modelRoot { get; set; }
         protected GeneratedTextTransformation host { get; set; }
#pragma warning restore IDE1006 // Naming Styles
      }

      #endregion Template
   }
}