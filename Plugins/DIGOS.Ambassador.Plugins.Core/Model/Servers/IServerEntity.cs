﻿//
//  IServerEntity.cs
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

using DIGOS.Ambassador.Core.Database.Entities;
using JetBrains.Annotations;

namespace DIGOS.Ambassador.Plugins.Core.Model.Servers
{
    /// <summary>
    /// Represents an EF entity unique to a server.
    /// </summary>
    [PublicAPI]
    public interface IServerEntity : IEFEntity
    {
        /// <summary>
        /// Gets the Discord server ID.
        /// </summary>
        long ServerID { get; }
    }
}
