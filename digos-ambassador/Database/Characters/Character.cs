﻿//
//  Character.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2017 Jarl Gullberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using DIGOS.Ambassador.Database.Appearances;
using DIGOS.Ambassador.Database.Interfaces;
using DIGOS.Ambassador.Database.Users;

using Discord;

namespace DIGOS.Ambassador.Database.Characters
{
	/// <summary>
	/// Represents a user's character.
	/// </summary>
	public class Character : IOwnedNamedEntity
	{
		/// <summary>
		/// Gets or sets the character's unique key.
		/// </summary>
		public uint CharacterID { get; set; }

		/// <inheritdoc />
		public User Owner { get; set; }

		/// <inheritdoc />
		public string Name { get; set; }

		/// <inheritdoc />
		public string EntityTypeDisplayName => nameof(Character);

		/// <summary>
		/// Gets or sets a URL pointing to the character's avatar.
		/// </summary>
		public string AvatarUrl { get; set; }

		/// <summary>
		/// Gets or sets the nickname that a user should have when playing as the character.
		/// </summary>
		public string Nickname { get; set; }

		/// <summary>
		/// Gets or sets the character summary.
		/// </summary>
		public string Summary { get; set; }

		/// <summary>
		/// Gets or sets the full description of the character.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the character's default appearance.
		/// </summary>
		public Appearance DefaultAppearance { get; set; }

		/// <summary>
		/// Gets or sets the character's transformed appearance.
		/// </summary>
		public Appearance TransformedAppearance { get; set; }

		/// <inheritdoc />
		public bool IsOwner(User user)
		{
			return IsOwner(user.DiscordID);
		}

		/// <inheritdoc />
		public bool IsOwner(IUser user)
		{
			return IsOwner(user.Id);
		}

		/// <inheritdoc />
		public bool IsOwner(ulong userID)
		{
			return this.Owner.DiscordID == userID;
		}
	}
}
