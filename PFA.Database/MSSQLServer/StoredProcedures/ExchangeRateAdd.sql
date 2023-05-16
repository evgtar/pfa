CREATE procedure [dbo].[ExchangeRateAdd]
(	
	@CurrencyCodeFrom varchar(3),
	@CurrencyCodeTo varchar(3),
	@ExchangeRateDate varchar(20),
	@ExchangeRate varchar(20)
)
as
begin	
	insert into ExchangeRates (cc_date, cc_from, cc_to, cc_rate, cc_status) values (@ExchangeRateDate, @CurrencyCodeFrom, @CurrencyCodeTo, @ExchangeRate, 0);
    update ExchangeRates set cc_status = 99 where cc_status = 1 and cc_from = @CurrencyCodeFrom and cc_to = @CurrencyCodeTo;
	update ExchangeRates set cc_status = 1 where cc_status = 0 and cc_from = @CurrencyCodeFrom and cc_to = @CurrencyCodeTo;
end