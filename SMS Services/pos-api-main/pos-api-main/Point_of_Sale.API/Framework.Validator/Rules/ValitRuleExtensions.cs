using System.Collections.Generic;
using System.Linq;
using ECom.Framework.Validator.Exceptions;
using ECom.Framework.Validator.Result;

namespace ECom.Framework.Validator.Rules
{
    internal static  class ValitRuleExtensions
    {
        internal static IValitResult ValidateRules<TObject>(this IEnumerable<IValitRule<TObject>> rules, IValitStrategy strategy, TObject @object) where TObject : class
        {
            rules.ThrowIfNull();

            var result = ValitResult.Success;

            foreach(var rule in rules.ToList())
            {
                result &= rule.Validate(@object);

                if(!result.Succeeded)
                {
                    strategy.Fail(rule, result, out bool cancel);
                    if(cancel)
                    {
                        break;
                    }
                }
            }
            
            strategy.Done(result);

            return result;
        }
    }
}