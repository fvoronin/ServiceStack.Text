using System;

namespace ServiceStack.Text.Common
{
#if NETCF
    internal abstract class DelegateCreator<TS> where TS : ITypeSerializer
    {
        protected abstract Delegate GetDelegate();

        public static Delegate ParseDictionaryDelegate(Type[] argTypes)
        {
            var typeDefinition = typeof(ParseDictionaryDelegateCreator<,,>).GetGenericTypeDefinition();
            var genericType = typeDefinition.MakeGenericType(typeof(TS), argTypes[0], argTypes[1]);
            var creator = (DelegateCreator<TS>)Activator.CreateInstance(genericType);

            return creator.GetDelegate();
        }
    }

    internal class ParseDictionaryDelegateCreator<TS, TKey, TValue> : DelegateCreator<TS> where TS : ITypeSerializer
    {
        protected override Delegate GetDelegate()
        {
            return (DeserializeDictionary<TS>.ParseDictionaryDelegate)DeserializeDictionary<TS>.ParseDictionary<TKey, TValue>;
        }
    }
#endif
}