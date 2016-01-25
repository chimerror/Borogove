using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Wyam.Common.Documents;
using Wyam.Common.Modules;
using Wyam.Common.Pipelines;
using Wyam.Core.Modules.Control;
using Wyam.Modules.Yaml;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Dynamic;

using static Borogove.WorkMetadataCanonicalNames;

namespace Borogove
{
    public class WorkMetadataYamlUnpacker : IModule
    {
        public const string DefaultKeyName = "Borogove";
        public const string TagSetKeySuffix = "TagSet";
        public const string DefaultTagSetKey = DefaultKeyName + TagSetKeySuffix;
        public const char ListSeparator = ',';

        private static readonly char[] _listSeparatorArray = new char[] { ListSeparator };

        private readonly string _key;
        private readonly bool _flatten;
        private readonly IModule[] _modules;
        private Model.TagSet _defaultTagSet = null;

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
            var tagSetKey = _key + TagSetKeySuffix;
            foreach (IDocument document in processedDocuments)
            {
                Dictionary<string, object> processedMetadata = document.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                dynamic borogroveObject = document.Get(_key);
                if (borogroveObject == null)
                {
                    continue;
                }

                if (_defaultTagSet == null)
                {
                    if (processedMetadata.ContainsKey(tagSetKey))
                    {
                        _defaultTagSet = (Model.TagSet)processedMetadata[tagSetKey];
                    }
                    else
                    {
                        _defaultTagSet = new Model.TagSet();
                    }
                }

                if (!processedMetadata.ContainsKey(tagSetKey))
                {
                    processedMetadata[tagSetKey] = _defaultTagSet;
                }

                var currentTagSet = (Model.TagSet)processedMetadata[tagSetKey];
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
                        case DraftOf:
                        case ArtifactOf:
                        case CommentsOn:
                            processedMetadata.Add(canonicalizedKey, Guid.Parse(stringValue));
                            continue;

                        case Previous:
                        case Next:
                            processedMetadata.Add(canonicalizedKey,
                                stringValue
                                    .Split(_listSeparatorArray, StringSplitOptions.RemoveEmptyEntries)
                                    .Select(w => Guid.Parse(w))
                                    .ToList());
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
                                if (string.IsNullOrWhiteSpace(creatorObject.Text))
                                {
                                    creatorObject.Text = "Anonymous";
                                }
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

                        case Tags:
                            processedMetadata.Add(Tags, currentTagSet.ResolveTagList(stringValue, true, true));
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
