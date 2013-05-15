using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Beast.IO
{
    public class PropertyOutput : BasicOutput
    {
        public PropertyOutput(string inputId, string objectId, string name, object value)
            : base(inputId)
        {
            Command = "property";
            Data = new
            {
                ObjectId = objectId, 
                Name = name,
                Value = value
            };
        }

        public static PropertyOutput Create<TObject, TProperty>(string inputId, TObject obj, Expression<Func<TObject, TProperty>> expression) where TObject : IGameObject
        {
            var p = ReflectionExtensions.GetProperty(expression);
            return new PropertyOutput(
                inputId,
                obj.Id,
                p.Name,
                p.GetValue(obj)
            );
        }
    }
}
