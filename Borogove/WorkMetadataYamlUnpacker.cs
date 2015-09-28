using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wyam.Common;
using Wyam.Core.Modules;
using Wyam.Modules.Yaml;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Dynamic;
using System.Diagnostics;
using Model = Borogove.Model;
using static Borogove.WorkMetadataCanonicalNames;
using System.Globalization;

namespace Borogove
{
    public class WorkMetadataYamlUnpacker : IModule
    {
        public const string DefaultKeyName = "Borogove";

        private readonly string _key;
        private readonly bool _flatten;
        private readonly IModule[] _modules;

        public WorkMetadataYamlUnpacker() : this(DefaultKeyName, false)
        {
        }

        public WorkMetadataYamlUnpacker(string key) : this(key, false)
        {
        }

        public WorkMetadataYamlUnpacker(bool flatten) : this(DefaultKeyName, flatten)
        {
        }

        public WorkMetadataYamlUnpacker(string key, bool flatten, params IModule[] modules)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            if (modules == null)
            {
                throw new ArgumentNullException("metadataModules");
            }

            _key = key;
            _flatten = flatten;
            var test2 = new FrontMatter(new Yaml(_key, _flatten));
            var test = new IModule[] { test2, };
            _modules = modules.Length == 0 ? new IModule[] { new FrontMatter(new Yaml(_key, _flatten)), } : modules;
        }

        public IEnumerable<IDocument> Execute(IReadOnlyList<IDocument> inputs, IExecutionContext context)
        {
            if (inputs == null)
            {
                throw new ArgumentNullException("inputs");
            }

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var processedDocuments = context.Execute(_modules, inputs);
            foreach (IDocument document in processedDocuments)
            {
                Dictionary<string, object> processedMetadata = document.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                dynamic borogroveObject = document.Get(_key);
                if (borogroveObject == null)
                {
                    continue;
                }

                Dictionary<string, object> borogroveDictionary = borogroveObject;
                foreach (KeyValuePair<string, object> keyValuePair in borogroveDictionary)
                {
                    string canonicalizedKey = CanonicalizeString(keyValuePair.Key);
                    dynamic value = keyValuePair.Value;
                    string stringValue = (string)value;
                    switch (canonicalizedKey)
                    {
                        case Identifier:
                        case Parent:
                        case Previous:
                        case Next:
                        case DraftOf:
                        case ArtifactOf:
                        case CommentsOn:
                            processedMetadata.Add(canonicalizedKey, Guid.Parse(stringValue));
                            continue;

                        case Title:
                        case Description:
                        case DraftIdentifier:
                            processedMetadata.Add(canonicalizedKey, stringValue);
                            continue;

                        case Creator:
                            var creators = new List<Model.Creator>();
                            foreach (var creator in value.Children)
                            {
                                var creatorObject = new Model.Creator()
                                {
                                    Role = Model.RoleUtilities.ParseFriendlyName((string)creator["role"]),
                                    Text = (string)creator["text"],
                                    FileAs = (string)creator["file-as"],
                                };
                                creators.Add(creatorObject);
                            }
                            processedMetadata.Add(Creator, creators);
                            continue;

                        case CreatedDate:
                        case ModifiedDate:
                        case PublishedDate:
                            processedMetadata.Add(canonicalizedKey, DateTime.Parse(stringValue));
                            continue;

                        case Rights:
                        case License:
                            processedMetadata.Add(Rights, stringValue);
                            Model.License parsedLicense;
                            if (Model.LicenseUtilities.TryParseLicense(stringValue, out parsedLicense))
                            {
                                processedMetadata.Add(License, parsedLicense);
                            }
                            else
                            {
                                context.Trace.Warning($"Unable to parse license: {stringValue}");
                            }
                            continue;

                        case Language:
                            processedMetadata.Add(Language, CultureInfo.GetCultureInfo(stringValue));
                            continue;

                        case WorkType:
                            processedMetadata.Add(WorkType, Model.WorkTypeUtilities.ParseFriendlyName(stringValue));
                            continue;

                        case ContentRating:
                            processedMetadata.Add(ContentRating, Model.ContentRatingUtilities.ParseShortName(stringValue));
                            continue;

                        case ContentDescriptors:
                            processedMetadata.Add(ContentDescriptors, Model.ContentDescriptorUtilities.ParseContentDescriptor(stringValue));
                            continue;

                        default:
                            if (processedMetadata.ContainsKey(keyValuePair.Key))
                            {
                                // Don't replace extant keys, we assume flatten or previous processing got it right.
                                break;
                            }
                            else
                            {
                                dynamic originalValue = keyValuePair.Value;
                                object processedValue = keyValuePair.Value;
                                if (originalValue.Count == 0)
                                {
                                    // Should be scalar, unpack the raw YAML through a cast.
                                    var test = (string)originalValue;
                                    var yamlValue = (YamlScalarNode)YamlDoc.LoadFromString((string)originalValue);
                                    processedValue = yamlValue.Value;
                                }
                                // If it is not scalar, we'll save the original DynamicYaml object.

                                // Add under the original key.
                                processedMetadata.Add(keyValuePair.Key, processedValue);
                                break;
                            }
                    }
                }
                // Really, only do the meta-data tying together. If it's a known
                // property, convert it, tie it, whatever. The assumption is that
                // all properties will be migrated to the top level metadata rather
                // than being stuck in the Borogove dynamic object. That object will
                // be left there. Any properties that are unknown on the Borogove object
                // will be migrated as-is.

                // The return value of execute will be all the input documents with
                // the metadata added. If this can be done with lazy processing, do it.
                yield return document.Clone(document.Source, document.Content, processedMetadata);
            }

            yield break;
        }
    }
}
