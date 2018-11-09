using GraphQL.Types;

namespace App.Areas.Doc.Gql
{
    public class DocType : ObjectGraphType<Models.Doc>
    {
        public DocType()
        {
            Field(c => c.Id, type: typeof(IdGraphType));
            Field(c => c.CreatedAt);
            Field(c => c.IsDeleted);

            Field(c => c.Description);
            Field(c => c.OriginalFileName);
            Field(c => c.FileName);

            Field(c => c.SizeInMbs);
            Field(c => c.ContentType);
            Field(c => c.IsPrimary);
            Field(c => c.CreatedBy, type: typeof(IdGraphType));
        }
    }
}