CREATE procedure [dbo].[ReportIncomingsAndExpensesGet]
(
	@Year int
)
as
begin	
	select 
		concat(year(trans_date), '-',IIF(month(trans_date)<10, '0', ''), month(trans_date)) as Month, 
		sum(case when trans_type = 0 then amount_base else 0.0 end) as Incomings, 
		sum(case when trans_type = 1 then amount_base else 0.0 end) as Expenses,
		sum(case when trans_type = 0 then amount_base else -1 * amount_base end) as Diff
	from
		Transactions t
		left outer join ReportExcludes re on re.report_id = 2 and re.exclude_id = t.category_id
	where
		year(trans_date) = coalesce(@Year, year(GetDate()))
		and re.report_id is null
		and trans_status > 0
	group by
		concat(year(trans_date), '-',IIF(month(trans_date)<10, '0', ''), month(trans_date))
	order by Month
end
;