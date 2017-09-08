﻿DECLARE @CurrentMigration [nvarchar](max)

IF object_id('[dbo].[__MigrationHistory]') IS NOT NULL
    SELECT @CurrentMigration =
        (SELECT TOP (1) 
        [Project1].[MigrationId] AS [MigrationId]
        FROM ( SELECT 
        [Extent1].[MigrationId] AS [MigrationId]
        FROM [dbo].[__MigrationHistory] AS [Extent1]
        WHERE [Extent1].[ContextKey] = N'SysDev.Migrations.Configuration'
        )  AS [Project1]
        ORDER BY [Project1].[MigrationId] DESC)

IF @CurrentMigration IS NULL
    SET @CurrentMigration = '0'

IF @CurrentMigration < '201709070459006_InitialCreationModel'
BEGIN
    CREATE TABLE [dbo].[AspNetRoles] (
        [Id] [nvarchar](128) NOT NULL,
        [Name] [nvarchar](256) NOT NULL,
        CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY ([Id])
    )
    CREATE UNIQUE INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]([Name])
    CREATE TABLE [dbo].[AspNetUserRoles] (
        [UserId] [nvarchar](128) NOT NULL,
        [RoleId] [nvarchar](128) NOT NULL,
        CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId])
    )
    CREATE INDEX [IX_UserId] ON [dbo].[AspNetUserRoles]([UserId])
    CREATE INDEX [IX_RoleId] ON [dbo].[AspNetUserRoles]([RoleId])
    CREATE TABLE [dbo].[AspNetUsers] (
        [Id] [nvarchar](128) NOT NULL,
        [Email] [nvarchar](256),
        [EmailConfirmed] [bit] NOT NULL,
        [PasswordHash] [nvarchar](max),
        [SecurityStamp] [nvarchar](max),
        [PhoneNumber] [nvarchar](max),
        [PhoneNumberConfirmed] [bit] NOT NULL,
        [TwoFactorEnabled] [bit] NOT NULL,
        [LockoutEndDateUtc] [datetime],
        [LockoutEnabled] [bit] NOT NULL,
        [AccessFailedCount] [int] NOT NULL,
        [UserName] [nvarchar](256) NOT NULL,
        CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY ([Id])
    )
    CREATE UNIQUE INDEX [UserNameIndex] ON [dbo].[AspNetUsers]([UserName])
    CREATE TABLE [dbo].[AspNetUserClaims] (
        [Id] [int] NOT NULL IDENTITY,
        [UserId] [nvarchar](128) NOT NULL,
        [ClaimType] [nvarchar](max),
        [ClaimValue] [nvarchar](max),
        CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY ([Id])
    )
    CREATE INDEX [IX_UserId] ON [dbo].[AspNetUserClaims]([UserId])
    CREATE TABLE [dbo].[AspNetUserLogins] (
        [LoginProvider] [nvarchar](128) NOT NULL,
        [ProviderKey] [nvarchar](128) NOT NULL,
        [UserId] [nvarchar](128) NOT NULL,
        CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey], [UserId])
    )
    CREATE INDEX [IX_UserId] ON [dbo].[AspNetUserLogins]([UserId])
    ALTER TABLE [dbo].[AspNetUserRoles] ADD CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE
    ALTER TABLE [dbo].[AspNetUserRoles] ADD CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
    ALTER TABLE [dbo].[AspNetUserClaims] ADD CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
    ALTER TABLE [dbo].[AspNetUserLogins] ADD CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
    CREATE TABLE [dbo].[__MigrationHistory] (
        [MigrationId] [nvarchar](150) NOT NULL,
        [ContextKey] [nvarchar](300) NOT NULL,
        [Model] [varbinary](max) NOT NULL,
        [ProductVersion] [nvarchar](32) NOT NULL,
        CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY ([MigrationId], [ContextKey])
    )
    INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
    VALUES (N'201709070459006_InitialCreationModel', N'SysDev.Migrations.Configuration',  0x1F8B0800000000000400DD5C5B6FE3B6127E3FC0F90F829E7A0E522B97B38B6D60EF2275929EA09B0BD6D9A26F0B5AA21D61254A95A83441D15FD687FEA4FE850E254A166FBAD88AED140B2C2272F8CD70382487C3A1FFFAE3CFF187A730B01E7192FA1199D847A343DBC2C48D3C9F2C27764617DFBEB33FBCFFF7BFC6175EF864FD54D29D303A6849D289FD40697CEA38A9FB8043948E42DF4DA2345AD0911B850EF222E7F8F0F03BE7E8C8C100610396658D3F6584FA21CE3FE0731A1117C73443C175E4E120E5E55033CB51AD1B14E234462E9ED8B3E7F41C3F8E0A42DB3A0B7C0442CC70B0B02D4448441105114F3FA7784693882C673114A0E0FE39C640B740418AB9E8A72BF2AEBD383C66BD70560D4B28374B6914F6043C3AE16A71E4E66B29D7AED4068ABB0005D367D6EB5C7913FBCAC379D1A7280005C80C4FA741C28827F675C5E22C8D6F301D950D4705E4650270BF46C9D7511DF1C0EADCEEA032A3E3D121FB77604DB38066099E109CD1040507D65D360F7CF747FC7C1F7DC5647272345F9CBC7BF31679276FFF874FDED47B0A7D053AA1008AEE9228C609C8861755FF6DCB11DB3972C3AA59AD4DA115B0259811B6758D9E3E62B2A40F30578EDFD9D6A5FF84BDB2841BD767E2C304824634C9E0F3260B02340F7055EF34F264FF37703D7EF37610AE37E8D15FE6432FF1878993C0BCFA8483BC367DF0E3627A09E3FD85935D2651C8BE45FB2A6ABFCCA22C7159672223C93D4A96988AD28D9D95F17632690635BC5997A8FB6FDA4C52D5BCB5A4AC43EBCC8492C5B6674329EFCBF2ED6C7167710C83979B16D34893C109FBD4486A786015D52B8339EA6A30043AF24F5EFF2E42E407032C801DB880DBB1F0931057BDFC3E027343A4B7CC77284D61FE7BFF47E94383E8F0E700A2CFB09B256096338AC2F8C5B9DD3D4404DF64E19C59FBF6780D3634F7BF4697C8A551724158AB8DF13E46EED728A317C43B47147FA66E09C83EEFFDB03BC020E29CB92E4ED34B3066EC4D23F0AA4BC02B424F8E7BC3B1B569D70EC834407EA8F740A455F44B49BAF242F4148A276220D379234DA27E8C963EE9266A496A16B5A068159593F5159581759394539A05CD095AE52CA806F3EFF2111ADEC1CB61F7DFC3DB6CF336AD053535CE6085C43F60821358C6BC3B44294EC86A04BAAC1BBB7016F2E1634C5F7C6FCA39FD84826C68566BCD867C11187E36E4B0FB3F1B7231A1F8D1F79857D2E1D85312037C277AFD89AA7DCE49926D7B3A08DDDC36F3EDAC01A6E97296A691EBE7B34013F0E2E10A517EF0E1ACF6D845D11B39FE011D0343F7D9960725D0375B36AA5B728E034CB175E61601C1294A5DE4A96A840E793D042B77548D60AB388828DC7F159E60E938618D103B04A530537D42D569E113D78F51D0AA25A965C72D8CF5BDE221D79CE31813C6B055135D98EBC31E4C808A8F34286D1A1A3B358B6B364483D76A1AF336177635EE4A34622B36D9E23B1BEC92FB6F2F6298CD1ADB827136ABA48B00C610DE2E0C949F55BA1A807C70D93703954E4C0603E52ED5560C54D4D80E0C5454C9AB33D0E288DA75FCA5F3EABE99A77850DEFEB6DEA8AE1DD8A6A08F3D33CDC2F78436145AE04435CFF339ABC44F5473380339F9F92CE5AEAE6C220C7C86A918B259F9BB5A3FD46906918DA8097065682DA0FCF24F015226540FE1CA585EA374DC8BE8015BC6DD1A61F9DA2FC1D66C40C5AE5F82D608CD57A5B271763A7D543DABAC4131F24E87851A8EC620E4C54BEC7807A598E2B2AA62BAF8C27DBCE15AC7F8603428A8C5733528A9ECCCE05A2A4DB35D4B3A87AC8F4BB6919624F7C9A0A5B233836B89DB68BB92344E410FB7602315895BF84093AD8C7454BB4D5537768AB4285E30760CF953E36B14C73E59D6F2A97889352B92A9A6DFCEFAA71A850586E3A69A8CA34ADA8A138D12B4C4522DB006492FFD24A5E788A23962719EA9172A64DABDD5B0FC972CEBDBA73A88E53E5052B3BFF9CDAA70652F6CB3AA1FC29B5F42E742E6CCE41174CDD0EB9B5B2CB50D0528D104EDA7519085C4EC5B995B175777F5F645898A307624F915DF495194E2E18A5AEF3426EA7CD87C7C2AAF65FD31324398345DFA9C755D9BFC50334A1996AAA39842553B1B3393FBD2759C64A7B0FF30B522BCCC6CE2992875005ED413A396CCA080D5EABAA38AF926754CB1A63BA29454528794AA7A48594F1D1184AC57AC8567D0A89EA23B073559A48EAED67647D6A48DD4A135D56B606B6496EBBAA36A324BEAC09AEAEED8AB341379FDDCE3FDCA78545967C32A0EB29BED58068C97590C87D9F06AF7F575A05A714F2C7E23AF80F1F2BD3424E3696E1D432A42179B199201C3BCDE0897DCE272D378336FC6146EAE8525BDE9E6DE8CD7CF5C5FD42894739C4C5271AFCE73D2B96DCCCF50ED8F6394435541625BA51A7353A2381C3182D1EC97601AF8982DDE25C13522FE02A7B4C8D6B08F0F8F8EA54736FBF3E0C549532FD09C414DAF5EC431DB42E215794489FB8012350D628347212B5025C27C453CFC34B17FCB5B9DE6C10AF6575E7C605DA59F89FF4B0615F74986ADDFD5B4CE6192E49B4F557BFAA4A1BB56AF7EFE52343DB06E139831A7D6A1A4CB7546587CE8D04B9AA2E906D2ACFDFCE1F54E28E1958116559A10EB3F2A98FB74900705A594DF84E8E93F7D45D33E1AD80851F3306028BC4154684AFC5F07CB98F4EFC127CD93FEFB7556FF08601DD18C0F007CD21F4C4EFFEFBE0C952D77B8D5688E43DB5892723DB7A64F6F944BB9EBBD49C9B2DE68A2AB99D43DE036C8965EC3325E59A2F160BBA3268F7830EC5D9AF68B270FEF4BBEF02A9363B769C2DBCC0C6EB807FA472504EF410A9B262567F769BFDBB635530877CF7327FB25F7EE99B1F144ADDDA7F06EDBD84C61DE3D37B65E89BA7B666BBBDA3F776C699DB7D09DA7DDAA194486AB185D2CB82DADB6089CC3097F1E8111141E65F11A529FC7D59483DAC2704562666A4E2093192B1347E1AB5034B3EDD757BEE1377696D334B335A45D36F1E6EB7F236F4ED3CCDB90CCB88B84606D3AA12E49BB651D6BCA7A7A4D09C0424F5AF2CDDB7CD6C67BF5D794EF3B885284D963B8237E3DE9BD83A864C8A9D3239D57BDEE85BDB3F68B89B07FA7FE7205C17E3F916057D8352B9A2BB288CACD5B92A824912234D798220FB6D4B384FA0BE452A86631E6FC39771EB763371D73EC5D91DB8CC619852EE3701E08012FE60434F1CF73964599C7B771FECB24437401C4F4596CFE967C9FF98157C97DA9890919209877C123BA6C2C298BEC2E9F2BA49B887404E2EAAB9CA27B1CC60180A5B764861EF13AB281F97DC44BE43EAF22802690F68110D53E3EF7D1324161CA3156EDE1136CD80B9FDEFF0D58E9F6E338540000 , N'6.1.3-40302')
