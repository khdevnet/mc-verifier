//using Json.Schema;
//using NJsonSchema.Validation;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Core
//{
//    public static class MessageSchemaValidator
//    {
//        public static async Task<IReadOnlyCollection<MessageSchemaValidationError>> Validate(MessageSchema schema, Type type)
//        {
//            var messageSchema = MessageSchema.Create(type);
//            var sampleData = AutoDataGenerator.GetFromJsonSchema(messageSchema.Json);
//            var errors = schema.Validate(sampleData);

//            return errors.Select(e => ToError(e)).ToReadOnly();
//        }

//        private static MessageSchemaValidationError ToError(ValidationError e)
//        {
//            return new MessageSchemaValidationError(e.Kind.ToString(), e.Property, e.Path, e.LineNumber, e.LinePosition);
//        }
//    }
//}
