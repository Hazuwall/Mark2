using Common;
using Shouldly;
using System;
using Xunit;

namespace Server.Roles.Tests
{
    public class ClientRoleRegistryTests
    {
        IClientRoleRegistry _registry;
        Guid _client1;
        Guid _client2;

        public ClientRoleRegistryTests()
        {
            _registry = new ClientRoleRegistry();
            _client1 = Guid.NewGuid();
            _client2 = Guid.NewGuid();
        }

        [Theory]
        [InlineData(Role.Reader)]
        [InlineData(Role.Writer)]
        [InlineData(Role.Admin)]
        public void ShoudStoreClientRole(Role role)
        {
            _registry.SetClientRole(_client1, role);
            var newRole = _registry.GetRole(_client1);
            newRole.ShouldBe(role);
        }

        [Theory]
        [InlineData(Role.Writer)]
        [InlineData(Role.Admin)]
        public void ShouldStoreNonReaderRoleOwner(Role role)
        {
            _registry.SetClientRole(_client1, role);
            var owner = _registry.GetOwner(role);
            owner.ShouldBe(_client1);
        }

        [Fact]
        public void ShoudStoreSeparateWriterAndAdmin()
        {
            _registry.SetClientRole(_client1, Role.Admin);
            _registry.SetClientRole(_client2, Role.Writer);
            var admin = _registry.GetOwner(Role.Admin);
            var writer = _registry.GetOwner(Role.Writer);
            admin.Value.ShouldNotBe(writer.Value);
        }

        [Fact]
        public void ShouldNotStoreReaderRoleOwner()
        {
            _registry.SetClientRole(Guid.Empty, Role.Reader);
            var reader = _registry.GetOwner(Role.Reader);
            reader.ShouldBeNull();
        }

        [Theory]
        [InlineData(Role.Writer)]
        [InlineData(Role.Admin)]
        public void ShouldReplaceRoleOwner_IfNewWasSet(Role role)
        {
            _registry.SetClientRole(_client1, role);
            _registry.SetClientRole(_client2, role);
            var owner = _registry.GetOwner(role);
            owner.ShouldBe(_client2);
        }

        [Fact]
        public void ShouldNotChangeWriter_IfNewAdminWasSet()
        {
            _registry.SetClientRole(_client1, Role.Writer);
            _registry.SetClientRole(_client2, Role.Admin);
            var owner = _registry.GetOwner(Role.Writer);
            owner.ShouldBe(_client1);
        }

        [Theory]
        [InlineData(Role.Writer, Role.Admin)]
        [InlineData(Role.Admin, Role.Writer)]
        public void ShouldSetOwnerToNull_IfClientChangedRole(Role oldRole, Role newRole)
        {
            _registry.SetClientRole(_client1, oldRole);
            _registry.SetClientRole(_client1, newRole);
            var owner = _registry.GetOwner(oldRole);
            owner.ShouldBeNull();
        }
    }
}
