using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;

namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;

// A base class that can inject custom evaluators.
public class BaseSpecificationEvaluator : SpecificationEvaluator
{
  /// <inheritdoc />
  public BaseSpecificationEvaluator(bool cacheEnabled = false) : base(cacheEnabled)
  {
  }

  /// <inheritdoc />
  public BaseSpecificationEvaluator(IEnumerable<IEvaluator> evaluators) : base(evaluators)
  {
      // Add new custom evaluators here.
  }
}
