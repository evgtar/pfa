CREATE procedure [dbo].[PayersGet]
(	
	@PayerId bigint = -1,
	@PayerStatus int = -1
)
as
begin	
	select 
		payer_id, 
		payer_name 
	from 
		Payers 
	where
		nullif(@PayerId, -1) is null or payer_id = @PayerId
		--and (nullif(@PayerStatus, -2) is null or account_status > 0) 
		--and account_status > @AccountStatus
	order by 
		payer_name;
end