CREATE procedure [dbo].[SettingUpd]
(
	@SettingName varchar(50),
	@SettingValue varchar(50)
)
as
begin	
	merge dbo.Settings as target
	using (select @SettingName, @SettingValue) as source (SettingName, SettingValue)
	on target.setting_name = source.SettingName
	when matched then
		update set setting_value = source.SettingValue
	when not matched then
		insert (setting_name, setting_value)
		values (source.SettingName, source.SettingValue);
	
end
;