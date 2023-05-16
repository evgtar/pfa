CREATE procedure [dbo].[ExchangeRateGetAll]
(	
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
		cc_status >= @ExchangeRateStatus  
	order by cc_date desc
	
end