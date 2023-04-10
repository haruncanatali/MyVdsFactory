using System.Collections;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace MyVdsFactory.Application.Common.Extensions;

public static class ListExtensions
{
    public async static Task<List<T>> PagingListAsync<T>(this IQueryable<T> queryableEntities, 
        int page, int pageSize,CancellationToken token)
    {
        if (page == 0 || pageSize == 0)
        {
            return await queryableEntities.ToListAsync(token);
        }
        
        return await queryableEntities
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(token);
    }
    
    public static IQueryable<T> AsAsyncQueryable<T>(this IEnumerable<T> source) =>
        new AsyncQueryable<T>(source.AsQueryable());
    
    internal class AsyncQueryable<T> : IAsyncEnumerable<T>, IQueryable<T>
    {
        private readonly IQueryable<T> Source;

        public AsyncQueryable(IQueryable<T> source)
        {
            Source = source;
        }

        public Type ElementType => typeof(T);

        public Expression Expression => Source.Expression;

        public IQueryProvider Provider => new AsyncQueryProvider<T>(Source.Provider);

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new AsyncEnumeratorWrapper<T>(Source.GetEnumerator());
        }

        public IEnumerator<T> GetEnumerator() => Source.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal class AsyncQueryProvider<T> : IQueryProvider
    {
        private readonly IQueryProvider Source;

        public AsyncQueryProvider(IQueryProvider source)
        {
            Source = source;
        }

        public IQueryable CreateQuery(Expression expression) =>
            Source.CreateQuery(expression);

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression) =>
            new AsyncQueryable<TElement>(Source.CreateQuery<TElement>(expression));

        public object? Execute(Expression expression) => Execute<T>(expression);

        public TResult Execute<TResult>(Expression expression) =>
            Source.Execute<TResult>(expression);
    }



    internal class AsyncEnumeratorWrapper<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> Source;

        public AsyncEnumeratorWrapper(IEnumerator<T> source)
        {
            Source = source;
        }

        public T Current => Source.Current;

        public ValueTask DisposeAsync()
        {
            return new ValueTask(Task.CompletedTask);
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(Source.MoveNext());
        }
    }
    
    public static void Shuffle<T>(this IList<T> list, Random rnd)
    {
        for(var i=list.Count; i > 0; i--)
            list.Swap(0, rnd.Next(0, i));
    }

    private static void Swap<T>(this IList<T> list, int i, int j)
    {
        (list[i], list[j]) = (list[j], list[i]);
    }
}