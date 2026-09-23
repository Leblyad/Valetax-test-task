using AutoFixture;
using MapsterMapper;
using Moq;
using SharedModels.Enums;
using Wallets.Application.Dto;
using Wallets.Application.Exceptions;
using Wallets.Application.Interfaces.Clients;
using Wallets.Application.Interfaces.Repositories;
using Wallets.Application.Services;
using Wallets.Domain.Models;

namespace Wallets.Application.Tests.Services;

public class CommissionServiceTests
{
    private readonly Mock<IRepositoryManager> repositoryMock;
    private readonly Mock<ICommissionRepository> commissionRepositoryMock;
    private readonly Mock<IWalletRepository> walletRepositoryMock;
    private readonly Mock<IUsersApiClient> usersApiClientMock;
    private readonly Mock<IMapper> mapperMock;
    private readonly CommissionService service;
    private readonly Fixture fixture;

    public CommissionServiceTests()
    {
        repositoryMock = new Mock<IRepositoryManager>();
        commissionRepositoryMock = new Mock<ICommissionRepository>();
        walletRepositoryMock = new Mock<IWalletRepository>();
        usersApiClientMock = new Mock<IUsersApiClient>();
        mapperMock = new Mock<IMapper>();
        fixture = new Fixture();
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        repositoryMock.Setup(x => x.Commission).Returns(commissionRepositoryMock.Object);
        repositoryMock.Setup(x => x.Wallet).Returns(walletRepositoryMock.Object);

        service = new CommissionService(
            repositoryMock.Object,
            usersApiClientMock.Object,
            mapperMock.Object);
    }

