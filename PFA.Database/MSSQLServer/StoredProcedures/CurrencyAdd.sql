CREATE procedure [dbo].[CurrencyAdd]
(	
	@CurrencyCode varchar(3),
    @CurrencyName varchar(255),
    @CurrencySymbol varchar(10),
    @CurrencyRate decimal(18, 4),
    @CurrencyStatus smallint
)
as
begin	
	insert into Currencies (currency_code, currency_name, currency_symbol, currency_rate, currency_status) 
	values (@CurrencyCode, @CurrencyName, @CurrencySymbol, @CurrencyRate, @CurrencyStatus);
end