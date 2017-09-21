/* *********************************************************************
 * Copyright (c) 2017 南京华盾电力信息安全测评有限公司 All Rights Reserved.
 * CLR 版本: 4.0.30319.42000
 * 公司名称: 南京华盾电力信息安全测评有限公司
 * 命名空间: IdentityServer3.Dapper
 * 文 件 名: DapperServiceOptions
 * 版 本 号: V1.0.0.0
 * 创 建 人: 管超
 * 电子邮箱: levid.gc@outlook.com
 * 创建时间: 9/19/2017 11:19:20
 * =====================================================================
 * 修改标记 
 * 修改时间: 9/19/2017 11:19:20
 * 修 改 人: 管超
 * 版 本 号: V1.0.0.0
 * 修改描述:
 * ********************************************************************* */

#region Usings

using System;
using System.Data;

#endregion

namespace IdentityServer3.Dapper
{
    public class DapperServiceOptions
    {
        public DapperServiceOptions(IDbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            this.Connection = connection;
        }

        public IDbConnection Connection { get; set; }

        public string Schema { get; set; }

        public bool SynchronousReads { get; set; }
    }
}
