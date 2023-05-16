CREATE procedure [dbo].[CategoriesGet]
(	
	@CategoryId bigint = -1
)
as
begin	
	select 
		cat.category_id CategoryID, 
		concat(pcat.category_name, ' - ', cat.category_name) CName 
	from 
		Categories cat
		inner join Categories pcat on cat.pcategory_id = pcat.category_id 
	where 
		cat.category_status > 0 
		and pcat.category_status > 0
		and (nullif(@CategoryId, -1) is null or cat.category_id = @CategoryId)
	order by 
		CName
end