CREATE procedure [dbo].[AccountDel]
(	
	@AccountId bigint
)
as
begin	
	update dbo.Accounts set account_status = -999 where account_id = @AccountId;
end
GO