    [Fact]
    public async Task ProcessEventAsync_NonPositiveProfit_DoesNotCallUsersOrSave()
    {
        //Arrange
        var processEventDto = new ProcessEventDto
        {
            EventExternalId = Guid.NewGuid(),
            UserExternalId = Guid.NewGuid(),
            Profit = 0,
        };

        //Act
        await service.ProcessEventAsync(processEventDto);

        //Assert
        usersApiClientMock.Verify(
            x => x.GetPartnerRelationsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);
        commissionRepositoryMock.Verify(x => x.CreateRange(It.IsAny<IEnumerable<Commission>>()), Times.Never);
        repositoryMock.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ProcessEventAsync_NegativeProfit_DoesNotCallUsersOrSave()
    {
        //Arrange
        var processEventDto = new ProcessEventDto
        {
            EventExternalId = Guid.NewGuid(),
            UserExternalId = Guid.NewGuid(),
            Profit = -100,
        };

        //Act
        await service.ProcessEventAsync(processEventDto);

        //Assert
        commissionRepositoryMock.Verify(
            x => x.GetByEventExternalIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()),
            Times.Never);
        repositoryMock.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ProcessEventAsync_ExistingCommissions_DoesNotCreateAgain()
    {
        //Arrange
        var processEventDto = fixture.Build<ProcessEventDto>()
            .With(x => x.Profit, 1000m)
            .Create();

        var existing = fixture.CreateMany<Commission>(2).ToList();

        commissionRepositoryMock
            .Setup(x => x.GetByEventExternalIdAsync(processEventDto.EventExternalId, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        //Act
        await service.ProcessEventAsync(processEventDto);

        //Assert
        usersApiClientMock.Verify(
            x => x.GetPartnerRelationsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);
        commissionRepositoryMock.Verify(x => x.CreateRange(It.IsAny<IEnumerable<Commission>>()), Times.Never);
        repositoryMock.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ProcessEventAsync_NoPartnerRelations_DoesNotSave()
    {
        //Arrange
        var processEventDto = fixture.Build<ProcessEventDto>()
            .With(x => x.Profit, 1000m)
            .Create();

        commissionRepositoryMock
            .Setup(x => x.GetByEventExternalIdAsync(processEventDto.EventExternalId, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        usersApiClientMock
            .Setup(x => x.GetPartnerRelationsAsync(processEventDto.UserExternalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        //Act
        await service.ProcessEventAsync(processEventDto);

        //Assert
        commissionRepositoryMock.Verify(x => x.CreateRange(It.IsAny<IEnumerable<Commission>>()), Times.Never);
        repositoryMock.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ProcessEventAsync_LinearPartners_CreatesCommissionsAndUpdatesBalances()
    {
        //Arrange
        var eventId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var partnerLevel1Id = Guid.NewGuid();
        var partnerLevel2Id = Guid.NewGuid();
        const decimal profit = 1000m;

        var processEventDto = new ProcessEventDto
        {
            EventExternalId = eventId,
            UserExternalId = userId,
            Profit = profit,
        };

        var walletLevel1 = new Wallet
        {
            UserExternalId = partnerLevel1Id,
            Balance = 0,
            SchemaType = SchemaType.Linear,
            Commissions = [],
        };
        var walletLevel2 = new Wallet
        {
            UserExternalId = partnerLevel2Id,
            Balance = 5,
            SchemaType = SchemaType.Linear,
            Commissions = [],
        };

        IReadOnlyList<Commission>? createdCommissions = null;

        commissionRepositoryMock
            .Setup(x => x.GetByEventExternalIdAsync(eventId, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        usersApiClientMock
            .Setup(x => x.GetPartnerRelationsAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                new PartnerRelationInfo { UserExternalId = userId, PartnerExternalId = partnerLevel1Id, Level = 1 },
                new PartnerRelationInfo { UserExternalId = userId, PartnerExternalId = partnerLevel2Id, Level = 2 },
            ]);

        walletRepositoryMock
            .Setup(x => x.GetByUserExternalIdAsync(partnerLevel1Id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(walletLevel1);
        walletRepositoryMock
            .Setup(x => x.GetByUserExternalIdAsync(partnerLevel2Id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(walletLevel2);

        commissionRepositoryMock
            .Setup(x => x.CreateRange(It.IsAny<IEnumerable<Commission>>()))
            .Callback<IEnumerable<Commission>>(items => createdCommissions = items.ToList());

        //Act
        await service.ProcessEventAsync(processEventDto);

        //Assert
        Assert.NotNull(createdCommissions);
        Assert.Equal(2, createdCommissions.Count);

        var level1 = Assert.Single(createdCommissions, c => c.Level == 1);
        Assert.Equal(partnerLevel1Id, level1.UserExternalId);
        Assert.Equal(10m, level1.Amount);
        Assert.Equal(SchemaType.Linear, level1.SchemaType);
        Assert.Equal(eventId, level1.EventExternalId);
        Assert.NotNull(level1.PaidAt);

        var level2 = Assert.Single(createdCommissions, c => c.Level == 2);
        Assert.Equal(20m, level2.Amount);

        Assert.Equal(10m, walletLevel1.Balance);
        Assert.Equal(25m, walletLevel2.Balance);

        commissionRepositoryMock.Verify(x => x.CreateRange(It.IsAny<IEnumerable<Commission>>()), Times.Once);
        repositoryMock.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ProcessEventAsync_FibonacciPartner_CreatesExpectedAmount()
    {
        //Arrange
        var eventId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var partnerId = Guid.NewGuid();
        const decimal profit = 1000m;

        var processEventDto = new ProcessEventDto
        {
            EventExternalId = eventId,
            UserExternalId = userId,
            Profit = profit,
        };

        var wallet = new Wallet
        {
            UserExternalId = partnerId,
            Balance = 0,
            SchemaType = SchemaType.Fibonacci,
            Commissions = [],
        };

        IReadOnlyList<Commission>? createdCommissions = null;

        commissionRepositoryMock
            .Setup(x => x.GetByEventExternalIdAsync(eventId, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        usersApiClientMock
            .Setup(x => x.GetPartnerRelationsAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                new PartnerRelationInfo { UserExternalId = userId, PartnerExternalId = partnerId, Level = 5 },
            ]);

        walletRepositoryMock
            .Setup(x => x.GetByUserExternalIdAsync(partnerId, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(wallet);

        commissionRepositoryMock
            .Setup(x => x.CreateRange(It.IsAny<IEnumerable<Commission>>()))
            .Callback<IEnumerable<Commission>>(items => createdCommissions = items.ToList());

        //Act
        await service.ProcessEventAsync(processEventDto);

        //Assert
        var commission = Assert.Single(createdCommissions!);
        Assert.Equal(50m, commission.Amount);
        Assert.Equal(SchemaType.Fibonacci, commission.SchemaType);
        Assert.Equal(50m, wallet.Balance);
    }

    [Fact]
    public async Task ProcessEventAsync_MissingWallet_ThrowsWalletNotFoundException()
    {
        //Arrange
        var processEventDto = fixture.Build<ProcessEventDto>()
            .With(x => x.Profit, 1000m)
            .Create();
        var partnerId = Guid.NewGuid();

        commissionRepositoryMock
            .Setup(x => x.GetByEventExternalIdAsync(processEventDto.EventExternalId, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        usersApiClientMock
            .Setup(x => x.GetPartnerRelationsAsync(processEventDto.UserExternalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                new PartnerRelationInfo
                {
                    UserExternalId = processEventDto.UserExternalId,
                    PartnerExternalId = partnerId,
                    Level = 1,
                },
            ]);

        walletRepositoryMock
            .Setup(x => x.GetByUserExternalIdAsync(partnerId, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Wallet?)null);

        var act = async () => await service.ProcessEventAsync(processEventDto);

        //Act
        var exception = await Assert.ThrowsAsync<WalletNotFoundException>(act);

        //Assert
        Assert.Equal(partnerId, exception.UserExternalId);
        Assert.Equal(WalletNotFoundException.ERROR_CODE, exception.Code);
        repositoryMock.Verify(x => x.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ProcessEventAsync_LevelsOutsideAllowedRange_AreIgnored()
    {
        //Arrange
        var eventId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var validPartnerId = Guid.NewGuid();
        var invalidPartnerId = Guid.NewGuid();

        var processEventDto = new ProcessEventDto
        {
            EventExternalId = eventId,
            UserExternalId = userId,
            Profit = 1000m,
        };

        var validWallet = new Wallet
        {
            UserExternalId = validPartnerId,
            Balance = 0,
            SchemaType = SchemaType.Linear,
            Commissions = [],
        };

        IReadOnlyList<Commission>? createdCommissions = null;

        commissionRepositoryMock
            .Setup(x => x.GetByEventExternalIdAsync(eventId, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        usersApiClientMock
            .Setup(x => x.GetPartnerRelationsAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                new PartnerRelationInfo { UserExternalId = userId, PartnerExternalId = invalidPartnerId, Level = 0 },
                new PartnerRelationInfo { UserExternalId = userId, PartnerExternalId = invalidPartnerId, Level = 11 },
                new PartnerRelationInfo { UserExternalId = userId, PartnerExternalId = validPartnerId, Level = 1 },
            ]);

        walletRepositoryMock
            .Setup(x => x.GetByUserExternalIdAsync(validPartnerId, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validWallet);

        commissionRepositoryMock
            .Setup(x => x.CreateRange(It.IsAny<IEnumerable<Commission>>()))
            .Callback<IEnumerable<Commission>>(items => createdCommissions = items.ToList());

        //Act
        await service.ProcessEventAsync(processEventDto);

        //Assert
        var commission = Assert.Single(createdCommissions!);
        Assert.Equal(validPartnerId, commission.UserExternalId);
        Assert.Equal(1, commission.Level);

        walletRepositoryMock.Verify(
            x => x.GetByUserExternalIdAsync(invalidPartnerId, true, It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task GetCommissionsByEventAsync_ExistingEvent_ReturnsMappedDtos()
    {
        //Arrange
        var eventId = Guid.NewGuid();
        var commissions = fixture.CreateMany<Commission>(3).ToList();
        var commissionDtos = fixture.CreateMany<CommissionDto>(3).ToList();

        commissionRepositoryMock
            .Setup(x => x.GetByEventExternalIdAsync(eventId, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(commissions);

        mapperMock
            .Setup(x => x.Map<List<CommissionDto>>(commissions))
            .Returns(commissionDtos);

        //Act
        var actual = await service.GetCommissionsByEventAsync(eventId);

        //Assert
        Assert.Equal(commissionDtos, actual);
        mapperMock.Verify(x => x.Map<List<CommissionDto>>(commissions), Times.Once);
    }
}
