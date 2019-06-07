﻿//
//  DossierCommands.cs
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

using System;
using System.Threading.Tasks;

using DIGOS.Ambassador.Database;
using DIGOS.Ambassador.Database.Dossiers;
using DIGOS.Ambassador.Modules.Base;
using DIGOS.Ambassador.Pagination;
using DIGOS.Ambassador.Services;
using DIGOS.Ambassador.Services.Interactivity;
using Discord;
using Discord.Commands;

using JetBrains.Annotations;
using static Discord.Commands.RunMode;

#pragma warning disable SA1615 // Disable "Element return value should be documented" due to TPL tasks

namespace DIGOS.Ambassador.Modules
{
    /// <summary>
    /// Commands for viewing, adding, and editing dossier entries.
    /// </summary>
    [Group("dossier")]
    [Summary("Commands for viewing, adding, and editing dossier entries.")]
    public class DossierCommands : DatabaseModuleBase
    {
        private readonly UserFeedbackService _feedback;
        private readonly ContentService _content;
        private readonly DossierService _dossiers;
        private readonly InteractivityService _interactivity;

        /// <summary>
        /// Initializes a new instance of the <see cref="DossierCommands"/> class.
        /// </summary>
        /// <param name="database">A database context from the context pool.</param>
        /// <param name="feedback">The feedback service.</param>
        /// <param name="content">The content service.</param>
        /// <param name="dossiers">The dossier service.</param>
        /// <param name="interactivity">The interactivity service.</param>
        public DossierCommands
        (
            GlobalInfoContext database,
            UserFeedbackService feedback,
            ContentService content,
            DossierService dossiers,
            InteractivityService interactivity
        )
            : base(database)
        {
            _feedback = feedback;
            _content = content;
            _dossiers = dossiers;
            _interactivity = interactivity;
        }

        /// <summary>
        /// Lists the available dossiers.
        /// </summary>
        [UsedImplicitly]
        [Command("list")]
        [Summary("Lists the available dossiers.")]
        public async Task ListDossiersAsync()
        {
            var appearance = PaginatedAppearanceOptions.Default;
            appearance.Title = "Dossier Database";

            var paginatedEmbed = PaginatedEmbedFactory.SimpleFieldsFromCollection
            (
                _feedback,
                this.Context.User,
                this.Database.Dossiers,
                d => d.Title,
                d => d.Summary ?? "No summary set.",
                "There are no dossiers available.",
                appearance
            );

            await _interactivity.SendInteractiveMessageAndDeleteAsync
            (
                this.Context.Channel,
                paginatedEmbed,
                TimeSpan.FromMinutes(5.0)
            );
        }

        /// <summary>
        /// Views the named dossier.
        /// </summary>
        /// <param name="title">The title of the dossier to view.</param>
        [UsedImplicitly]
        [Alias("view", "show")]
        [Command("view")]
        [Summary("Views the named dossier.")]
        public async Task ViewDossierAsync([NotNull] string title)
        {
            var getDossierResult = await _dossiers.GetDossierByTitleAsync(this.Database, title);
            if (!getDossierResult.IsSuccess)
            {
                await _feedback.SendErrorAsync(this.Context, getDossierResult.ErrorReason);
                return;
            }

            var dossier = getDossierResult.Entity;

            var eb = BuildDossierEmbed(dossier);
            await _feedback.SendEmbedAsync(this.Context.Channel, eb);

            var dossierDataResult = _content.GetDossierStream(dossier);
            if (!dossierDataResult.IsSuccess)
            {
                await _feedback.SendErrorAsync(this.Context, dossierDataResult.ErrorReason);
                return;
            }

            using (var dossierData = dossierDataResult.Entity)
            {
                await this.Context.Channel.SendFileAsync(dossierData, $"{dossier.Title}.pdf");
            }
        }

        [NotNull]
        private Embed BuildDossierEmbed([NotNull] Dossier dossier)
        {
            var eb = _feedback.CreateEmbedBase();
            eb.WithTitle(dossier.Title);
            eb.WithDescription(dossier.Summary);

            return eb.Build();
        }

        /// <summary>
        /// Adds a new dossier with the given title and summary. A PDF with the full dossier can be attached.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="summary">The summary.</param>
        [UsedImplicitly]
        [Alias("add", "create")]
        [Command("add")]
        [Summary("Adds a new dossier with the given title and summary. A PDF with the full dossier can be attached.")]
        [RequireOwner]
        public async Task AddDossierAsync([NotNull] string title, [NotNull] string summary = "No summary set.")
        {
            var dossierCreationResult = await _dossiers.CreateDossierAsync(this.Database, title, summary);
            if (!dossierCreationResult.IsSuccess)
            {
                await _feedback.SendErrorAsync(this.Context, dossierCreationResult.ErrorReason);
                return;
            }

            var dossier = dossierCreationResult.Entity;

            var modifyResult = await _dossiers.SetDossierDataAsync(this.Database, dossier, this.Context);
            if (!modifyResult.IsSuccess)
            {
                if (modifyResult.Error == CommandError.Exception)
                {
                    await _feedback.SendErrorAsync(this.Context, modifyResult.ErrorReason);
                    await _dossiers.DeleteDossierAsync(this.Database, dossier);
                    return;
                }

                await _feedback.SendWarningAsync(this.Context, modifyResult.ErrorReason);
            }

            await _feedback.SendConfirmationAsync(this.Context, $"Dossier \"{dossier.Title}\" added.");
        }

