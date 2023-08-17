/* =============================================
 * Copyright 2022 TVU Networks Co.,Ltd. All rights reserved.
 * For internal members in TVU Networks only.
 * FileName: TVUCloudService.cs
 * Purpose:
 * Author:   YulongLu added on 2.8th, 2022.
 * Since:    Microsoft Visual Studio 2019
 * =============================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using Nett;

namespace tomlSerilizer
{
    public class TVUCloudServiceList : ICloneable
    {
        public AWSAccessInfo AWS { get; set; }

        private List<AWSAccessInfo> _configs = new List<AWSAccessInfo>();
        public List<AWSAccessInfo> Configs { get => _configs; set => _configs = value ?? new List<AWSAccessInfo>(); }

        public void Adjust()
        {
            if (AWS != null)
            {
                if (_configs.Any() == false)
                {
                    _configs.Add(AWS);
                }
                else
                {
                    if (!_configs.Any(i => i.S3?.Key == AWS.S3?.Key))
                    {
                        _configs.Add(AWS);
                    }
                }
            }
        }

        public AWSAccessInfo Update(AWSAccessInfo accessInfo)
        {
            AWSAccessInfo result = null;
            if (accessInfo != null)
            {
                if (!_configs.Any(i => i.S3?.Key == accessInfo.S3?.Key))
                {
                    _configs.Add(accessInfo);
                    result = accessInfo;
                }
            }
            return result;
        }

        public object Clone()
        {
            return new TVUCloudServiceList()
            {
                AWS = new AWSAccessInfo()
                {
                    AccessKey = this.AWS?.AccessKey,
                    SecretKey = this.AWS?.SecretKey,
                    S3 = new AWSS3()
                    {
                        BucketName = this.AWS?.S3?.BucketName,
                        EndPoint = this.AWS?.S3?.EndPoint
                    }
                }
            };
        }
    }

    public class AWSAccessInfo
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public AWSS3 S3 { get; set; } = new AWSS3();
    }

    public class AWSS3
    {
        [TomlIgnore]
        public string Key => $"{EndPoint}-{BucketName}";
        public string EndPoint { get; set; }
        public string BucketName { get; set; }
        public string Domain { get; set; }
        public string Url { get; set; }
    }
}