using System.Collections.Generic;
using System.Data;

namespace userservice.helper
{
    public static class DbHelper
    {
        public static List<Dictionary<string, object>> ToJson(DataTable table)
        {
            var list = new List<Dictionary<string, object>>();

            if (table == null || table.Rows.Count == 0)
                return list;

            foreach (DataRow row in table.Rows)
            {
                var dict = new Dictionary<string, object>();

                foreach (DataColumn col in table.Columns)
                {
                    dict[col.ColumnName] =
                        row[col] == DBNull.Value ? null : row[col];
                }

                list.Add(dict);
            }

            return list;
        }

    }
}
