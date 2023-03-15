using System.Collections.Immutable;

namespace ECom.Framework.Validator
{
    public interface IValitResult
    {
        bool Succeeded { get; }

        ImmutableArray<string> ErrorMessages { get; }

        ImmutableArray<int> ErrorCodes { get; }
    }
}
