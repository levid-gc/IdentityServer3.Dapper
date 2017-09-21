/* *********************************************************************
 * Copyright (c) 2017 南京华盾电力信息安全测评有限公司 All Rights Reserved.
 * CLR 版本: 4.0.30319.42000
 * 公司名称: 南京华盾电力信息安全测评有限公司
 * 命名空间: IdentityServer3.Dapper.Serialization
 * 文 件 名: ClaimsPrincipalConverter
 * 版 本 号: V1.0.0.0
 * 创 建 人: 管超
 * 电子邮箱: levid.gc@outlook.com
 * 创建时间: 9/20/2017 11:26:04
 * =====================================================================
 * 修改标记 
 * 修改时间: 9/20/2017 11:26:04
 * 修 改 人: 管超
 * 版 本 号: V1.0.0.0
 * 修改描述:
 * ********************************************************************* */

#region Usings

using IdentityServer3.Core;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Claims;

#endregion

namespace IdentityServer3.Dapper.Serialization
{
    public class ClaimsPrincipalLite
    {
        public string AuthenticationType { get; set; }
        public ClaimLite[] Claims { get; set; }
    }

    public class ClaimLite
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public class ClaimsPrincipalConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(ClaimsPrincipal) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var source = serializer.Deserialize<ClaimsPrincipalLite>(reader);
            if (source == null) return null;

            var claims = source.Claims.Select(x => new Claim(x.Type, x.Value));
            var id = new ClaimsIdentity(claims, source.AuthenticationType, Constants.ClaimTypes.Name, Constants.ClaimTypes.Role);
            var target = new ClaimsPrincipal(id);
            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var source = (ClaimsPrincipal)value;

            var target = new ClaimsPrincipalLite
            {
                AuthenticationType = source.Identity.AuthenticationType,
                Claims = source.Claims.Select(x => new ClaimLite { Type = x.Type, Value = x.Value }).ToArray()
            };
            serializer.Serialize(writer, target);
        }
    }
}
