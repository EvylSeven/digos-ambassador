//
//  ShiftBodypartAction.cs
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

namespace DIGOS.Ambassador.Core.Results
{
    /// <summary>
    /// Enumerates the actions that can be taken on a bodypart during a shift.
    /// </summary>
    public enum ShiftBodypartAction
    {
        /// <summary>
        /// The bodypart was added.
        /// </summary>
        Add,

        /// <summary>
        /// The bodypart was removed.
        /// </summary>
        Remove,

        /// <summary>
        /// The bodypart was shifted.
        /// </summary>
        Shift,

        /// <summary>
        /// Nothing was done to the bodypart.
        /// </summary>
        Nothing
    }
}