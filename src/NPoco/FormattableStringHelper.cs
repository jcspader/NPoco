using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPoco
{
#if !(NET35 || NET40)
    public static class FormattableStringHelper
    {
        public static Sql Build(FormattableString sql)
        {
            var newSql = string.Format(sql.Format, Enumerable.Range(0, sql.ArgumentCount).Select(x => (object)("@" + x)).ToArray());
            return new Sql(newSql, sql.GetArguments());
        }
    }
#endif
}
