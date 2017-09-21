/* *********************************************************************
 * Copyright (c) 2017 南京华盾电力信息安全测评有限公司 All Rights Reserved.
 * CLR 版本: 4.0.30319.42000
 * 公司名称: 南京华盾电力信息安全测评有限公司
 * 命名空间: IdentityServer3.Dapper.Serialization
 * 文 件 名: ClientConverter
 * 版 本 号: V1.0.0.0
 * 创 建 人: 管超
 * 电子邮箱: levid.gc@outlook.com
 * 创建时间: 9/20/2017 12:15:55
 * =====================================================================
 * 修改标记 
 * 修改时间: 9/20/2017 12:15:55
 * 修 改 人: 管超
 * 版 本 号: V1.0.0.0
 * 修改描述:
 * ********************************************************************* */

#region Usings

using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using Newtonsoft.Json;
using System;

#endregion

namespace IdentityServer3.Dapper.Serialization
{
    public class ClientLite
    {
        public string ClientId { get; set; }
    }

    public class ClientConverter : JsonConverter
    {
        private readonly IClientStore _clientStore;

        public ClientConverter(IClientStore clientStore)
        {
            _clientStore = clientStore ?? throw new ArgumentNullException("clientStore");
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Client) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var source = serializer.Deserialize<ClientLite>(reader);
            return AsyncHelper.RunSync(async () => await _clientStore.FindClientByIdAsync(source.ClientId));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var source = (Client)value;

            var target = new ClientLite
            {
                ClientId = source.ClientId
            };
            serializer.Serialize(writer, target);
        }
    }
}
