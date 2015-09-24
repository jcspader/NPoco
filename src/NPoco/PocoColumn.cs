using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NPoco
{
    public class PocoColumn
    {
        public PocoColumn()
        {
            ForceToUtc = true;
            MemberInfoChain = new List<MemberInfo>();
        }
        
        public static string GenerateKey(IEnumerable<MemberInfo> memberInfoChain)
        {
            return string.Join(PocoData.Separator, memberInfoChain.Select(x => x.Name).ToArray());
        }

        public TableInfo TableInfo;
        public string ColumnName;

        public List<MemberInfo> MemberInfoChain { get; set; }
        public string MemberInfoKey { get { return GenerateKey(MemberInfoChain); } }
        public MemberInfo MemberInfo { get; set; }

        public bool ResultColumn;
        public bool VersionColumn;
        public VersionColumnType VersionColumnType;
        public bool ComputedColumn;
        private Type _columnType;
        private MemberAccessor _memberAccessor;
        private List<MemberAccessor> _memberAccessorChain = new List<MemberAccessor>();

        public Type ColumnType
        {
            get { return _columnType ?? MemberInfo.GetMemberInfoType(); }
            set { _columnType = value; }
        }

        public bool ForceToUtc { get; set; }
        public string ColumnAlias { get; set; }

        public ReferenceType ReferenceType { get; set; }
        public bool StoredAsJson { get; set; }

        public void SetMemberAccessors(List<MemberAccessor> memberAccessors)
        {
            _memberAccessor = memberAccessors.Last();
            _memberAccessorChain = memberAccessors;
        }

        public virtual void SetValue(object target, object val)
        {
            _memberAccessor.Set(target, val);
        }

        public virtual object GetValue(object target)
        {
            foreach (var memberAccessor in _memberAccessorChain)
            {
                target = target == null ? null : memberAccessor.Get(target);
            }
            //foreach (var memberInfo in MemberInfoChain)
            //{
            //    target = target == null ? null : memberInfo.GetMemberInfoValue(target);
            //}
            return target;
        }

        public virtual object ChangeType(object val) { return Convert.ChangeType(val, MemberInfo.GetMemberInfoType()); }
    }
}