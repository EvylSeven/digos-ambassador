﻿//
//  MockHelper.cs
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

using Discord;
using JetBrains.Annotations;
using Moq;

namespace DIGOS.Ambassador.Tests.Utility
{
    /// <summary>
    /// Helper methods for mocking objects.
    /// </summary>
    [PublicAPI]
    public static class MockHelper
    {
        /// <summary>
        /// Creates a simple mocked <see cref="IUser"/> object with the given ID.
        /// </summary>
        /// <param name="id">The ID of the object.</param>
        /// <returns>A mocked object.</returns>
        [NotNull]
        public static IUser CreateDiscordUser(long id)
        {
            var mock = new Mock<IUser>();
            mock.Setup(u => u.Id).Returns((ulong)id);

            return mock.Object;
        }

        /// <summary>
        /// Creates a simple mocked <see cref="IGuildUser"/> object with the given ID.
        /// </summary>
        /// <param name="id">The ID of the object.</param>
        /// <returns>A mocked object.</returns>
        [NotNull]
        public static IGuildUser CreateDiscordGuildUser(long id)
        {
            var mock = new Mock<IGuildUser>();
            mock.Setup(u => u.Id).Returns((ulong)id);

            return mock.Object;
        }

        /// <summary>
        /// Creates a simple mocked <see cref="IGuild"/> object with the given ID.
        /// </summary>
        /// <param name="id">The ID of the object.</param>
        /// <param name="ownerId">The ID of the guild owner.</param>
        /// <returns>A mocked object.</returns>
        [NotNull]
        public static IGuild CreateDiscordGuild(long id, [CanBeNull] long? ownerId = null)
        {
            var mock = new Mock<IGuild>();
            mock.Setup(u => u.Id).Returns((ulong)id);

            if (!(ownerId is null))
            {
                mock.Setup(u => u.OwnerId).Returns((ulong)ownerId.Value);
            }

            return mock.Object;
        }

        /// <summary>
        /// Creates a simple mocked <see cref="IRole"/> object with the given ID.
        /// </summary>
        /// <param name="id">The ID of the object.</param>
        /// <param name="guild">The guild of the role.</param>
        /// <returns>A mocked object.</returns>
        [NotNull]
        public static IRole CreateDiscordRole(int id, [CanBeNull] IGuild guild = null)
        {
            var mock = new Mock<IRole>();
            mock.Setup(r => r.Id).Returns((ulong)id);

            if (!(guild is null))
            {
                mock.Setup(r => r.Guild).Returns(guild);
            }

            return mock.Object;
        }
    }
}
