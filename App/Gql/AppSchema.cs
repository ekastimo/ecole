using GraphQL;

namespace App.Gql
{
    public class AppSchema : GraphQL.Types.Schema
    {
        public AppSchema(Query query, IDependencyResolver resolver)
        {
            Query = query;
            DependencyResolver = resolver;
        }
    }
}