CREATE procedure [dbo].[AssetsGet]
(	
	@AssetId bigint = -1,
	@AssetGroupId bigint = -1,
	@AssetStatus int = -1
)
as
begin	
	select 
		a.asset_id as AssetId,
		a.ag_id as AssetGroupId,
		a.asset_name as AssetName,
		ag.ag_name as AssetGroupName,
		coalesce(a.asset_note, '') as AssetNote,
		a.asset_status as AssetStatus
	from 
		Assets a
		join AssetGroups ag on a.ag_id = ag.ag_id
	where
		a.asset_status > @AssetStatus	
		and (nullif(@AssetId, -1) is null or a.asset_id = @AssetId)
		and (nullif(@AssetGroupId, -1) is null or a.ag_id = @AssetGroupId)
	order by a.ag_id, a.asset_id;
end