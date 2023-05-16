CREATE procedure [dbo].[ExchangeRateGet]
(	
	@CurrencyCodeFrom varchar(3),
	@CurrencyCodeTo varchar(3),
	@ExchangeRateStatus int = 1
)
as
begin	
	select 
		cc_from, 
		cc_to, 
		cc_rate 
	from 
		ExchangeRates 
	where 
		cc_from = @CurrencyCodeFrom
		and cc_to = @CurrencyCodeTo
		and cc_status >= @ExchangeRateStatus  
	order by cc_date desc
end