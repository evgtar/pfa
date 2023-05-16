CREATE procedure [dbo].[TagsGet]
(	
	@TagId bigint = -1
)
as
begin	
	select 
		tag_id, 
		tag_code 
	from
		tags 	
	where
		nullif(@TagId, -1) is null or tag_id = @TagId
	order by 
		tag_code;
end
;