        /// <summary>
        /// Removes the dossier with the given title.
        /// </summary>
        /// <param name="title">The title.</param>
        [UsedImplicitly]
        [Alias("remove", "delete")]
        [Command("remove")]
        [Summary("Removes the dossier with the given title.")]
        [RequireOwner]
        public async Task RemoveDossierAsync([NotNull] string title)
        {
            var getDossierResult = await _dossiers.GetDossierByTitleAsync(this.Database, title);
            if (!getDossierResult.IsSuccess)
            {
                await _feedback.SendErrorAsync(this.Context, getDossierResult.ErrorReason);
                return;
            }

            var dossier = getDossierResult.Entity;
            var deleteDossierResult = await _dossiers.DeleteDossierAsync(this.Database, dossier);
            if (!deleteDossierResult.IsSuccess)
            {
                await _feedback.SendErrorAsync(this.Context, getDossierResult.ErrorReason);
                return;
            }

            await _feedback.SendConfirmationAsync(this.Context, $"Dossier \"{dossier.Title}\" deleted.");
        }

        /// <summary>
        /// Setters for dossier properties.
        /// </summary>
        [Group("set")]
        public class SetCommands : DatabaseModuleBase
        {
            private readonly UserFeedbackService _feedback;
            private readonly DossierService _dossiers;

            /// <summary>
            /// Initializes a new instance of the <see cref="SetCommands"/> class.
            /// </summary>
            /// <param name="database">A database context from the context pool.</param>
            /// <param name="feedback">The feedback service.</param>
            /// <param name="dossiers">The dossier service.</param>
            public SetCommands(GlobalInfoContext database, UserFeedbackService feedback, DossierService dossiers)
                : base(database)
            {
                _feedback = feedback;
                _dossiers = dossiers;
            }

            /// <summary>
            /// Sets the title of the given dossier.
            /// </summary>
            /// <param name="title">The title of the dossier to edit.</param>
            /// <param name="newTitle">The new title of the dossier.</param>
            [UsedImplicitly]
            [Command("title")]
            [Summary("Sets the title of the given dossier.")]
            [RequireOwner]
            public async Task SetTitleAsync([NotNull] string title, [NotNull] string newTitle)
            {
                var getDossierResult = await _dossiers.GetDossierByTitleAsync(this.Database, title);
                if (!getDossierResult.IsSuccess)
                {
                    await _feedback.SendErrorAsync(this.Context, getDossierResult.ErrorReason);
                    return;
                }

                var dossier = getDossierResult.Entity;

                var modifyResult = await _dossiers.SetDossierTitleAsync(this.Database, dossier, newTitle);
                if (!modifyResult.IsSuccess)
                {
                    await _feedback.SendErrorAsync(this.Context, modifyResult.ErrorReason);
                    return;
                }

                await _feedback.SendConfirmationAsync(this.Context, "New dossier title set.");
            }

            /// <summary>
            /// Sets the summary of the given dossier.
            /// </summary>
            /// <param name="title">The title of the dossier to edit.</param>
            /// <param name="newSummary">The new summary of the dossier.</param>
            [UsedImplicitly]
            [Command("summary")]
            [Summary("Sets the summary of the given dossier.")]
            [RequireOwner]
            public async Task SetSummaryAsync([NotNull] string title, [NotNull] string newSummary)
            {
                var getDossierResult = await _dossiers.GetDossierByTitleAsync(this.Database, title);
                if (!getDossierResult.IsSuccess)
                {
                    await _feedback.SendErrorAsync(this.Context, getDossierResult.ErrorReason);
                    return;
                }

                var dossier = getDossierResult.Entity;

                var modifyResult = await _dossiers.SetDossierSummaryAsync(this.Database, dossier, newSummary);
                if (!modifyResult.IsSuccess)
                {
                    await _feedback.SendErrorAsync(this.Context, modifyResult.ErrorReason);
                    return;
                }

                await _feedback.SendConfirmationAsync(this.Context, "New dossier summary set.");
            }

            /// <summary>
            /// Sets the data of the given dossier. Attach a PDF to the command.
            /// </summary>
            /// <param name="title">The title of the dossier to edit.</param>
            [UsedImplicitly]
            [Command("data")]
            [Summary("Sets the data of the given dossier. Attach a PDF to the command.")]
            [RequireOwner]
            public async Task SetFileAsync([NotNull] string title)
            {
                var getDossierResult = await _dossiers.GetDossierByTitleAsync(this.Database, title);
                if (!getDossierResult.IsSuccess)
                {
                    await _feedback.SendErrorAsync(this.Context, getDossierResult.ErrorReason);
                    return;
                }

                var dossier = getDossierResult.Entity;

                var modifyResult = await _dossiers.SetDossierDataAsync(this.Database, dossier, this.Context);
                if (!modifyResult.IsSuccess)
                {
                    await _feedback.SendErrorAsync(this.Context, modifyResult.ErrorReason);
                    return;
                }

                await _feedback.SendConfirmationAsync(this.Context, "Dossier data set.");
            }
        }
    }
}
