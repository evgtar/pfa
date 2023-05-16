SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accounts](
	[account_id] [bigint] NOT NULL,
	[holder_id] [bigint] NULL,
	[currency_code] [char](3) NOT NULL,
	[account_name] [varchar](255) NOT NULL,
	[account_number] [varchar](255) NULL,
	[initial_balance] [decimal](18, 4) NOT NULL,
	[local_balance] [decimal](18, 4) NOT NULL,
	[base_balance] [decimal](18, 4) NOT NULL,
	[account_status] [smallint] NOT NULL,
	[account_type] [smallint] NULL,
	[min_balance] [decimal](18, 4) NULL,
	[account_checkdt] [datetime] NULL,
	PRIMARY KEY CLUSTERED ([account_id] ASC)
)
GO

CREATE TABLE [dbo].[AccountTypes](
	[account_type] [bigint] NOT NULL,
	[acctype_code] [char](10) NOT NULL,
	[acctype_name] [varchar](255) NOT NULL,
	[acctype_status] [smallint] NOT NULL,
	PRIMARY KEY CLUSTERED ([account_type] ASC)
)
GO

CREATE TABLE [dbo].[AssetGroups](
	[ag_id] [bigint] NOT NULL,
	[ag_name] [varchar](255) NOT NULL,
	[ag_status] [smallint] NOT NULL,
	PRIMARY KEY CLUSTERED([ag_id] ASC)
)
GO

CREATE TABLE [dbo].[Assets](
	[asset_id] [bigint] NOT NULL,
	[ag_id] [bigint] NULL,
	[asset_name] [varchar](255) NOT NULL,
	[asset_note] [text] NULL,
	[asset_status] [smallint] NOT NULL,
	[asset_date_from] [datetime] NULL,
	[asset_date_till] [datetime] NULL,
	PRIMARY KEY CLUSTERED ([asset_id] ASC)
)
GO

CREATE TABLE [dbo].[Categories](
	[category_id] [bigint] NOT NULL,
	[pcategory_id] [bigint] NOT NULL,
	[category_name] [varchar](255) NOT NULL,
	[category_status] [smallint] NOT NULL,
	PRIMARY KEY CLUSTERED ([category_id] ASC)
)
GO

CREATE TABLE [dbo].[Currencies](
	[currency_code] [char](3) NOT NULL,
	[currency_name] [varchar](255) NOT NULL,
	[currency_symbol] [varchar](10) NULL,
	[currency_rate] [decimal](18, 4) NOT NULL,
	[currency_status] [smallint] NOT NULL,
	PRIMARY KEY CLUSTERED ([currency_code] ASC)
)
GO

CREATE TABLE [dbo].[ExchangeRates](
	[cc_date] [datetime] NOT NULL,
	[cc_from] [char](3) NOT NULL,
	[cc_to] [char](3) NOT NULL,
	[cc_rate] [float] NOT NULL,
	[cc_status] [smallint] NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[cc_date] ASC,
		[cc_from] ASC,
		[cc_to] ASC
	)
)
GO

CREATE TABLE [dbo].[Payers](
	[payer_id] [bigint] NOT NULL,
	[payer_name] [varchar](255) NOT NULL,
	PRIMARY KEY CLUSTERED ([payer_id] ASC)
)
GO

CREATE TABLE [dbo].[ReportCostSharing](
	[year] [smallint] NOT NULL,
	[category_id] [bigint] NOT NULL,
	[amount_january] [decimal](18, 4) NULL,
	[amount_february] [decimal](18, 4) NULL,
	[amount_march] [decimal](18, 4) NULL,
	[amount_april] [decimal](18, 4) NULL,
	[amount_may] [decimal](18, 4) NULL,
	[amount_june] [decimal](18, 4) NULL,
	[amount_july] [decimal](18, 4) NULL,
	[amount_august] [decimal](18, 4) NULL,
	[amount_september] [decimal](18, 4) NULL,
	[amount_october] [decimal](18, 4) NULL,
	[amount_november] [decimal](18, 4) NULL,
	[amount_december] [decimal](18, 4) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[year] ASC,
		[category_id] ASC
	)
)
GO

