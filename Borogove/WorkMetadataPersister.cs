using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wyam.Common;
using System.Data.Entity;
using Borogove.Model;
using Borogove.DataAccess;
using static Borogove.WorkMetadataCanonicalNames;

namespace Borogove
{
    class WorkMetadataPersister : IModule
    {
        public IEnumerable<IDocument> Execute(IReadOnlyList<IDocument> inputs, IExecutionContext context)
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<WorkContext>());

            var workDictionary = new Dictionary<Guid, Work>();
            var processedDocuments = new List<IDocument>();
            foreach (var document in inputs)
            {
                Dictionary<string, object> metadata = document.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                if (!metadata.ContainsKey(Identifier))
                {
                    metadata.Add(Identifier, Guid.NewGuid());
                }
            }

            using (var workContext = new WorkContext())
            {
                var work = new Work();
                workContext.Works.Add(work);
            }

            return processedDocuments;
        }
    }
}
