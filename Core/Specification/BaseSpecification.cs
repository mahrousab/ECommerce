using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specification
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        private readonly Expression<Func<T, bool>>? criteria;
        public BaseSpecification(Expression<Func<T, bool>>? criteria)
        {
            this.criteria = criteria;
        }
        protected BaseSpecification() : this(null) { }
        public Expression<Func<T, bool>>? Criteria => criteria;
        public Expression<Func<T, object>>? OrderBy { get; private set; }
        public Expression<Func<T, object>>? OrderByDescending { get; private set; }
		public bool IsDistinct { get; private set; }

		public int Take { get; private set; }

		public int Skip  { get; private set; }

	public bool IsPagingEnable { get; private set; }

		public List<Expression<Func<T, object>>> Includes { get; }

		public List<string> IncludeStrings { get; }

		protected void AddOrderBy(Expression<Func<T, object>> orderExpression)
        {
            OrderBy = orderExpression;

        }
        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;

        }

		protected void ApplyDistinct()
		{
			IsDistinct = true;
		}

        public void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnable = true;
        }

		public IQueryable<T> ApplyCriteria(IQueryable<T> query)
		{
			if(Criteria != null)
            {
                query = query.Where(Criteria);
            }
            return query;
		}

        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        protected void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }
	}

	public class BaseSpecification<T, TResult> :
        BaseSpecification<T>, ISpecification<T, TResult>
    {
		private readonly Expression<Func<T, bool>> criteria;

		public BaseSpecification(Expression<Func<T,bool>> criteria)
        {
			this.criteria = criteria;
		}
		protected BaseSpecification() : this(null!) { }
		public Expression<Func<T, TResult>>? Select { get; private set; }

        protected void AddSelect(Expression<Func<T,TResult>> selectExpression)
        {
            Select = selectExpression;
        }
        
	}

}



