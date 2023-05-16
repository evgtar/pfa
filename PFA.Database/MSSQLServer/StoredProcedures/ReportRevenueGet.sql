CREATE procedure [dbo].[ReportRevenueGet]
(
	@Year int 
)
as
begin	
	with tg as(
		select 
			category_id, 
			month(t.trans_date) as TM,
			coalesce(sum(t.amount_base), 0.00) as Amount
		from 
			Transactions t
		where 
			category_id not in (select exclude_id from ReportExcludes where report_id = 3) 	
			and year(t.trans_date) = isnull(@Year, year(GETDATE()))
			and t.trans_type = 0
			and t.trans_status > 0
		group by
			category_id, month(t.trans_date)
	)
	select 
		pcat.category_id as CategoryID,
		concat(pcat.category_name,' - ', cat.category_name) CName, 
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
		Categories cat
		inner join Categories pcat on cat.pcategory_id = pcat.category_id
	inner join (
		select 
			category_id, [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12]
		from tg		
		pivot(
			sum(amount) for tm in ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
		) p
	) res on cat.category_id = res.category_id 
	group by pcat.category_id, concat(pcat.category_name,' - ', cat.category_name)
	order by CName;
end
;