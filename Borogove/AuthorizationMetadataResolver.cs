using System;
using System.Collections.Generic;
using System.Linq;
using Auth0.ManagementApi;
using Wyam.Common.Documents;
using Wyam.Common.Execution;
using Wyam.Common.Modules;

using Borogove.Exceptions;

using static Borogove.AuthorizationMetadataNames;
using static Borogove.MetadataHelpers;

namespace Borogove
{
    public class AuthorizationMetadataResolver : IModule
    {
        private readonly Uri _domain;
        private readonly string _managementToken;
        private readonly bool _takeFirst;
        private readonly bool _breakOnUnesolvedUser;

        public AuthorizationMetadataResolver(string domain, string managementToken, bool takeFirst = true, bool breakOnUnresolvedUser = true)
        {
            if (string.IsNullOrEmpty(domain))
            {
                throw new ArgumentNullException(nameof(domain));
            }

            if (string.IsNullOrEmpty(managementToken))
            {
                throw new ArgumentNullException(nameof(managementToken));
            }

            _domain = new Uri($"https://{domain}/api/v2");
            _managementToken = managementToken;
            _takeFirst = takeFirst;
            _breakOnUnesolvedUser = breakOnUnresolvedUser;
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

            var managementClient = new ManagementApiClient(_managementToken, _domain);
            foreach (IDocument document in inputs)
            {
                var newMetadata = new Dictionary<string, object>();
                foreach (var metadataKeyValuePair in document)
                {
                    string canonicalizedKey = CanonicalizeString(metadataKeyValuePair.Key);
                    string stringValue = metadataKeyValuePair.Value as string;
                    IEnumerable<string> splitString = stringValue.SeparateList();
                    if (stringValue != null)
                    {
                        switch (canonicalizedKey)
                        {
                            case WhitelistedUsers:
                                newMetadata.Add(
                                    WhitelistedUsers,
                                    splitString
                                        .Select(s => FindUserId(s, managementClient))
                                        .Where(s => s != null).ToList());
                                break;

                            case WhitelistedGroups:
                                newMetadata.Add(WhitelistedGroups, splitString.ToList());
                                break;

                            default:
                                // Leave other metadata alone
                                break;
                        }
                    }
                    // If we failed to get a string, it may have matched our keys, but we can't process it, so do nothing.
                }

                yield return context.GetDocument(document, newMetadata);
            }
        }

        private string FindUserId(string searchTerm, ManagementApiClient client)
        {
            try
            {
                var queryString = $"user_id:\"{searchTerm}\"^32 email:\"{searchTerm}\"^16 username:\"{searchTerm}\"^8 nickname:\"{searchTerm}\"^4 name:\"{searchTerm}\"^2";
                var matchingUsers = client.Users.GetAllAsync(fields: "user_id", q: queryString, searchEngine: "v2").Result;

                string result = null;
                if (matchingUsers == null || matchingUsers.Count == 0)
                {
                    if (_breakOnUnesolvedUser)
                    {
                        throw new UnableToResolveUserException($"No users were returned for the search term '{searchTerm}'");
                    }
                }
                else if (matchingUsers.Count > 1 && !_takeFirst)
                {
                    throw new UnableToResolveUserException($"Multiple users where returned for the search term '{searchTerm}'");
                }
                else
                {
                    result = matchingUsers.First().UserId;
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
