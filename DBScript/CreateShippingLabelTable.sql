CREATE TABLE [dbo].[ShippingLabel] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [RecevierZipcode]       NVARCHAR (MAX) NULL,
    [ReceiverShortAddress1] NVARCHAR (MAX) NULL,
    [ReceiverShortAddress2] NVARCHAR (MAX) NULL,
    [DriverName]            NVARCHAR (MAX) NULL,
    [DriverCode]            NVARCHAR (MAX) NULL,
    [MainTerminalCode]      NVARCHAR (MAX) NULL,
    [SubTerminalCode]       NVARCHAR (MAX) NULL,
    [OrderId]               INT FOREIGN KEY REFERENCES [dbo].[Order](ORDERID),
    [AgencyName]            NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.ShippingLabel] PRIMARY KEY CLUSTERED ([ID] ASC)
);