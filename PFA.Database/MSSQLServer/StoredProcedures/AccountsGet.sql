CREATE procedure [dbo].[AccountsGet]
(	
	@AccountId bigint = -1,
	@AccountStatus int = -1
)
as
begin	
	select 
		a.account_id, 
		a.account_name, 
		a.currency_code, 
		a.account_status, 
		c.currency_rate,
		holder_id, 
		account_number, 
		account_type, 
		initial_balance, 
		account_status,
		a.account_checkdt
	from 
		Accounts a
		left join Currencies c on a.currency_code = c.currency_code 
	where
		account_status > @AccountStatus
		and (nullif(@AccountId, -1) is null or a.account_id = @AccountId)
		and (nullif(@AccountStatus, -2) is null or account_status > 0) 		
	order by account_name;
end