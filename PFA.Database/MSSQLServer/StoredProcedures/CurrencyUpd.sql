CREATE procedure [dbo].[CurrencyUpd]
(	
	@CurrencyCode varchar(3),
    @CurrencyName varchar(255),
    @CurrencySymbol varchar(10),
    @CurrencyRate decimal(18, 4),
    @CurrencyStatus smallint
)
as
begin	
	update Currencies set 
		currency_name = @CurrencyName,
        currency_symbol = @CurrencySymbol,
        currency_rate = @CurrencyRate,
        currency_status = @CurrencyStatus
    where 
		currency_code = @CurrencyCode;
end