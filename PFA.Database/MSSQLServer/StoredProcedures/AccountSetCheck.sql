CREATE procedure [dbo].[AccountSetCheck]
(	
	@AccountId bigint
)
as
begin	
	update Accounts set account_checkdt = getdate() where account_id = @AccountId;
end