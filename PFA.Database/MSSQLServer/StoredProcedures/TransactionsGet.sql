CREATE procedure [dbo].[TransactionsGet]
(	
	@TransFrom varchar(20),
	@TransTill varchar(20),
	@AccountId bigint = -1,
	@CategoryId bigint = -1,
	@PayerId bigint = -1,
	@AssetId bigint = -1,
	@Tags varchar(8000) = null
)
as
begin	
	select 
		trans_id, 
		trans.account_id, 
		trans.payer_id, 
		currency_rate, 
		trans.asset_id,
		convert(date, trans_date, 120) DT, account_name, concat(pcat.category_name, ' - ', cat.category_name) CName, 
		currency_code, 
		amount_local,
		(case when trans_type = 0 then amount_local else 0.00 end) deposit, 
		(case when trans_type = 1 then amount_local else 0.00 end) withdraw, 
		isnull(p.payer_name, '') as payer_name,
		isnull(a.asset_name, '') as asset_name,
		isnull(trans_notes, '') as trans_notes,
		(case when trans_type = 0 then amount_base else 0.00 end) as deposit_base, 
		(case when trans_type = 1 then amount_base else 0.00 end) as withdraw_base,
		(case when trans_type = 1 then -1 else 1 end) * amount_base as amount_base,
		trans.trans_type
	from 
		Transactions as trans
		left join Accounts acc on trans.account_id = acc.account_id
		left join Categories cat on trans.category_id = cat.category_id
		left join Categories pcat on cat.pcategory_id = pcat.category_id
		left join Payers p on p.payer_id=trans.payer_id
		left outer join Assets a on a.asset_id = trans.asset_id
	where 
		trans_date between convert(datetime, @TransFrom, 120) and convert(datetime, @TransTill, 120)
		and trans_status > 0
		and (nullif(@AccountId, -1) is null or acc.account_id = @AccountId)
		and (nullif(@CategoryId, -1) is null or trans.category_id = @CategoryId)
		and (nullif(@PayerId, -1) is null or trans.payer_id = @PayerId)		
		and (nullif(@AssetId, -1) is null or trans.asset_id = @AssetId)		
		and (nullif(ltrim(rtrim(@Tags)), '') is null or trans.trans_notes like '%'+@Tags+'%')		
	order by trans_date;
end
;