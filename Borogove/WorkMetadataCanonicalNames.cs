namespace Borogove
{
    public static class WorkMetadataCanonicalNames
    {
        public const string Identifier = "identifier";
        public const string Title = "title";
        public const string Description = "description";
        public const string Creator = "creator";
        public const string CreatedDate = "createddate";
        public const string ModifiedDate = "modifieddate";
        public const string PublishedDate = "publisheddate";
        public const string Rights = "rights";
        public const string License = "license";
        public const string Language = "language";
        public const string WorkType = "worktype";
        public const string ContentRating = "contentrating";
        public const string ContentDescriptors = "contentdescriptors";
        public const string Tags = "tags";
        public const string Parent = "parent";
        public const string Previous = "previous";
        public const string Next = "next";
        public const string DraftIdentifier = "draftidentifier";
        public const string DraftOf = "draftof";
        public const string ArtifactOf = "artifactof";
        public const string CommentsOn = "commentson";

        public static string CanonicalizeString(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return input.ToLowerInvariant().Replace(" ", string.Empty);
        }
    }
}
