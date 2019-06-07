﻿//
//  OwnedEntityServiceTests.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DIGOS.Ambassador.Database.Characters;
using DIGOS.Ambassador.Database.Interfaces;
using DIGOS.Ambassador.Database.Users;
using DIGOS.Ambassador.Modules;
using DIGOS.Ambassador.Services;
using DIGOS.Ambassador.Services.Interactivity;
using DIGOS.Ambassador.Tests.TestBases;
using DIGOS.Ambassador.TypeReaders;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using MockQueryable.Moq;
using Moq;
using Xunit;

#pragma warning disable SA1600
#pragma warning disable CS1591

namespace DIGOS.Ambassador.Tests.ServiceTests
{
    public class OwnedEntityServiceTests
    {
        public class IsEntityNameUniqueForUserAsync : OwnedEntityServiceTestBase
        {
            [Fact]
            public async Task ReturnsTrueForEmptySet()
            {
                var entityMock = new Mock<IOwnedNamedEntity>();
                entityMock.Setup(e => e.Name).Returns("Test");

                var queryable = new List<IOwnedNamedEntity>().AsQueryable().BuildMock().Object;

                var result = await this.Entities.IsEntityNameUniqueForUserAsync(queryable, "Test2");

                Assert.True(result);
            }

            [Fact]
            public async Task ReturnsTrueForUniqueName()
            {
                var entityMock = new Mock<IOwnedNamedEntity>();
                entityMock.Setup(e => e.Name).Returns("Test");

                var collection = new List<IOwnedNamedEntity> { entityMock.Object };
                var queryable = collection.AsQueryable().BuildMock().Object;

                var result = await this.Entities.IsEntityNameUniqueForUserAsync(queryable, "Test2");

                Assert.True(result);
            }

            [Fact]
            public async Task ReturnsFalseForNonUniqueName()
            {
                var entityMock = new Mock<IOwnedNamedEntity>();
                entityMock.Setup(e => e.Name).Returns("Test");

                var collection = new List<IOwnedNamedEntity> { entityMock.Object };
                var queryable = collection.AsQueryable().BuildMock().Object;

                var result = await this.Entities.IsEntityNameUniqueForUserAsync(queryable, "Test");

                Assert.False(result);
            }

            [Fact]
            public async Task ReturnsFalseForNonUniqueNameAndIsCaseInsensitive()
            {
                var entityMock = new Mock<IOwnedNamedEntity>();
                entityMock.Setup(e => e.Name).Returns("Test");

                var collection = new List<IOwnedNamedEntity> { entityMock.Object };
                var queryable = collection.AsQueryable().BuildMock().Object;

                var result = await this.Entities.IsEntityNameUniqueForUserAsync(queryable, "TEST");

                Assert.False(result);
            }
        }

        public class TransferEntityOwnershipAsync : OwnedEntityServiceTestBase, IAsyncLifetime
        {
            private readonly IUser _originalUser;
            private readonly IUser _newUser;

            private User _originalDBUser;
            private User _newDBUser;

            public TransferEntityOwnershipAsync()
            {
                // Set up mocked discord users
                var originalUserMock = new Mock<IUser>();
                originalUserMock.Setup(u => u.Id).Returns(0);

                var newUserMock = new Mock<IUser>();
                newUserMock.Setup(u => u.Id).Returns(1);

                _originalUser = originalUserMock.Object;
                _newUser = newUserMock.Object;
            }

            public async Task InitializeAsync()
            {
                // Set up mocked datbase users
                _originalDBUser = await this.Database.AddUserAsync(_originalUser);
                _newDBUser = await this.Database.AddUserAsync(_newUser);

                await this.Database.SaveChangesAsync();
            }

            [Fact]
            public async Task ReturnsErrorIfUserAlreadyOwnsTheEntity()
            {
                // Set up entity owned by the original user
                var entityMock = new Mock<IOwnedNamedEntity>();
                entityMock.Setup(e => e.Name).Returns("Test");
                entityMock.Setup(e => e.Owner).Returns(_originalDBUser);
                entityMock.Setup
                (
                    e =>
                        e.IsOwner(It.IsAny<IUser>())
                )
                .Returns<IUser>
                (
                    u =>
                        u.Id == (ulong)entityMock.Object.Owner.DiscordID
                );

                // Set up the list of entities owned by the new owner
                var collection = new List<IOwnedNamedEntity> { entityMock.Object };
                var ownerEntities = collection.AsQueryable().BuildMock().Object;

                var result = await this.Entities.TransferEntityOwnershipAsync(this.Database, _originalUser, ownerEntities, entityMock.Object);
                Assert.False(result.IsSuccess);
                Assert.Equal(CommandError.Unsuccessful, result.Error);
            }

