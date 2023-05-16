CREATE procedure [dbo].[ReportPayersGet]
(
	@Year int 
)
as
begin		
	with tg as(
		select 
			payer_id, 
			month(t.trans_date) as TM,
			coalesce(sum(t.amount_base), 0.00) as Amount
		from 
			Transactions t
		where 
			year(t.trans_date) = isnull(@Year, year(GETDATE()))
			and t.trans_type = 0
			and t.trans_status > 0
		group by
			payer_id, month(t.trans_date)
		having
			sum(t.amount_base) > 0
	)
	select 
		payer_name CName, 
		sum(isnull([1], 0)) as M01, 
		sum(isnull([2], 0)) as M02, 
		sum(isnull([3], 0)) as M03, 
		sum(isnull([4], 0)) as M04, 
		sum(isnull([5], 0)) as M05, 
		sum(isnull([6], 0)) as M06, 
		sum(isnull([7], 0)) as M07, 
		sum(isnull([8], 0)) as M08, 
		sum(isnull([9], 0)) as M09, 
		sum(isnull([10], 0)) as M10, 
		sum(isnull([11], 0)) as M11, 
		sum(isnull([12], 0)) as M12
	from 
		Payers p
	inner join (
		select 
			payer_id, [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12]
		from tg		
		pivot(
			sum(amount) for tm in ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
		) p
	) res on res.payer_id = p.payer_id
	group by p.payer_name
	--having
	--	sum(isnull([1], 0)) > 0 or 
	--	sum(isnull([2], 0)) > 0 or 
	--	sum(isnull([3], 0)) > 0 or 
	--	sum(isnull([4], 0)) > 0 or  
	--	sum(isnull([5], 0)) > 0 or  
	--	sum(isnull([6], 0)) > 0 or  
	--	sum(isnull([7], 0)) > 0 or  
	--	sum(isnull([8], 0)) > 0 or  
	--	sum(isnull([9], 0)) > 0 or  
	--	sum(isnull([10], 0)) > 0 or  
	--	sum(isnull([11], 0)) > 0 or
	--	sum(isnull([12], 0)) > 0 
	order by p.payer_name;
end
;