CREATE TABLE [dbo].[ReportExcludes](
	[report_id] [bigint] NOT NULL,
	[exclude_id] [bigint] NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[report_id] ASC,
		[exclude_id] ASC
	)
)
GO

CREATE TABLE [dbo].[Settings](
	[setting_name] [varchar](100) NOT NULL,
	[setting_value] [text] NULL,
	PRIMARY KEY CLUSTERED ([setting_name] ASC)
)

CREATE TABLE [dbo].[tag2transaction](
	[trans_id] [bigint] NOT NULL,
	[tag_id] [bigint] NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[trans_id] ASC,
		[tag_id] ASC
	)
)

CREATE TABLE [dbo].[Tags](
	[tag_id] [bigint] NOT NULL,
	[tag_code] [varchar](50) NULL,
	PRIMARY KEY CLUSTERED ([tag_id] ASC)
)
GO

CREATE TABLE [dbo].[Transactions](
	[trans_id] [bigint] NOT NULL,
	[trans_date] [datetime] NOT NULL,
	[trans_type] [smallint] NOT NULL,
	[category_id] [bigint] NOT NULL,
	[account_id] [bigint] NOT NULL,
	[account_id_ref] [bigint] NULL,
	[amount_local] [decimal](18, 4) NOT NULL,
	[currency_rate] [float] NOT NULL,
	[amount_base] [decimal](18, 4) NULL,
	[trans_notes] [text] NULL,
	[trans_status] [smallint] NOT NULL,
	[payer_id] [bigint] NULL,
	[asset_id] [bigint] NULL,
	[liability_id] [bigint] NULL,
	PRIMARY KEY CLUSTERED ([trans_id] ASC)
) 
GO

CREATE TABLE [dbo].[TransactionsLog](
	[trans_id] [bigint] NOT NULL,
	[trans_log_dt] [datetime] NOT NULL,
	[trans_log_txt1] [ntext] NULL,
	[trans_source] [smallint] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[trans_id] ASC,
		[trans_log_dt] ASC
	)
)
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0 - manual entry, 1 - added from external source' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionsLog', @level2type=N'COLUMN',@level2name=N'trans_source'
GO

