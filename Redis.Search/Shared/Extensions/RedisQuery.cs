using System;
using System.Collections.Generic;
using System.Linq;

namespace Redis.Search.Features.UseCases.GetFunds.Models
{
    public sealed class RedisQuery
    {
        internal string Index { get; set; }
        public string? QueryText { get; set; }
        public SearchLimit? Limit { get; set; }
        public RedisSortBy? SortBy { get; set; }
        public ReturnFields? Return { get; set; }

        public RedisQuery(string index)
        {
            Index = index;
        }

        public string[] ToArray()
        {
            var ret = new List<string>();
            if (string.IsNullOrEmpty(Index))
            {
                throw new ArgumentException("Index cannot be null");
            }

            ret.Add(Index);
            ret.Add(QueryText ?? "*");

            if (Limit != null)
            {
                ret.AddRange(Limit.SerializeArgs);
            }

            if (Return != null)
            {
                ret.AddRange(Return.SerializeArgs);
            }

            if (SortBy != null)
            {
                ret.AddRange(SortBy.SerializeArgs);
            }

            return ret.ToArray();
        }
    }

    public enum SortDirection
    {
        Ascending,
        Descending,
    }

    public class SearchLimit : QueryOption
    {
        internal int Limit { get; set; }
        internal int Offset { get; set; }

        public SearchLimit(int? limit, int? offset)
        {
            Limit = limit ?? 10;
            Offset = offset ?? 0;
        }

        internal override IEnumerable<string> SerializeArgs
        {
            get
            {
                return new[]
                {
                    "LIMIT",
                    Offset.ToString(),
                    Limit.ToString(),
                };
            }
        }
    }

    public class RedisSortBy : QueryOption
    {
        public string Field { get; set; } = string.Empty;

        public SortDirection Direction { get; set; }

        internal override IEnumerable<string> SerializeArgs
        {
            get
            {
                var dir = Direction == SortDirection.Ascending ? "ASC" : "DESC";
                return new[]
                {
                    "SORTBY",
                    Field,
                    dir,
                };
            }
        }
    }

    public class ReturnFields : QueryOption
    {
        private readonly IEnumerable<string> _fields;

        public ReturnFields(IEnumerable<string>? fields)
        {
            _fields = fields ?? new List<string>();
        }

        internal override IEnumerable<string> SerializeArgs
        {
            get
            {
                var ret = new List<string> { "RETURN", _fields.Count().ToString() };
                foreach (var field in _fields)
                {
                    ret.Add($"{field}");
                }

                return ret.ToArray();
            }
        }
    }

    public abstract class QueryOption
    {
        internal abstract IEnumerable<string> SerializeArgs { get; }
    }
}
