CREATE procedure [dbo].[AccountTypesGet]
as
begin	
	select 
		account_type,
		acctype_name, 
		acctype_code, 
		acctype_status
	from 
		AccountTypes
	where
		acctype_status > 0 
	order by 
		acctype_name;
end