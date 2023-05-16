CREATE procedure [dbo].[SettingsGet]
(
	@SettingName varchar(50) = ''
)
as
begin	
	select 
		setting_name,
		setting_value 
	from 
		dbo.Settings
	where
		nullif(@SettingName, '') is null or setting_name = @SettingName
	order by
		setting_name;
end
;