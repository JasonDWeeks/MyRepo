using System;
using System.Dynamic;

namespace Common.ModelTools.ModelAdaptor.Internal
{
    class MemberBinderSet : SetMemberBinder
    {
        public MemberBinderSet(string name, bool ignoreCase) : base(name, ignoreCase)
        {
        }

        public override DynamicMetaObject FallbackSetMember(DynamicMetaObject target, DynamicMetaObject value, DynamicMetaObject errorSuggestion)
        {
            throw new NotImplementedException();
        }
    }
}
