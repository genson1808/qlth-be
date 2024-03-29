using System.Text;
using Dapper;

namespace QLTH.Common.Utils;

public static class Utils
{
    public static DynamicParameters EntityAllDynamicParams<T>(T entity)
    {
        var properties = typeof(T).GetProperties();
        var parameters = new DynamicParameters();
        foreach (var property in properties)
        {
            if (property.Name is "Department" or "Subjects" or "Rooms" or "DepartmentName")
            {
                continue;
            }

            string propertyName = $"v_{property}";
            object? propertyValue = null;
            var value = property.GetValue(entity);
            var type = value?.GetType();

            if (value != null && type == typeof(string) && !value.Equals(""))
            {
                propertyValue = value;
            }
            else if (value != null && type == typeof(Guid) && !value.Equals(Guid.Empty))
            {
                propertyValue = value;
            }
            else if (value != null && type != typeof(string) && type != typeof(Guid))
            {
                propertyValue = value;
            }

            parameters.Add(propertyName, propertyValue);
        }

        return parameters;
    }

    public static string MappingWhere(Dictionary<string, string> filters, string? prefix)
    {
        var whereClauses = new StringBuilder();

        if (filters.Count == 0 || filters == null)
        {
            whereClauses.Append("1 = 1");
        }
        else
        {
            foreach (var filter in filters)
            {
                if (filter.Equals(filters.ElementAt(0)))
                {
                    if (!string.IsNullOrEmpty(prefix))
                    {
                        whereClauses.Append($" {prefix}.{filter.Key} LIKE \"%{filter.Value}%\"");
                    }
                    else
                    {
                        whereClauses.Append($" {filter.Key} LIKE \"%{filter.Value}%\"");
                    }

                    continue;
                }

                if (!string.IsNullOrEmpty(prefix))
                {
                    var q = $" AND {prefix}.{filter.Key} LIKE \"%{filter.Value}%\"";
                    whereClauses.Append(q);
                }
                else
                {
                    var q = $" AND {filter.Key} LIKE \"%{filter.Value}%\"";
                    whereClauses.Append(q);
                }
            }
        }

        return whereClauses.ToString();
    }

    public static string MappingOrder(Dictionary<string, string> sorts, string? prefix)
    {
        var orderClauses = new StringBuilder();

        if (sorts.Count == 0 || sorts == null)
        {
            if (!string.IsNullOrEmpty(prefix))
            {
                orderClauses.Append($"{prefix}.CreatedDate DESC");
            }
            else
            {
                orderClauses.Append("CreatedDate DESC");
            }
        }
        else
        {
            foreach (var sort in sorts)
            {
                if (!string.IsNullOrEmpty(prefix))
                {
                    var q = $"{prefix}.{sort.Key} {sort.Value}";
                    orderClauses.Append(q);
                }
                else
                {
                    var q = $"{sort.Key} {sort.Value}";
                    orderClauses.Append(q);
                    ;
                }

                if (sort.Equals(sorts.ElementAt(sorts.Count - 1)))
                {
                    continue;
                }

                orderClauses.Append(" , ");
            }
        }

        return orderClauses.ToString();
    }

    public static string BuildValuesFromGuidList(List<Guid>? guids, Guid relationshipKey)
    {
        var rs = new StringBuilder();

        if (guids != null && guids.Count > 0 && relationshipKey != Guid.Empty)
        {
            for (int i = 0; i < guids.Count ; i++)
            {
                rs.Append($"('{guids[i].ToString()}','{relationshipKey}')");
                if (i != guids.Count - 1)
                {
                    rs.Append(",");
                } 
            }
        }

        return rs.ToString();
    }
}