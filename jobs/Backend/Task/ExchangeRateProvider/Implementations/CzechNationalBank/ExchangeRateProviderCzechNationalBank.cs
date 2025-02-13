using ExchangeRateProvider.Models;

namespace ExchangeRateProvider.Implementations.CzechNationalBank;

internal class ExchangeRateProviderCzechNationalBank(ICzechNationalBankApi czechNationalBankApi) : IExchangeRateProvider
{
    private readonly ICzechNationalBankApi _czechNationalBankApi = czechNationalBankApi;

    public string TargetCurrency => "CZK";

    public async Task<ICollection<ExchangeRate>> GetExchangeRates(ICollection<Currency> currencies, DateTimeOffset date)
    {
        var response = await _czechNationalBankApi.GetExratesDaily(date);
        return response
            .Rates
            .Where(res => currencies.Any(curr => curr.Code.Equals(res.CurrencyCode, StringComparison.OrdinalIgnoreCase)))
            .Select(res =>
                new ExchangeRate(
                    new Currency(res.CurrencyCode.ToUpperInvariant()),
                    new Currency(TargetCurrency.ToUpperInvariant()),
                    res.Rate
                )
            ).ToArray();
    }
}