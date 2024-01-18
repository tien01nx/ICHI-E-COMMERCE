using ICHI_CORE.Entities;

namespace ICHI_CORE.Model
{
    public class KeyValueGenericModel<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
    }
}
