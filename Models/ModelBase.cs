using System.Linq;

namespace ModelClient
{
    public abstract class ModelBase
    {
        public override string ToString()
        {
            var props = GetType()?.GetProperties()?.Select(p => $"{p.Name} = {p.GetValue(this)}");
            return (props?.Count() ?? 0) > 0 ? string.Join("; ", props) : base.ToString();
        }
    }
}
