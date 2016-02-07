using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.OData;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Borogove.DataAccess;

namespace Borogove.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var builder = new ODataConventionModelBuilder();
            builder.Namespace = "Borogove";

            var workConfiguration = builder.EntitySet<WorkEntity>("Works")
                .EntityType.HasKey(w => w.Identifier);

            var workCreatorsConfiguration = builder.EntitySet<WorkCreatorEntity>("WorkCreators").EntityType;
            workCreatorsConfiguration.HasKey(w => w.WorkIdentifier);
            workCreatorsConfiguration.HasKey(w => w.CreatorName);
            workCreatorsConfiguration.HasKey(w => w.Role);
            workCreatorsConfiguration.HasKey(w => w.WorkedAsName);

            var creatorsConfiguration = builder.EntitySet<CreatorInfoEntity>("Creators")
                .EntityType.HasKey(c => c.Name);
            creatorsConfiguration.Function("GetWorks")
                .ReturnsCollectionFromEntitySet<WorkEntity>("Works");

            var creatorAliasesConfiguration = builder.EntitySet<CreatorAliasEntity>("CreatorAliases")
                .EntityType.HasKey(ca => ca.Alias);
            creatorAliasesConfiguration.Function("GetCreator")
                .ReturnsFromEntitySet<CreatorInfoEntity>("Creators");
            creatorAliasesConfiguration.Function("GetOtherAliases")
                .ReturnsCollectionFromEntitySet<CreatorAliasEntity>("CreatorAliases");
            creatorAliasesConfiguration.Function("GetWorks")
                .ReturnsCollectionFromEntitySet<WorkEntity>("Works");

            builder.EntitySet<LanguageEntity>("Languages")
                .EntityType.HasKey(l => l.Name);

            builder.EntitySet<TagEntity>("Tags")
                .EntityType.HasKey(t => t.TagName);

            builder.EntitySet<TagAliasEntity>("TagAliases")
                .EntityType.HasKey(ta => ta.Alias);

            config.MapODataServiceRoute(
                "ODataRoute",
                null,
                builder.GetEdmModel(),
                defaultHandler: new ODataNullValueMessageHandler() { InnerHandler = new HttpControllerDispatcher(config) });
        }
    }
}