            [Fact]
            public async Task ReturnsErrorIfUserAlreadyOwnsAnEntityWithTheSameName()
            {
                // Set up the entities owned by the users
                var entityOwnedByOriginal = new Mock<IOwnedNamedEntity>();
                entityOwnedByOriginal.Setup(e => e.Name).Returns("Test");
                entityOwnedByOriginal.Setup(e => e.Owner).Returns(_originalDBUser);

                var entityOwnedByNew = new Mock<IOwnedNamedEntity>();
                entityOwnedByNew.Setup(e => e.Name).Returns("Test");
                entityOwnedByNew.Setup(e => e.Owner).Returns(_newDBUser);

                // Set up the list of entities owned by the new owner
                var collection = new List<IOwnedNamedEntity> { entityOwnedByNew.Object };
                var ownerEntities = collection.AsQueryable().BuildMock().Object;

                var result = await this.Entities.TransferEntityOwnershipAsync(this.Database, _newUser, ownerEntities, entityOwnedByOriginal.Object);
                Assert.False(result.IsSuccess);
                Assert.Equal(CommandError.MultipleMatches, result.Error);
            }

            [Fact]
            public async Task IsSuccessfulIfUserDoesNotOwnTheEntityAndDoesNotOwnAnEntityWithTheSameName()
            {
                // Set up the entities owned by the users
                var entityOwnedByOriginalMock = new Mock<IOwnedNamedEntity>();
                entityOwnedByOriginalMock.Setup(e => e.Name).Returns("Test");
                entityOwnedByOriginalMock.Setup(e => e.Owner).Returns(_originalDBUser);
                entityOwnedByOriginalMock.SetupProperty(e => e.Owner);

                var entityOwnedByNewMock = new Mock<IOwnedNamedEntity>();
                entityOwnedByNewMock.Setup(e => e.Name).Returns("Test2");
                entityOwnedByNewMock.Setup(e => e.Owner).Returns(_newDBUser);

                var entityOwnedByOriginal = entityOwnedByOriginalMock.Object;
                var entityOwnedByNew = entityOwnedByNewMock.Object;

                // Set up the list of entities owned by the new owner
                var collection = new List<IOwnedNamedEntity> { entityOwnedByNew };
                var ownerEntities = collection.AsQueryable().BuildMock().Object;

                var result = await this.Entities.TransferEntityOwnershipAsync(this.Database, _newUser, ownerEntities, entityOwnedByOriginal);
                Assert.True(result.IsSuccess);
                Assert.Same(_newDBUser, entityOwnedByOriginal.Owner);
                Assert.Equal(ModifyEntityAction.Edited, result.ActionTaken);
            }

            public Task DisposeAsync()
            {
                return Task.CompletedTask;
            }
        }

        public class IsEntityNameValid : OwnedEntityServiceTestBase, IAsyncLifetime
        {
            private ModuleInfo _commandModule;
            private IServiceProvider _services;

            public async Task InitializeAsync()
            {
                var commands = new CommandService();

                var client = new DiscordSocketClient();

                _services = new ServiceCollection()
                    .AddSingleton(this.Database)
                    .AddSingleton<ContentService>()
                    .AddSingleton<CommandService>()
                    .AddSingleton<DiscordService>()
                    .AddSingleton<UserFeedbackService>()
                    .AddSingleton<OwnedEntityService>()
                    .AddSingleton<TransformationService>()
                    .AddSingleton<InteractivityService>()
                    .AddSingleton<CharacterService>()
                    .AddSingleton(client)
                    .AddSingleton<BaseSocketClient>(client)
                    .AddSingleton<Random>()
                    .BuildServiceProvider();

                commands.AddTypeReader<Character>(new CharacterTypeReader());
                _commandModule = await commands.AddModuleAsync<CharacterCommands>(_services);
            }

            [Fact]
            public void ReturnsFailureForNullNames()
            {
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                var result = this.Entities.IsEntityNameValid(_commandModule, null);

                Assert.False(result.IsSuccess);
                Assert.Equal(CommandError.ObjectNotFound, result.Error);
            }

            [Theory]
            [InlineData(':')]
            public void ReturnsFailureIfNameContainsInvalidCharacters(char invalidCharacter)
            {
                var result = this.Entities.IsEntityNameValid(_commandModule, $"Test{invalidCharacter}");

                Assert.False(result.IsSuccess);
                Assert.Equal(CommandError.UnmetPrecondition, result.Error);
            }

            [Theory]
            [InlineData("current")]
            public void ReturnsFailureIfNameIsAReservedName(string reservedName)
            {
                var result = this.Entities.IsEntityNameValid(_commandModule, reservedName);

                Assert.False(result.IsSuccess);
                Assert.Equal(CommandError.UnmetPrecondition, result.Error);
            }

            [Theory]
            [InlineData("create")]
            [InlineData("show")]
            [InlineData("character show")]
            [InlineData("create Test Testsson")]
            [InlineData("set name Amby")]
            public void ReturnsFailureIfNameContainsACommandName(string commandName)
            {
                var result = this.Entities.IsEntityNameValid(_commandModule, commandName);

                Assert.False(result.IsSuccess);
                Assert.Equal(CommandError.UnmetPrecondition, result.Error);
            }

            [Theory]
            [InlineData("Norm")]
            [InlineData("Tali'Zorah")]
            [InlineData("August Strindberg")]
            public void ReturnsSuccessForNormalNames(string name)
            {
                var result = this.Entities.IsEntityNameValid(_commandModule, name);

                Assert.True(result.IsSuccess);
            }

            public Task DisposeAsync()
            {
                return Task.CompletedTask;
            }
        }
    }
}
