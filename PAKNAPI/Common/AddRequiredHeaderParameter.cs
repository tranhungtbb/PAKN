using NJsonSchema;
using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using System.Threading.Tasks;
namespace PAKNAPI.Common
{
    public class AddRequiredHeaderParameter : IOperationProcessor
    {
        public bool Process(OperationProcessorContext context)
        {

            context.OperationDescription.Operation.Parameters.Add(
                new OpenApiParameter
                {
                    Name = "logAction",
                    Kind = OpenApiParameterKind.Header,
                    Schema = new JsonSchema { Type = JsonObjectType.String },
                    IsRequired = true,
                    Description = "Hành động ghi log",
                    Default = "Insert"
                }
            );

            context.OperationDescription.Operation.Parameters.Add(
                new OpenApiParameter
                {
                    Name = "logObject",
                    Kind = OpenApiParameterKind.Header,
                    Schema = new JsonSchema { Type = JsonObjectType.String },
                    IsRequired = true,
                    Description = "Đối tượng tác động",
                    Default = "Fields Category"
                }
            );
            

            return true;
        }
    }
}
