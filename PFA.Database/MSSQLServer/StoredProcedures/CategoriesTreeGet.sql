CREATE procedure [dbo].[CategoriesTreeGet]
(	
	@CategoryId bigint = -1,
	@CategoryStatus int = -1
)
as
begin	
	select 
		category_id,
		pcategory_id,
		category_name,
		category_status
	from 
		Categories cat
	where
		cat.category_status > 0
		and (nullif(@CategoryId, -1) is null or cat.category_id = @CategoryId)
		and (nullif(@CategoryStatus, -1) is null or cat.category_status = @CategoryStatus)		
	order by 
		category_name;
end