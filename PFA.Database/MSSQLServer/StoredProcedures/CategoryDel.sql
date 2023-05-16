CREATE procedure [dbo].[CategoryDel]
(	
	@CategoryId varchar(3)    
)
as
begin	
	update Categories set category_status = -1 where pcategory_id = @CategoryId or category_id = @CategoryId;
end