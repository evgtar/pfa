CREATE procedure [dbo].[ReportIncomingsAndExpensesDetailedGet]
(
	@Year int,
	@DetailedMonth int
)
as
begin	
	with t as (
	select 
		t.category_id,
		sum(case when trans_type = 0 then amount_base else 0.0 end) as Incomings, 
		sum(case when trans_type = 1 then amount_base else 0.0 end) as Expenses,
		sum(case when trans_type = 0 then amount_base else -1 * amount_base end) as Diff
	from
		Transactions t	
		left outer join ReportExcludes re on re.report_id = 2 and re.exclude_id = t.category_id
	where
		year(trans_date) = @Year and month(trans_date) = @DetailedMonth
		and re.report_id is null
		and t.trans_status > 0
	group by
		t.category_id
	having
		sum(case when trans_type = 0 then amount_base else 0.0 end) > 0 or sum(case when trans_type = 1 then amount_base else 0.0 end) > 0
	)
	select 
		concat('   ',cat.category_name,' - ',tcat.category_name) 'Month', 
		t.Incomings,
		t.Expenses,
		t.Diff
	from
		t
		join Categories tcat on tcat.category_id = t.category_id
		join Categories cat on cat.category_id = tcat.pcategory_id and cat.pcategory_id = 0
	order by cat.category_name, tcat.category_name

end
;