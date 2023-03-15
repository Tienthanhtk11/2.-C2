using ECom.Framework.Validator.Exceptions;
using ECom.Framework.Validator.Validators;

namespace ECom.Framework.Validator
{
    public static class ValitRulesExtensions
    {
        public static IValitator<TObject> CreateValitator<TObject>(this IValitRules<TObject> valitRules) where TObject : class
        {
            valitRules.ThrowIfNull();
            return new Valitator<TObject>(valitRules);
        }
    }
}
