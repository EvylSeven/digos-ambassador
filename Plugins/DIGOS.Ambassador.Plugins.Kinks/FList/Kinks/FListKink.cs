﻿//
//  FListKink.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2017 Jarl Gullberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Affero General Public License for more details.
//
//  You should have received a copy of the GNU Affero General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using Newtonsoft.Json;

namespace DIGOS.Ambassador.Plugins.Kinks.FList.Kinks
{
    /// <summary>
    /// Represents a JSON kink from the F-list API.
    /// </summary>
    internal class FListKink
    {
        /// <summary>
        /// Gets or sets the description of the kink.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the ID of the kink.
        /// </summary>
        [JsonProperty("kink_id")]
        public uint KinkId { get; set; }

        /// <summary>
        /// Gets or sets the name of the kink.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
