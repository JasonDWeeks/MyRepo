using System;
using System.Dynamic;

namespace Common.ModelTools.ModelAdaptor.Internal
{
    class MemberBinderGet : GetMemberBinder
    {
        public MemberBinderGet(string name, bool ignoreCase) : base(name, ignoreCase)
        {
        }

        public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
        {
            throw new NotImplementedException();
        }
    }
}