END

IF @CurrentMigration < '201709080327419_AddedModels'
BEGIN
    CREATE TABLE [dbo].[AuditTrails] (
        [Id] [int] NOT NULL IDENTITY,
        [ModuleId] [int] NOT NULL,
        [PageId] [int] NOT NULL,
        [Action] [nvarchar](max),
        [UserProfileId] [int] NOT NULL,
        [Description] [nvarchar](max),
        [DateCreated] [datetime] NOT NULL,
        CONSTRAINT [PK_dbo.AuditTrails] PRIMARY KEY ([Id])
    )
    CREATE INDEX [IX_UserProfileId] ON [dbo].[AuditTrails]([UserProfileId])
    CREATE TABLE [dbo].[UserProfiles] (
        [Id] [int] NOT NULL IDENTITY,
        [FirstName] [nvarchar](50) NOT NULL,
        [LastName] [nvarchar](50) NOT NULL,
        [MiddleName] [nvarchar](50) NOT NULL,
        [Address] [nvarchar](50) NOT NULL,
        [ContactNo] [nvarchar](50) NOT NULL,
        [CompanyName] [nvarchar](50) NOT NULL,
        [CompanyId] [nvarchar](50) NOT NULL,
        [Gender] [nvarchar](50) NOT NULL,
        [MaritalStatus] [nvarchar](50) NOT NULL,
        [DateCreated] [nvarchar](50) NOT NULL,
        CONSTRAINT [PK_dbo.UserProfiles] PRIMARY KEY ([Id])
    )
    CREATE TABLE [dbo].[MasterDatas] (
        [Id] [int] NOT NULL IDENTITY,
        [Name] [nvarchar](max),
        [Description] [nvarchar](max),
        [Status] [nvarchar](max),
        [OrderNumber] [int] NOT NULL,
        [DateTimeCreated] [datetime] NOT NULL,
        [DateTimeUpdated] [datetime] NOT NULL,
        CONSTRAINT [PK_dbo.MasterDatas] PRIMARY KEY ([Id])
    )
    CREATE TABLE [dbo].[MasterDetails] (
        [Id] [int] NOT NULL IDENTITY,
        [Name] [nvarchar](max),
        [Value] [nvarchar](max),
        [Description] [nvarchar](max),
        [Status] [nvarchar](max),
        [OrderNumber] [int] NOT NULL,
        [DateTimeCreated] [datetime] NOT NULL,
        [DateTimeUpdated] [datetime] NOT NULL,
        [MasterDataId] [int] NOT NULL,
        CONSTRAINT [PK_dbo.MasterDetails] PRIMARY KEY ([Id])
    )
    CREATE INDEX [IX_MasterDataId] ON [dbo].[MasterDetails]([MasterDataId])
    CREATE TABLE [dbo].[Permissions] (
        [Id] [int] NOT NULL IDENTITY,
        [IdentityRoleId] [nvarchar](128),
        [MasterDetailId] [int] NOT NULL,
        [AllowView] [int] NOT NULL,
        [AllowCreate] [int] NOT NULL,
        [AllowEdit] [int] NOT NULL,
        [AllowDelete] [int] NOT NULL,
        [AllowGenerateReport] [int] NOT NULL,
        CONSTRAINT [PK_dbo.Permissions] PRIMARY KEY ([Id])
    )
    CREATE INDEX [IX_IdentityRoleId] ON [dbo].[Permissions]([IdentityRoleId])
    CREATE INDEX [IX_MasterDetailId] ON [dbo].[Permissions]([MasterDetailId])
    ALTER TABLE [dbo].[AspNetUsers] ADD [Status] [nvarchar](max)
    ALTER TABLE [dbo].[AspNetUsers] ADD [UserProfileId] [int] NOT NULL DEFAULT 0
    CREATE INDEX [IX_UserProfileId] ON [dbo].[AspNetUsers]([UserProfileId])
    ALTER TABLE [dbo].[AspNetUsers] ADD CONSTRAINT [FK_dbo.AspNetUsers_dbo.UserProfiles_UserProfileId] FOREIGN KEY ([UserProfileId]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE CASCADE
    ALTER TABLE [dbo].[AuditTrails] ADD CONSTRAINT [FK_dbo.AuditTrails_dbo.UserProfiles_UserProfileId] FOREIGN KEY ([UserProfileId]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE CASCADE
    ALTER TABLE [dbo].[MasterDetails] ADD CONSTRAINT [FK_dbo.MasterDetails_dbo.MasterDatas_MasterDataId] FOREIGN KEY ([MasterDataId]) REFERENCES [dbo].[MasterDatas] ([Id]) ON DELETE CASCADE
    ALTER TABLE [dbo].[Permissions] ADD CONSTRAINT [FK_dbo.Permissions_dbo.AspNetRoles_IdentityRoleId] FOREIGN KEY ([IdentityRoleId]) REFERENCES [dbo].[AspNetRoles] ([Id])
    ALTER TABLE [dbo].[Permissions] ADD CONSTRAINT [FK_dbo.Permissions_dbo.MasterDetails_MasterDetailId] FOREIGN KEY ([MasterDetailId]) REFERENCES [dbo].[MasterDetails] ([Id]) ON DELETE CASCADE
    INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
    VALUES (N'201709080327419_AddedModels', N'SysDev.Migrations.Configuration',  0x1F8B0800000000000400ED5DD96EDCCA117D0F907F20F89404BA1A2DB1E108D2BDD095AC4488B5C0231B79135A646B4498CB5C2EB284205F96877C527E21CD6DD8FB42F670388EE017AB9753CDEAAAEAEEEA9EAAFFFEFB3FC7BFBC44A1F30CD32C48E213777F77CF7560EC257E102F4EDC227FFCE983FBCBCFBFFFDDF1473F7A71BEB6ED0ECB76A8679C9DB84F79BE3C9ACD32EF094620DB8D022F4DB2E431DFF5926806FC6476B0B7F797D9FEFE0C2208176139CEF1E722CE8308567FA03FCF92D883CBBC00E155E2C3306BCA51CDBC4275AE4104B325F0E0893B7FCDCEE1F36EDDD0754EC300A041CC61F8E83A208E931CE46888475F3238CFD3245ECC97A8008477AF4B88DA3D823083CDD08FBAE6BA5FB177507EC5ACEBD842794596279121E0FE61C39619DDBD1773DD15DB10E33E2206E7AFE55757CC3B714F0B3FC8EF521084AE43933B3A0BD3B229C5DDDDAECF8E53D7ECAC6400894AF96FC7392BC2BC48E1490C8B3C05A8E56DF11006DEDFE1EB5DF20DC627711186F8D0D0E0501D51808A6ED36409D3FCF5337C6C067CE9BBCE8CEC37A33BAEBA617DEA0FB98CF3C303D7B946C4C1430857338F7DF43C4F52F85718C314E4D0BF05790ED3B8C48015EF18EA142DC4A322846A8A72945BB0188C71EAD532D34C619E22E5759D2BF0F209C68BFC090D15BCB8CE45F002FDB6A4C1FD120748D751A73C2D9464904AA5A8E83118FED5E730F3D26039CAB0CFD1EC9EA5B09CE39656597487EC8F7ADCD7E0395854E2226686EB7C8661D5267B0A96B52DC254E79E687A9126D1E7242414126F713F4F8AD4430DEF1269B33B902E604E8EF778D669BDD4161023D2350658A7376BC0D0BA08D22C2FFF2B91E7777B5AE26CA84B9FC086085F05BE1FC28D903EF5FD1466D9E874D14625075E7E9D6C8072B404F1EB46B8DDD0EED46934CA481F7D988E2FD9200D7210CE916928C61732CE82B526DADA0BC615B23130450303FAEB45D7E76DB9606829F4D8D2CE67C45D965257EC90B949913DB82EA287CE2AF4DC81365BC0DE3B4301DE97A5DF03CF5411616E748EC37BBD29E36694F12B088B37957F5379315EB7601A1FAF85C7547CE1E69C5271C3708FB7ED8EA98226CC3955D46ED041F516A65190659534EB1ABBAECF9BA9E3D0AADB957326DDCFEF1F7CB0A0BBB8500CF6728561F2FD6B00BF5B80A94D8005A08F7E905B80398721B4329E56323EC365929A8E4C684570A9E1DA914EE7EEC9B69D1D113461EC88A81DCF8E6858BE66A7241F33D9963B66C2BC49C64CB41B64FB48468AADDFD5EA4EE0345B5EC37CB7EDB85B435EA408EE7B927EDBC511771CED7E9DF93CD0359F87FB0F8F871FDEBD07FEE1FB3FC3C377E39BD20126CD50EF14BBC78377EFAD50957AA233AE8413FAD334EB849BAD65E49AD3C48A489750F6C5BA459DBE68972365C59BDBB45DA84D35A12531B63658DB58D8392F9F2E9768F22AD12A396270F54976DCBAADE478333ED231D0E2EDE3C7A85AE7079B6C0D2A6749FC18A451773AFC35410A02E21EF7C459862C96FF37903DADFF640FBD22458A84A6365AAE9DDAED531243F2803F062D6B5373F73DB9001E3A977D8CCB5E83F13E25DEB7A4C83FC67EE94AF8927BAC674113C0CA704E3D0F66D9051266E89F25453CF0BC53EAF1A6B74C67210822FE9E89B2FBF76D53ECDE9EDB82BDB7E737333DC67C4A1641AC37D4B6A978A8750BE5509B66A6432DC1F446DAB4140FB46AA01C67DDCA7498CA971B1415D1F30D7133E5C0AD3DE4C0B7BE9580D9DF5157B0D3DF524FDDF1B6A9FD78357D25D1B52FAD15A575DC72F4D286CA86D9D7860A76FADA500D13153F07D5F3098D7366DB18C16BB5E71F61D53A478D6C6C75203E736CE2E3D80091BA9C6659E2059516304F8289458EFC06B40D75F41E2CD65F453E7E449F87C43D28973F34A413F74F0C8F94F8ED0289E1134B324960DFA565F726AEBDEB4EFD4C16192A9079C067670BF1CD274B90B8C3B4546C501EE432A4AE419CB3BA11C45EB004A1D66750BD35D7B272742B3A74CD395C96CFA4E25C6B9E7406409DB5D9B1AC48521C5431EC788649A15C3845579322E951DE5376E243BAFBF5055475C5C992A8EE52A726A08ACF1841421573A53302F2B27C2312CA71AF8B2447E66BEF8486BCF159A3D84807C69166D66FAF529A5E8229E6D2083229E6840E71BE9B7E244114DDBE8A265D7915DBCD3CFE0043DF50AAEE7035657E6F7777DF8E6429063482782978AE3702F2EDC6A6658D58463544817F856E51D6B877EF9AEBFEE69765C5878C2BA3BCB9325896570F7E3622A3021FAEF0DCA170E862E71AFA367194355AE14916ACD38D3B702D0BB59C63631C71A42CD13DE24C46401BCFBDAE00D06EFCA90928757F2010D0C643378A80921CDB8080922CD93A01AD2F6C74E79FBABD999A7892D746E31F73A4ECDA806C12FCD83AD1D4F25CEADCD5698BA9D487A971D3B7358E4CF5B76C405AB7C6A5597BE1AB5F0C07314C59F13D7F282BE14BCEB9A642C36D6EAAB2C6E94F4B5C093E8739E3CACF5CA7F3FF73FCF18CE89240189F784884C02AA03A171D0F09F790EA0155870A095473AE538075871C1E147E065500358F181808D2A3A100A1971C1960B72C69CC21570C68BB6630B8F61D8C7474CD99C300B67DB322856D768A142CA6763C3D20DF6D608DE5013A68BBA07DFDB5FA4A4A19194BA37DE18521925A499B7B92131A5C12FEEC8B6593D6358CD1450CF65994624B58A5BA7A61416BB3339855BC37F42C97545701BA9701D86734D640C21389EF1EC3E15898C14C11FEDC87E58C966FDAC83B8D7D1B61CB259C52F9A3556C1FC622727992B248EC523572AA0E6311D78DAAABB53D58257A72C9B1D91A8E3D13D71E656115D65AEE8613A85CFB31D6B9D4AE9C6A2EF1BC4B26FEA5415CA27C41022EB51F639D4B8D3AAB99C4F17018F83806B188F447ACD9744B5FB7AAD9A4B955D2386F0F62D97AB64BEDCBA9D5996D55773CAB03513605C73341C4CAE32BB05C06F1028B60D99438F33A7CE5D94F73F3E08E518D31F30809A54F982B4A79928205A46ACB304E3EACC294957BA30750BE1B3BF323A619F7842AD8D1B724D943283B97ED06BFED53FEBFF9B98920E824E760DF74BE401F18953E82EA552E77EBCD7676CA70A2200429E719F0591216512C7658887B776120718CAE541FA90D0589E3B465FA286D30481CA52DD347A1DC253898C29322C6246293E0884485011E1E278BC0C32B58BCE31925458C2F881156C6BF46CABF96761046A9BF7A482CB0867E487BAF4741B0C888380856AC8FD5C53AC4A1BA5203B5C58217128A8B951B285D1B8F90D0BAB6501F078B2F882361C5265858C440120DAB30C6A385012BD6C76AE3FAE1406D99C11C9261FA886924AB7E58A3823B3BFADB14B15F47C3A4C83AAFC7A2B0226D2ACBB697229E089ACB1E11E90A87222ACC6499087845CB3351698EBB0A7CC5C35D554E4D571ABFC5606DE13A69F4F545D07DAA1AD3FCB00B87688ADEB46E3BB44EBC8AE22FE9C94554F6C67E637A8CBB34FB6B3176D367AEC3B2CEEBD160FAF12F89247F18AC9CFBD5734DCEEC0B9F728A51B1486DC47EB82B36C46AC3B531686D85215E1DB58D41AB8B0DB1DAD06D0C5A5B61884747706370E90693D14B910B55572365F7431A3A29EFBEE975754373C2BA9087CFCFEA1D42FF391243C81C523C4F94D97CF1ACA7D86A6ED05134C8814A3DF3E8E1455521AC479BECECE7D6E1BA6C2246E1584D91210616748801C3EA4C9CC6785C28D2758CD718CC0219FC89980CB2CA6094788827629078452F3C0147F92DF429B0419D7074B6D6C087C98677229C996C750F6CCE98E93A932B0526021479BBC0549B692BBB8A76A593B1CA9C4B7D3B4B69FD686ED85A2AC0588F99B6B314638169087772576C88C5F150E0E5931424E1D38C3E82543F931C2648020CB1BD21A2B990E6461A82468C498468214CBA2C448D18CF4C5CD72A14CC0303BAC98A7A53B2FA7BF5C0A0B9DC57E7C9646EFBEB26AED3B2B112A51C46BB6583DDF96FE15918C0D278B70DAE401C3CC22CAFC312B9077BFB0754BECDE9E4BE9C65991F721E47E0A160054F03C6882F16947C55461033CD174125A0AC880C4A3FD907814C3E193F83D47B02E91F22F0F2471CAB7788D76A4CCCEF232E631FBE9CB8FFAC7A1E3997FFB8273AEF3895F3F6C8D973FE652339E5A0CFE2E4F32A9DB4B9C5B440E21F416DAB7033F914DB296092A00D4C97680B97CD86680B994A76680B96C965680F9849556819BA933D5BC064A2416B32C1CB23680B9C6356FA41F7C902F86318199E88F633F196970CBEC4F482E2A4CFEAB3CC0B9267692E64623C2A7996ED855112A3E6FF5E688958AD6FE2BF45E2CF5B69D8DC71BA3B67BCAFC1C6B94F12B71F4303F949D4BAC55783E72404C9753A20AC99C0F353AE19CA42D37BD0318AC9D7D6EB74C9666BEB0D83E76AEB0D42666AEB0DC3CFD3A686EB973D6C74B5E36E442961EEBDD8A905B9FCE8F27F55F18E73997D8983DF0A547187148852B12AD9845D9EF36FB7279A1ACAC86CD55D75CD558F845146A331339E36D3486DAF4259DC5A4DC45D476473E2F28952F1FEC99B1E823E1E563671D3B0CD312F39D320444E02265B785658284AB0D4074B985C89B7F5D6F9587EB2A53E4313265AEAB3CBE86ED04D0D6BDB73838B27E73A7B6B0F0BD35A6D99743083149D4DF962003720AD4B0FC9D8B28C28D6D6FB5B36E18935EC4D8AF6DAB29C100111B080546385EAE38587EA9D5CA557DC3FF92F8C758CAED32F5DC94403F959CB4D5285D8C26672A42421FC285A0332A20CC93AC2FF8DA975A152FC468FA5B7BDF944A69242A40B4F2808E33752E690319385487E0AA1EBAF1949B86E0D7384F492306EC20E5E8CB1DE791FB6423AA43F2DE45193FF7C6F92793D7A2D256BB2409B922BE345665CB9FA0172714C207A3C27BEAD607D1B31E3C6D86B9CE8370A06F717D3CFAB3131616BA21E0B846DC4EC19630B9BE8770C131736A31C191393B54DEDDB372C69DA1BC24909DA9679B1FA0BFE96FAB37AC8F4F84E2DEA8748ABF7D57488507A5A9BDF3631BE5545D689FAC74727AEFF503E37AFBDBA9230F9B2B414AAAC143C52D280B0342DDC2DC790C22B799464D1E705849AA3928854532D212688C44D93C3CF430C31BC9247EA561C475C98E0A15E38185264355732B2E535CCF9218F45C4BA954A48B06B22262A8EB5CCC83E6DBC5805A05BC8C99A7D6B73DA907E6CD3464E5610A15C46BBD97C4A69376DE4B40571BFC74EEDC1B12E4CB454D628F36F6AB0EE92B43BD3CCD981993C3AB4A3ECF379768C0D75379C056BCEC5C18DBDCE4BB8A3D8467350BACAC14C103972ADE4DE18CE0E7691A103A6596581FDDC1ABD847974168C973383B468F20C522AC7994033F869A3A69C22C30A538855541089C13E53D69511C30A4BC630A0A364C018BCA3E8CF5C3B092ED83813E8B059C4E50BC9FAAF7398058B0EE21861C6D0238E99AB3697F163D21E7AA911B54DE85FB12033EBA37DC7699A078FC0CB5175F938328817AE533D382B9FE83E40FF32BE29F26591A34F86D14348BCD42A4FCD32FA55160F72CCC737D58FD8321B9F808619948F4A6FE25F8B20F457E3BEE03C66124094C7F1E62962399779F92471F1BA42BA4E624DA0867D2B2FC21D8C962102CB6EE23978867DC68644EF135C00EFB57BBA2602514F04C9F6E3F3002C5210650D46D71FFD8964D88F5E7EFE1FC0B635F8BCB80000 , N'6.1.3-40302')
END

