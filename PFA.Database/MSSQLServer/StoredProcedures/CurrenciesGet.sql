CREATE procedure [dbo].[CurrenciesGet]
(	
	@CurrencyStatus int = -1
)
as
begin	
	select 
		* 
	from 
		dbo.Currencies 
	where 
		(nullif(@CurrencyStatus, -2) is null or currency_status > 0) 
		and currency_status > @CurrencyStatus
	order by 
		currency_name;
end