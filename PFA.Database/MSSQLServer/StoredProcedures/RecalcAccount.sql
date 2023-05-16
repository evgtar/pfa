CREATE procedure [dbo].[RecalcAccount]
(
	@AccountId as bigint,
	@CurrencyRate float = 1
)
as
begin	
	update Accounts set 
		local_balance = initial_balance  + dep_local  - wd_local,
		base_balance = initial_balance  / @CurrencyRate + dep_base - wd_base
	from
		Accounts a
		inner join (
			select account_id, sum(dep_local) dep_local, sum(dep_base) dep_base, sum(wd_local) wd_local, sum(wd_base) wd_base
			from
			(
				select  account_id, coalesce(sum(amount_local), 0) as dep_local, coalesce(sum(amount_base), 0) as dep_base, cast(0 as int) as wd_local, cast(0 as int) as wd_base
				from Transactions 
				where 
					account_id = @AccountId and trans_type = 0 and trans_status > 0
				group by account_id
				union
				select  account_id, cast(0 as int) as dep_local, cast(0 as int) as dep_base, coalesce(sum(amount_local), 0) as wd_local, coalesce(sum(amount_base), 0) as wd_base
				from Transactions 
				where 
					account_id = @AccountId and trans_type = 1 and trans_status > 0
				group by account_id
			) as t
			group by account_id
		) as tr on tr.account_id = a.account_id
	where a.account_id = @AccountId;
	
	select * from Accounts where account_id = @AccountId;
end
;