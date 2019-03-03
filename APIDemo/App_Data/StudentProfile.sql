use Student;
GO

create table StudentProfile
(
[guid] uniqueidentifier not null default NEWSEQUENTIALID() primary key,
Id varchar(12),
Name nvarchar(50),
Gender varchar(2),
Blood varchar(3),
Height decimal(6,2),
Weight decimal(6,2),
Coupon varchar(15),
CreateDate datetime default getdate(),
UpdateDate datetime
);

create index StudentProfile_Idx1_Id on StudentProfile(Id);

create type TVP_StudentProfile as table
(
[guid] uniqueidentifier not null default NEWSEQUENTIALID() primary key,
Id varchar(12),
Name nvarchar(50),
Gender varchar(2),
Blood varchar(3),
Height decimal(6,2),
Weight decimal(6,2),
Coupon varchar(15),
CreateDate datetime default getdate(),
UpdateDate datetime
);