ALTER TABLE [dbo].[Accounts] ADD  DEFAULT ((0.0)) FOR [initial_balance]
GO
ALTER TABLE [dbo].[Accounts] ADD  DEFAULT ((0.0)) FOR [local_balance]
GO
ALTER TABLE [dbo].[Accounts] ADD  DEFAULT ((0.0)) FOR [base_balance]
GO
ALTER TABLE [dbo].[Accounts] ADD  DEFAULT ((0)) FOR [account_status]
GO
ALTER TABLE [dbo].[Accounts] ADD  DEFAULT ((1)) FOR [account_type]
GO
ALTER TABLE [dbo].[Accounts] ADD  DEFAULT ((0.50)) FOR [min_balance]
GO
ALTER TABLE [dbo].[AccountTypes] ADD  DEFAULT ((0)) FOR [acctype_status]
GO
ALTER TABLE [dbo].[AssetGroups] ADD  DEFAULT ((0)) FOR [ag_status]
GO
ALTER TABLE [dbo].[Assets] ADD  DEFAULT ((0)) FOR [asset_status]
GO
ALTER TABLE [dbo].[Categories] ADD  DEFAULT ((0)) FOR [pcategory_id]
GO
ALTER TABLE [dbo].[Categories] ADD  DEFAULT ((0)) FOR [category_status]
GO
ALTER TABLE [dbo].[CreditPayments] ADD  DEFAULT ((0)) FOR [crpaym_type_id]
GO
ALTER TABLE [dbo].[CreditPayments] ADD  DEFAULT ((0)) FOR [crpaym_status]
GO
ALTER TABLE [dbo].[Credits] ADD  DEFAULT ((0.0)) FOR [credit_amount]
GO
ALTER TABLE [dbo].[Credits] ADD  DEFAULT ((1)) FOR [credit_terms]
GO
ALTER TABLE [dbo].[Credits] ADD  DEFAULT ((0.0)) FOR [credit_percent]
GO
ALTER TABLE [dbo].[Credits] ADD  DEFAULT ((1)) FOR [credit_percent_charge]
GO
ALTER TABLE [dbo].[Credits] ADD  DEFAULT ((1)) FOR [credit_repayment_periodicity]
GO
ALTER TABLE [dbo].[Credits] ADD  DEFAULT ((0)) FOR [credit_status]
GO
ALTER TABLE [dbo].[Currencies] ADD  DEFAULT ((1.0)) FOR [currency_rate]
GO
ALTER TABLE [dbo].[Currencies] ADD  DEFAULT ((0)) FOR [currency_status]
GO
ALTER TABLE [dbo].[Events] ADD  DEFAULT ((1)) FOR [event_status]
GO
ALTER TABLE [dbo].[ExchangeRates] ADD  DEFAULT ((1)) FOR [cc_rate]
GO
ALTER TABLE [dbo].[ExchangeRates] ADD  DEFAULT ((0)) FOR [cc_status]
GO
ALTER TABLE [dbo].[Holders] ADD  DEFAULT ((0)) FOR [holder_status]
GO
ALTER TABLE [dbo].[Investment] ADD  DEFAULT ((1)) FOR [invest_price2calc]
GO
ALTER TABLE [dbo].[Investment] ADD  DEFAULT ((0)) FOR [invest_status]
GO
ALTER TABLE [dbo].[Investment] ADD  DEFAULT ((0)) FOR [invest_qty]
GO
ALTER TABLE [dbo].[Investment] ADD  DEFAULT ((0.01)) FOR [invest_ave_price]
GO
ALTER TABLE [dbo].[Investment] ADD  DEFAULT ((0.01)) FOR [invest_last_price]
GO
ALTER TABLE [dbo].[Investment] ADD  DEFAULT ('USD') FOR [invest_currency]
GO
ALTER TABLE [dbo].[Investment] ADD  DEFAULT ((1)) FOR [invest_rate]
GO
ALTER TABLE [dbo].[InvestRates] ADD  DEFAULT ((1)) FOR [rate_buy]
GO
ALTER TABLE [dbo].[Liabilities] ADD  DEFAULT ((0)) FOR [liability_status]
GO
ALTER TABLE [dbo].[LiabilityGroups] ADD  DEFAULT ((0)) FOR [lg_status]
GO
ALTER TABLE [dbo].[StockShares] ADD  DEFAULT (getdate()) FOR [ss_date]
GO
ALTER TABLE [dbo].[StockShares] ADD  DEFAULT ('USD') FOR [ss_currency]
GO
ALTER TABLE [dbo].[StockShares] ADD  DEFAULT ((0.0)) FOR [ss_commission]
GO
ALTER TABLE [dbo].[Transactions] ADD  DEFAULT (getdate()) FOR [trans_date]
GO
ALTER TABLE [dbo].[Transactions] ADD  DEFAULT ((0)) FOR [trans_type]
GO
ALTER TABLE [dbo].[Transactions] ADD  DEFAULT ((0.0)) FOR [amount_local]
GO
ALTER TABLE [dbo].[Transactions] ADD  DEFAULT ((1)) FOR [currency_rate]
GO
ALTER TABLE [dbo].[Transactions] ADD  DEFAULT ((0)) FOR [trans_status]
GO
ALTER TABLE [dbo].[TransactionsLog] ADD  DEFAULT ((0)) FOR [trans_source]
GO