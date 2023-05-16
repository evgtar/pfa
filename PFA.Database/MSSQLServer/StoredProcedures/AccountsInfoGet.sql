CREATE procedure [dbo].[AccountsInfoGet]
(	
	@AccountId bigint = -1,
	@AccountStatus int = -1,
	@AccountType int = -1
)
as
begin	
	select 
		a.account_id, 
		a.account_name, 
		a.currency_code, 
		cast(round(c.currency_rate, 2) as decimal(18,2)) as currency_rate, 
		cast(round(a.local_balance, 2) as decimal(18,2)) as local_balance, 
		cast(round(a.base_balance, 2) as decimal(18,2)) as base_balance, 
		cast(round(a.local_balance / c.currency_rate, 2) as decimal(18,2)) as base_calc, 
		a.account_checkdt 
	from
		Accounts a
		inner join Currencies c on c.currency_code = a.currency_code
	where 
		((nullif(@AccountStatus, -1) is null and a.account_status > 0) or a.account_status = @AccountStatus)
		and (nullif(@AccountId, -1) is null or a.account_id = @AccountId)
		and abs(a.local_balance / c.currency_rate) > a.min_balance 
		and (nullif(@AccountType, -1) is null or a.account_type = @AccountType)
	order by account_name;
end