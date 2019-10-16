﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Wabbajack.Common;
using Wabbajack.Lib.Validation;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Game = Wabbajack.Common.Game;

namespace Wabbajack.Lib.ModListRegistry
{
    public class ModlistMetadata
    {
        /// <summary>
        /// Name of the modlist
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name of the author of the modlist
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Game this modlist is for
        /// </summary>
        public Game Game { get; set; }

        /// <summary>
        /// Short description of the modlist
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// URL of the logo for the modlist
        /// </summary>
        public string LogoUrl { get; set; }

        /// <summary>
        /// Download URL
        /// </summary>
        public string DownloadUrl { get; set; }

        public static List<ModlistMetadata> LoadFromGithub()
        {
            var d = new DeserializerBuilder()
                .WithNamingConvention(PascalCaseNamingConvention.Instance)
                .Build();
            var client = new HttpClient();
            Utils.Log("Loading Modlists from Github");
            using (var result = new StringReader(client.GetStringSync(Consts.ModlistMetadataURL)))
            {
                return d.Deserialize<List<ModlistMetadata>>(result);
            }
        }
    }
}
