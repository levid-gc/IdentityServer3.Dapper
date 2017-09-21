/* *********************************************************************
 * Copyright (c) 2017 南京华盾电力信息安全测评有限公司 All Rights Reserved.
 * CLR 版本: 4.0.30319.42000
 * 公司名称: 南京华盾电力信息安全测评有限公司
 * 命名空间: IdentityServer3.Dapper.Serialization
 * 文 件 名: ScopeConverter
 * 版 本 号: V1.0.0.0
 * 创 建 人: 管超
 * 电子邮箱: levid.gc@outlook.com
 * 创建时间: 9/20/2017 12:19:00
 * =====================================================================
 * 修改标记 
 * 修改时间: 9/20/2017 12:19:00
 * 修 改 人: 管超
 * 版 本 号: V1.0.0.0
 * 修改描述:
 * ********************************************************************* */

#region Usings

using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using Newtonsoft.Json;
using System;
using System.Linq;

#endregion

namespace IdentityServer3.Dapper.Serialization
{
    public class ScopeLite
    {
        public string Name { get; set; }
    }

    public class ScopeConverter : JsonConverter
    {
        private readonly IScopeStore scopeStore;

        public ScopeConverter(IScopeStore scopeStore)
        {
            this.scopeStore = scopeStore ?? throw new ArgumentNullException("scopeStore");
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Scope) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var source = serializer.Deserialize<ScopeLite>(reader);
            var scopes = AsyncHelper.RunSync(async () => await scopeStore.FindScopesAsync(new string[] { source.Name }));
            return scopes.Single();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var source = (Scope)value;

            var target = new ScopeLite
            {
                Name = source.Name
            };
            serializer.Serialize(writer, target);
        }
    }
}
