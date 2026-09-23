using Mapster;
using Wallets.Application.Dto;
using Wallets.Domain.Models;

namespace Wallets.Application.Mapping;

public sealed class MappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateWalletDto, Wallet>()
            .Ignore(dest => dest.Commissions)
            .AfterMapping(dest => dest.Commissions = []);

        config.NewConfig<Wallet, WalletDto>();
        config.NewConfig<Commission, CommissionDto>();
    }
}
